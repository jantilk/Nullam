import {Controller, useForm} from "react-hook-form";
import {useNavigate, useParams} from "react-router-dom";
import {InvalidateQueryFilters, useMutation, useQuery, useQueryClient} from "@tanstack/react-query";
import {ChangeEvent, useEffect, useState} from "react";
import {toast} from "sonner";
import {Button, Col, Container, Form, Row, Stack} from "react-bootstrap";
import utils from "../../../utils/utils.ts";
import queryKeys from "../../../api/queryKeys.ts";
import socialEventPersonsApi, {UpdateSocialEventPersonRequest} from "../../../api/socialEventPersonsApi.ts";
import resourceApi, {GetResourceByTypeResponse, resourceTypes} from "../../../api/resourceApi.ts";
import constants from "../../../utils/constants.ts";

export default function UpdatePersonParticipant() {
  const {control, handleSubmit, reset} = useForm<UpdateSocialEventPersonRequest>();
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
        PaymentTypeId: personData.paymentType.id,
        AdditionalInfo: personData.additionalInfo
      });
    }
  }, [personData, isLoading, reset]);

  const mutation = useMutation({
    mutationFn: ({eventId, personId, formData}: { eventId: string, personId: string, formData: UpdateSocialEventPersonRequest }) => {
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
      toast.error(constants.ERROR_TEXT.SOMETHING_WENT_WRONG);
      return;
    }

    mutation.mutate({eventId, personId, formData});
  }

  const [charCount, setCharCount] = useState(0);
  const handleTextChange = (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setCharCount(event.target.value.length);
  };

  const {data: paymentTypes} = useQuery({
    queryKey: [queryKeys.RESOURCES_BY_TYPE],
    queryFn: () => {
      return resourceApi.getByType(resourceTypes.PAYMENT_TYPE)
    },
    select: (response) => {
      return response.success ? response.data : [];
    }
  });

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
                        rules={{
                          required: "kohustuslik",
                          maxLength: {value: 50, message: 'Kuni 50 tähemärki'}
                        }}
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
                        rules={{
                          required: "kohustuslik",
                          maxLength: {value: 50, message: 'Kuni 50 tähemärki'}
                        }}
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
                    <Form.Label column sm={24} md={8}>Isikukood:*</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="IdCode"
                        control={control}
                        defaultValue=""
                        rules={{
                          required: "kohustuslik",
                          validate: {
                            validEstonianIdCode: value => utils.isValidEstonianIdCode(value) || "Viga isikukoodis."
                          }
                        }}
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
                  <Form.Group controlId="paymentTypeId" as={Row}>
                    <Form.Label column md={8}>Makseviis:*</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="PaymentTypeId"
                        control={control}
                        rules={{required: "Required"}}
                        render={({field, fieldState}) => (
                          <>
                            <Form.Control as={"select"} {...field} className={`form-control form-select ${fieldState.error ? 'is-invalid' : ''}`}>
                              <option/>
                              {paymentTypes && paymentTypes.map((paymentType: GetResourceByTypeResponse) => {
                                return (
                                  <option key={paymentType.id} value={paymentType.id}>{paymentType.text}</option>
                                )
                              })}
                            </Form.Control>
                            {fieldState.error && (
                              <div className="invalid-feedback">{fieldState.error.message}</div>
                            )}
                          </>
                        )}
                      />
                    </Col>
                  </Form.Group>
                  <Form.Group controlId="additionalInfo" as={Row}>
                    <Form.Label column md={8}>Lisainfo:</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="AdditionalInfo"
                        control={control}
                        defaultValue=""
                        rules={{maxLength: {value: 1500, message: 'Maksimaalselt 1500 tähemärki'}}}
                        render={({field, fieldState}) => (
                          <>
                            <Form.Control
                              as="textarea"
                              rows={4}
                              maxLength={1500}
                              className={`form-control ${fieldState.error ? 'is-invalid' : ''}`}
                              {...field}
                              onChange={(e) => {
                                field.onChange(e);
                                handleTextChange(e);
                              }}
                            />
                            <div className="text-count justify-content-end d-flex py-1 px-2"><span>{charCount}/1500</span></div>
                            {fieldState.error && (<div className="invalid-feedback">{fieldState.error.message}</div>)}
                          </>
                        )}
                      />
                    </Col>
                  </Form.Group>
                </Stack>
                <Row>
                  <Col sm={6}>
                    <Button variant={"secondary"} onClick={() => navigate(-1)} className={"w-100 mb-4 mb-sm-0"}>Tagasi</Button>
                  </Col>
                  <Col sm={6}>
                    <Button variant="primary" type="submit" className={"w-100"}>Salvesta</Button>
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
