import {Button, Col, Form, Row, Stack} from "react-bootstrap";
import {Controller, useForm} from "react-hook-form";
import {useNavigate} from "react-router-dom";
import {InvalidateQueryFilters, useMutation, useQueryClient} from "@tanstack/react-query";
import {toast} from "sonner";
import {SocialEvent} from "../../../../../../types/SocialEvent.ts";
import queryKeys from "../../../../../../api/queryKeys.ts";
import socialEventCompaniesApi, {AddSocialEventCompanyRequest} from "../../../../../../api/socialEventCompaniesApi.ts";
import {PaymentType} from "../../../../../../api/baseApi.ts";

interface ComponentProps {
  socialEvent?: SocialEvent | null;
}

export default function AddCompanyParticipant({socialEvent}: ComponentProps) {
  const {control, handleSubmit, reset} = useForm<AddSocialEventCompanyRequest>();
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: ({socialEventId, formData}: { socialEventId: string, formData: AddSocialEventCompanyRequest }) => {
      return socialEventCompaniesApi.add(socialEventId, formData);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([queryKeys.COMPANIES_BY_SOCIAL_EVENT_ID] as InvalidateQueryFilters);
      toast.success('Ettevõtte lisamine õnnestus!');
      reset();
    },
    onError: () => {
      toast.error('Midagi läks valesti');
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
            Salvesta
          </Button>
        </Col>
      </Row>
    </Form>
  );
}
