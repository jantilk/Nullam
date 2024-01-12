import {Button, Col, Container, Modal, Row, Stack, Table} from "react-bootstrap";
import {NavLink, useParams} from "react-router-dom";
import {InvalidateQueryFilters, useMutation, useQuery, useQueryClient} from "@tanstack/react-query";
import {toast} from "sonner";
import queryKeys from "../../api/queryKeys.ts";
import AddParticipants from "./components/AddParticipants";
import {format} from "date-fns";
import socialEventCompaniesApi from "../../api/socialEventCompaniesApi.ts";
import {AxiosError, HttpStatusCode} from "axios";
import socialEventPersonsApi from "../../api/socialEventPersonsApi.ts";
import {useMemo, useState} from "react";
import socialEventsApi from "../../api/socialEventsApi.ts";
import constants from "../../utils/constants.ts";

export interface CurrentParticipant {
  id: string | null;
  name: string;
  type: ParticipantType
}

type ParticipantType = 'company' | 'person' | null

export default function Participants() {
  const {eventId} = useParams();
  const [currentParticipant, setCurrentParticipant] = useState<CurrentParticipant>({id: null, name: "", type: null});
  const [showModal, setShowModal] = useState(false);
  const queryClient = useQueryClient();

  const ConfirmationModal = () => (
    <Modal show={showModal} onHide={() => setShowModal(false)}>
      <Modal.Header closeButton>
        <Modal.Title>Kustuta "<strong>{currentParticipant.name}</strong>"</Modal.Title>
      </Modal.Header>
      <Modal.Body>Oled kindel, et soovid osalejat kustutada?</Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={() => setShowModal(false)}>Tühista</Button>
        <Button variant="danger" onClick={handleDelete}>Kustuta</Button>
      </Modal.Footer>
    </Modal>
  );

  const {data: socialEvent, error: getSocialEventError} = useQuery({
    queryKey: [queryKeys.SOCIAL_EVENT, eventId],
    queryFn: () => {
      return socialEventsApi.getById(eventId)
    },
    select: (response) => {
      if (response) {
        return response.data;
      }
      return null;
    },
    enabled: !!eventId
  });

  const {data: companies, error: getCompaniesError, isLoading: isCompaniesLoading} = useQuery({
    queryKey: [queryKeys.COMPANIES_BY_SOCIAL_EVENT_ID, socialEvent?.id],
    queryFn: () => {
      const id = socialEvent?.id;
      if (typeof id === 'string') {
        return socialEventCompaniesApi.getBySocialEventId(id);
      } else {
        throw new Error("Social event ID is undefined");
      }
    },
    select: (response) => {
      if (response) {
        return response.data;
      }
      return null;
    },
    enabled: !!socialEvent?.id,
    refetchOnMount: "always",
    staleTime: 0
  });

  const {data: persons, error: getPersonsError, isLoading: isPersonsLoading} = useQuery({
    queryKey: [queryKeys.PERSONS_BY_SOCIAL_EVENT_ID, socialEvent?.id],
    queryFn: () => {
      const id = socialEvent?.id;
      if (typeof id === 'string') {
        return socialEventPersonsApi.getBySocialEventId(id);
      } else {
        throw new Error("Social event ID is undefined");
      }
    },
    select: (response) => {
      if (response) {
        return response.data;
      }
      return null;
    },
    enabled: !!socialEvent?.id,
    refetchOnMount: "always",
    staleTime: 0
  });

  if (getSocialEventError || getCompaniesError || getPersonsError) {
    const axiosError = getCompaniesError as AxiosError;

    const isError = axiosError && axiosError.response?.status !== HttpStatusCode.NotFound;
    if (isError) {
      toast.error(constants.ERROR_TEXT.SOMETHING_WENT_WRONG);
    }
  }

  const combinedParticipants = useMemo(() => {
    const companyParticipants = companies?.map(company => ({
      id: company.id,
      createdAt: company.createdAt,
      primaryText: company.name,
      secondaryText: company.registerCode,
      participantType: 'company' as const
    })) ?? [];

    const personParticipants = persons?.map(person => ({
      id: person.id,
      createdAt: person.createdAt,
      primaryText: `${person.firstName ?? ''} ${person.lastName ?? ''}`.trim(),
      secondaryText: person.idCode,
      participantType: 'person' as const
    })) ?? [];

    return [...companyParticipants, ...personParticipants]
      .sort((a, b) => {
        // Convert dates to timestamps. Invalid dates become NaN, which is handled in the comparison
        const dateA = new Date(a.createdAt).getTime();
        const dateB = new Date(b.createdAt).getTime();

        // Handle NaN values (invalid dates)
        if (!dateA) return 1;
        if (!dateB) return -1;

        return dateA - dateB;
      });
  }, [companies, persons]);

  const openModal = (participantId: string, participantName: string, participantType: ParticipantType) => {
    setCurrentParticipant({id: participantId, name: participantName, type: participantType});
    setShowModal(true);
  };

  const deleteParticipantMutation = useMutation({
    mutationFn: ({eventId, participantId}: { eventId: string, participantId: string }) => {
      return socialEventPersonsApi.delete(eventId, participantId);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([queryKeys.PERSONS_BY_SOCIAL_EVENT_ID, socialEvent?.id] as InvalidateQueryFilters);
      toast.success("Osaleja kustutatud");
    },
    onError: () => {
      toast.error("ürituse kustutamine ebaõnnestus");
    }
  });

  const deleteCompanyParticipantMutation = useMutation({
    mutationFn: ({eventId, participantId}: { eventId: string, participantId: string }) => {
      return socialEventCompaniesApi.delete(eventId, participantId);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([queryKeys.COMPANIES_BY_SOCIAL_EVENT_ID, socialEvent?.id] as InvalidateQueryFilters);
      toast.success("Osaleja kustutatud");
    },
    onError: () => {
      toast.error("ürituse kustutamine ebaõnnestus");
    }
  });

  const handleDelete = () => {
    if (!socialEvent?.id || !currentParticipant.id) {
      toast.error('Something went wrong');
      setShowModal(false);
      return;
    }

    if (currentParticipant.type === 'person') {
      deleteParticipantMutation.mutate({eventId: socialEvent?.id, participantId: currentParticipant.id});
    }

    if (currentParticipant.type === 'company') {
      deleteCompanyParticipantMutation.mutate({eventId: socialEvent?.id, participantId: currentParticipant.id});
    }

    setShowModal(false);
  }

  return (
    <Container className={"shadow-sm bg-white"}>
      <Row>
        <Col className={"bg-primary d-flex align-items-center justify-content-center"} md={6}><h1 className={"text-white"}>Osavõtjad</h1></Col>
        <Col className={"background-image-col"}/>
      </Row>
      <Row className={"justify-content-center"}>
        <Col md={16} lg={12} className={"p-4 mb-5"}>
          <Row className={"mb-3"}><Col><h2 className={"text-primary"}>Osavõtjad</h2></Col></Row>
          <Row>
            <Col>
              <Stack gap={3}>
                <Row>
                  <Col sm={24} md={8}>Ürituse nimi:</Col>
                  <Col md={16}>{socialEvent?.name}</Col>
                </Row>
                <Row>
                  <Col sm={24} md={8}>Toimumise aeg:</Col>
                  <Col md={16}>
                    {socialEvent?.date && format(new Date(socialEvent?.date), 'dd.MM.yyyy HH:mm')}
                  </Col>
                </Row>
                <Row>
                  <Col sm={24} md={8}>Koht:</Col>
                  <Col md={16}>{socialEvent?.location}</Col>
                </Row>
                <Row>
                  <Col>
                    <Row><Col sm={24} md={8}>Osavõtjad:</Col></Row>
                    {!isCompaniesLoading && !isPersonsLoading && (
                      <Row className={"mb-5"}>
                        <Col xl={{span: 16, offset: 8}}>
                          <Table borderless className={"participants-table"}>
                            <tbody>
                            {combinedParticipants.map((participant, index) => (
                              <tr key={participant.id}>
                                <th scope={"row"} className={"text-end px-0 table-col-min-width"}>{index + 1}.</th>
                                <td>{participant.primaryText}</td>
                                <td className={"table-col-min-width"}>{participant.secondaryText}</td>
                                <td className={"table-col-min-width"}>
                                  <NavLink
                                    className={"nav nav-link py-0"}
                                    to={`/social-events/${socialEvent?.id}/participants/${participant.participantType === 'company' ? 'companies' : participant.participantType + 's'}/${participant.id}`}
                                  >
                                    VAATA
                                  </NavLink>
                                </td>
                                <td className={"table-col-min-width"}>
                                  <Button
                                    type={"button"}
                                    className={"btn btn-link py-0 d-flex"}
                                    onClick={() => openModal(participant.id, participant.primaryText, participant.participantType)}
                                  >
                                    KUSTUTA
                                  </Button>
                                </td>
                              </tr>
                            ))}
                            </tbody>
                          </Table>
                        </Col>
                      </Row>
                    )}
                    <AddParticipants socialEvent={socialEvent}/>
                  </Col>
                </Row>
              </Stack>
            </Col>
          </Row>
        </Col>
      </Row>
      <ConfirmationModal/>
    </Container>
  );
}
