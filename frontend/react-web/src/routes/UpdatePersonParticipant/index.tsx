import {Controller, useForm} from "react-hook-form";
import {useNavigate, useParams} from "react-router-dom";
import {InvalidateQueryFilters, useMutation, useQuery, useQueryClient} from "@tanstack/react-query";
import queryKeys from "../../api/queryKeys.ts";
import {useEffect} from "react";
import {toast} from "sonner";
import {Button, Col, Container, Form, Row, Stack} from "react-bootstrap";
import socialEventPersonsApi, {
  AddSocialEventPersonRequest,
  UpdateSocialEventPersonRequest
} from "../../api/socialEventPersonsApi.ts";

export default function UpdatePersonParticipant() {
  const {control, handleSubmit, reset} = useForm<AddSocialEventPersonRequest>();
  const navigate = useNavigate();
  const {eventId, personId} = useParams();
  const queryClient = useQueryClient();

  const {data: personData, isLoading} = useQuery({
    queryKey: [queryKeys.GET_PERSON_BY_ID, eventId, personId],
    queryFn: () => {
      if (eventId && personId) {
        return socialEventPersonsApi.getByPersonId(eventId, personId)
      }
    },
    select: (response) => {
      if (response) {
        return response.data;
      }
      return null;
    },
    enabled: !!eventId
  });

  useEffect(() => {
    if (personData && !isLoading) {
      reset({
        FirstName: personData.person.firstName,
        LastName: personData.person.lastName,
        IdCode: personData.person.idCode,
        PaymentType: personData.paymentType,
        AdditionalInfo: personData.additionalInfo
      });
    }
  }, [personData, isLoading, reset]);

  const mutation = useMutation({
    mutationFn: ({eventId, personId, formData}: { eventId: string, personId: string, formData: AddSocialEventPersonRequest }) => {
      return socialEventPersonsApi.update(eventId, personId, formData);
    },
    onSuccess: async () => {
      navigate(`/social-events/${eventId}/participants`);
      await queryClient.invalidateQueries([queryKeys.GET_COMPANY_BY_ID, eventId, personId] as InvalidateQueryFilters);
      toast.success('Salvestamine õnnestus');
    },
    onError: () => {
      toast.error('Viga salvestamisel');
    }
  })

  const onSubmit = (formData: UpdateSocialEventPersonRequest) => {
    if (!eventId || !personId) {
      toast.error('Midagi läks valesti!');
      return;
    }

    mutation.mutate({eventId, personId, formData});
  }

  return (
    <Container className={"shadow-sm bg-white"}>
      <Row>
        <Col className={"bg-primary d-flex align-items-center justify-content-center"} md={6}>
          <h1 className={"text-white"}>Osavõtja info</h1>
        </Col>
        <Col className={"background-image-col"}/>
      </Row>
      <Row className={"justify-content-center"}>
        <Col sm={24} md={16} lg={10} className={"p-4"}>
          <Row>
            <Col>
              <h2 className={"text-primary"}>Osavõtja info</h2>
            </Col>
          </Row>
          <Row>
            <Col>
              <Form onSubmit={handleSubmit(onSubmit)}>
                <Stack gap={4} className={"mt-3 mb-5"}>
                  <Form.Group controlId="firstName" as={Row}>
                    <Form.Label column sm={24} md={8}>Nimi:*</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="FirstName"
                        control={control}
                        defaultValue=""
                        rules={{required: "kohustuslik"}}
                        render={({field, fieldState}) => (
                          <>
                            <Form.Control
                              className={`form-control ${fieldState.error ? 'is-invalid' : ''}`}
                              type="text" {...field}
                            />
                            {fieldState.error && (
                              <div className="invalid-feedback"> {/* Use div for error message */}
                                {fieldState.error.message}
                              </div>
                            )}
                          </>
                        )}
                      />
                    </Col>
                  </Form.Group>
                  <Form.Group controlId="lastName" as={Row}>
                    <Form.Label column sm={24} md={8}>Nimi:*</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="LastName"
                        control={control}
                        defaultValue=""
                        rules={{required: "kohustuslik"}}
                        render={({field, fieldState}) => (
                          <>
                            <Form.Control
                              className={`form-control ${fieldState.error ? 'is-invalid' : ''}`}
                              type="text" {...field}
                            />
                            {fieldState.error && (
                              <div className="invalid-feedback"> {/* Use div for error message */}
                                {fieldState.error.message}
                              </div>
                            )}
                          </>
                        )}
                      />
                    </Col>
                  </Form.Group>
                  <Form.Group controlId="idCode" as={Row}>
                    <Form.Label column sm={24} md={8}>Registrikood:*</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="IdCode"
                        control={control}
                        defaultValue=""
                        rules={{required: "kohustuslik"}}
                        render={({field, fieldState}) => (
                          <>
                            <Form.Control
                              className={`form-control ${fieldState.error ? 'is-invalid' : ''}`}
                              type="text" {...field}
                            />
                            {fieldState.error && (
                              <div className="invalid-feedback"> {/* Use div for error message */}
                                {fieldState.error.message}
                              </div>
                            )}
                          </>
                        )}
                      />
                    </Col>
                  </Form.Group>
                  <Form.Group controlId="paymentType" as={Row}>
                    <Form.Label column md={8}>Maksetüüp:*</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="PaymentType"
                        control={control}
                        rules={{required: "Required"}}
                        render={({field, fieldState}) => (
                          <>
                            <Form.Control as="select" {...field} className={`form-control form-select ${fieldState.error ? 'is-invalid' : ''}`}>
                              <option value=""/>
                              <option value={"Cash"}>Sularaha</option>
                              <option value={"BankTransaction"}>Pangaülekanne</option>
                            </Form.Control>
                            {fieldState.error && (
                              <div className="invalid-feedback"> {/* Use div for error message */}
                                {fieldState.error.message}
                              </div>
                            )}
                          </>
                        )}
                      />
                    </Col>
                  </Form.Group>
                  <Form.Group controlId="additionalInfo" as={Row}>
                    <Form.Label column md={8}>Lisainfo: (maksimaalselt 1000 tähemärki)</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="AdditionalInfo"
                        control={control}
                        defaultValue=""
                        render={({field}) => (
                          <Form.Control as={"textarea"} rows={4} maxLength={1000} {...field} />
                        )}
                      />
                    </Col>
                  </Form.Group>
                </Stack>
                <Row>
                  <Col sm={6}>
                    {/*TODO: navigate back*/}
                    <Button variant={"secondary"} onClick={() => navigate("/")} className={"w-100"}>
                      Tagasi
                    </Button>
                  </Col>
                  <Col sm={6}>
                    <Button variant="primary" type="submit" className={"w-100"}>
                      Salvesta
                    </Button>
                  </Col>
                </Row>
              </Form>
            </Col>
          </Row>
        </Col>
      </Row>
    </Container>
  );
}
