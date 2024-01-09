import {Button, Col, Container, Form, Row, Stack} from "react-bootstrap";
import {Controller, useForm} from "react-hook-form";
import {useNavigate, useParams} from "react-router-dom";
import socialEventCompaniesApi, {AddSocialEventCompanyRequest, UpdateSocialEventCompanyRequest} from "../../api/socialEventCompaniesApi.ts";
import {InvalidateQueryFilters, useMutation, useQuery, useQueryClient} from "@tanstack/react-query";
import queryKeys from "../../api/QueryKeys.ts";
import {useEffect} from "react";
import {toast} from "sonner";

export default function UpdateCompanyParticipant() {
  const {control, handleSubmit, reset} = useForm<AddSocialEventCompanyRequest>();
  const navigate = useNavigate();
  const {eventId, companyId} = useParams();
  const queryClient = useQueryClient();

  const {data: companyData, isLoading} = useQuery({
    queryKey: [queryKeys.GET_COMPANY_BY_ID, eventId, companyId],
    queryFn: () => {
      return socialEventCompaniesApi.getByCompanyId(eventId, companyId)
    },
    select: (response) => {
      return response.data;
    }
  });

  useEffect(() => {
    if (companyData && !isLoading) {
      reset({
        Name: companyData.company.name,
        RegisterCode: companyData.company.registerCode,
        NumberOfParticipants: companyData.numberOfParticipants,
        PaymentType: companyData.paymentType,
        AdditionalInfo: companyData.additionalInfo
      });
    }
  }, [companyData, isLoading, reset]);

  const mutation = useMutation({
    mutationFn: ({eventId, companyId, formData}: { eventId: string, companyId: string, formData: UpdateSocialEventCompanyRequest }) => {
      return socialEventCompaniesApi.update(eventId, companyId, formData);
    },
    onSuccess: async () => {
      navigate(`/social-events/${eventId}/participants`);
      await queryClient.invalidateQueries([queryKeys.GET_COMPANY_BY_ID, eventId, companyId] as InvalidateQueryFilters);
      toast.success('Salvestamine õnnestus');
    },
    onError: () => {
      toast.error('Viga salvestamisel');
    }
  })

  const onSubmit = (formData: UpdateSocialEventCompanyRequest) => {
    if (!eventId || !companyId) {
      toast.error('Midagi läks valesti!');
      return;
    }

    mutation.mutate({eventId, companyId, formData});
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
                  <Form.Group controlId="name" as={Row}>
                    <Form.Label column sm={24} md={8}>Nimi:*</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="Name"
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
                  <Form.Group controlId="registerCode" as={Row}>
                    <Form.Label column sm={24} md={8}>Registrikood:*</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="RegisterCode"
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
                  <Form.Group controlId="numberOfParticipants" as={Row}>
                    <Form.Label column md={8}>Osavõtjate arv:*</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="NumberOfParticipants"
                        control={control}
                        defaultValue={0}
                        rules={{
                          required: "kohustuslik",
                          min: {
                            value: 1,
                            message: "vähemalt 1 osaleja"
                          }
                        }}
                        render={({field, fieldState}) => (
                          <>
                            <Form.Control
                              type="number" {...field}
                              className={`form-control ${fieldState.error ? 'is-invalid' : ''}`}
                            />
                            {fieldState.error && (
                              <div className="invalid-feedback">
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
