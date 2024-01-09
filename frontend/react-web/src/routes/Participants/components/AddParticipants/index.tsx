import {Button, Col, Form, Row, Stack} from "react-bootstrap";
import {Controller, useForm} from "react-hook-form";
import {toast} from "sonner";
import {useNavigate} from "react-router-dom";
import "./index.scss";
import {SocialEvent} from "../../../../types/SocialEvent.ts";
import {GetCompaniesBySocialEventIdResponse} from "../../../../api/baseApi.ts";
import {useMutation} from "@tanstack/react-query";
import socialEventCompaniesApi, {AddSocialEventCompanyRequest, PaymentType} from "../../../../api/socialEventCompaniesApi.ts";

interface ComponentProps {
  socialEvent?: SocialEvent;
  companies?: GetCompaniesBySocialEventIdResponse[];
}

export default function AddParticipants({socialEvent, companies}: ComponentProps) {
  const {control, handleSubmit} = useForm<AddSocialEventCompanyRequest>();
  const navigate = useNavigate();

  const mutation = useMutation({
    mutationFn: ({socialEventId, formData}: { socialEventId: string, formData: AddSocialEventCompanyRequest }) => {
      return socialEventCompaniesApi.add(socialEventId, formData);
    },
    onSuccess: () => {
      navigate("/");
      toast.success('Ettevõtte lisamine õnnestus!');
    },
    onError: () => {
      toast.error('socialEventCompaniesApi2.add');
    }
  })

  const onSubmit = async (formData: AddSocialEventCompanyRequest) => {
    if (!socialEvent?.id) {
      toast.error('Social event ID is missing!');
      return;
    }

    mutation.mutate({socialEventId: socialEvent.id, formData});
  }

  return (
    <>
      <Row><Col><h2 className={"text-primary"}>Osavõtjate lisamine:</h2></Col></Row>
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
                          <option value={PaymentType.Cash}>Sularaha</option>
                          <option value={PaymentType.BankTransaction}>Pangaülekanne</option>
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
                  Lisa
                </Button>
              </Col>
            </Row>
          </Form>
        </Col>
      </Row>
    </>
  );
}
