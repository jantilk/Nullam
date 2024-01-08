import {Button, Col, Container, Form, Row, Stack} from "react-bootstrap";
import {useNavigate} from "react-router-dom";
import {InvalidateQueryFilters, useQueryClient} from "@tanstack/react-query";
import {Controller, useForm} from "react-hook-form";
import socialEventsApi from "../../api/socialEventsApi.ts";
import QueryKeys from "../../api/QueryKeys.ts";
import DatePicker from 'react-datepicker';
import {et} from 'date-fns/locale';
import "./index.scss";
import {startOfDay} from "date-fns";
import {toast} from "sonner";
import {useLoader} from "../../contexts/LoaderContext.tsx";

export interface SocialEventFormData {
  Name: string;
  Date: Date | null;
  Location: string;
  AdditionalInfo: string;
}

export default function AddSocialEvent() {
  const navigate = useNavigate();
  const {control, handleSubmit} = useForm<SocialEventFormData>();
  const queryClient = useQueryClient();
  const {setLoading} = useLoader();

  const onSubmit = async (data: SocialEventFormData) => {
    setLoading(true);
    try {
      await socialEventsApi.add(data);

      navigate("/");
      toast.success('Ürituse lisamine õnnestus!');
      await queryClient.invalidateQueries([QueryKeys.FUTURE_SOCIAL_EVENTS] as InvalidateQueryFilters);
    } catch (er) {
      console.error(er);
      toast.error('Midagi läks valesti!');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container className={"shadow-sm bg-white"}>
      <Row>
        <Col className={"bg-primary d-flex align-items-center justify-content-center"} md={6}>
          <h1 className={"text-white"}>Ürituse lisamine</h1>
        </Col>
        <Col className={"background-image-col"}/>
      </Row>
      <Row className={"justify-content-center"}>
        <Col sm={24} md={16} lg={10} className={"p-4"}>
          <Row>
            <Col>
              <h2 className={"text-primary"}>Ürituse lisamine</h2>
            </Col>
          </Row>
          <Row>
            <Col>
              <Form onSubmit={handleSubmit(onSubmit)}>
                <Stack gap={2} className={"mt-3 mb-5"}>
                  <Form.Group controlId="eventName" as={Row}>
                    <Form.Label column sm={24} md={8}>Ürituse nimi:*</Form.Label>
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
                  <Form.Group controlId="eventDateTime" as={Row}>
                    <Form.Label column md={8}>Toimumisaeg:*</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="Date"
                        control={control}
                        defaultValue={null}
                        rules={{required: "kohustuslik"}}
                        render={({field, fieldState}) => (
                          <>
                            <DatePicker
                              locale={et}
                              placeholderText="pp.kk.aaaa hh:mm"
                              onChange={(date) => field.onChange(date)}
                              selected={field.value}
                              dateFormat="dd.MM.yyyy HH:mm"
                              showTimeSelect
                              timeFormat="HH:mm"
                              timeCaption="kell"
                              minDate={startOfDay(new Date())}
                              filterTime={(time) => {
                                const currentDate = new Date();
                                const selectedDate = new Date(time);

                                // Disable times before the current time on the current day
                                if (startOfDay(currentDate).getTime() === startOfDay(selectedDate).getTime()) {
                                  return selectedDate.getTime() >= currentDate.getTime();
                                }

                                return true;
                              }}
                              className={`form-control ${fieldState.error ? 'is-invalid' : ''}`}
                              wrapperClassName="w-100"
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
                  <Form.Group controlId="eventLocation" as={Row}>
                    <Form.Label column md={8}>Koht:*</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="Location"
                        control={control}
                        defaultValue=""
                        rules={{required: "kohustuslik"}}
                        render={({field, fieldState}) => (
                          <>
                            <Form.Control
                              type="text" {...field}
                              className={`form-control ${fieldState.error ? 'is-invalid' : ''}`}
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
                  <Form.Group controlId="eventInfo" as={Row}>
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
                      Lisa
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
