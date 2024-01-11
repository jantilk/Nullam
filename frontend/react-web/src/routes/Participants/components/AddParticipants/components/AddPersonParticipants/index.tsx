import {Controller, useForm} from "react-hook-form";
import {useNavigate} from "react-router-dom";
import {InvalidateQueryFilters, useMutation, useQuery, useQueryClient} from "@tanstack/react-query";
import {toast} from "sonner";
import {Button, Col, Form, Row, Stack} from "react-bootstrap";
import {SocialEvent} from "../../../../../../types/SocialEvent.ts";
import socialEventPersonsApi, {AddSocialEventPersonRequest} from "../../../../../../api/socialEventPersonsApi.ts";
import queryKeys from "../../../../../../api/queryKeys.ts";
import utils from "../../../../../../utils/utils.ts";
import {ChangeEvent, useState} from "react";
import resourceApi, {GetResourceByTypeResponse, resourceTypes} from "../../../../../../api/resourceApi.ts";

interface ComponentProps {
  socialEvent?: SocialEvent | null;
}

export default function AddPersonParticipants({socialEvent}: ComponentProps) {
  const {control, handleSubmit, reset} = useForm<AddSocialEventPersonRequest>();
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: ({socialEventId, formData}: { socialEventId: string, formData: AddSocialEventPersonRequest }) => {
      return socialEventPersonsApi.add(socialEventId, formData);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([queryKeys.PERSONS_BY_SOCIAL_EVENT_ID] as InvalidateQueryFilters);
      toast.success('Isiku lisamine õnnestus!');
      reset();
    },
    onError: () => {
      toast.error('Midagi läks valesti');
    }
  })

  const onSubmit = async (formData: AddSocialEventPersonRequest) => {
    if (!socialEvent?.id) {
      toast.error('Social event ID is missing!');
      return;
    }

    mutation.mutate({socialEventId: socialEvent.id, formData});
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
    <>
      <Form onSubmit={handleSubmit(onSubmit)}>
        <Stack gap={4} className={"mt-3 mb-5"}>
          <Form.Group controlId="firstName" as={Row}>
            <Form.Label column sm={24} md={8}>Eesnimi:*</Form.Label>
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
                      <div className="invalid-feedback">
                        {fieldState.error.message}
                      </div>
                    )}
                  </>
                )}
              />
            </Col>
          </Form.Group>
          <Form.Group controlId="lastName" as={Row}>
            <Form.Label column sm={24} md={8}>Perenimi:*</Form.Label>
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
                      <div className="invalid-feedback">
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
                      <div className="invalid-feedback">
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
                rules={{required: "Kohustuslik"}}
                render={({field, fieldState}) => (
                  <>
                    <Form.Control as="select" {...field} className={`form-control form-select ${fieldState.error ? 'is-invalid' : ''}`}>
                      <option value=""/>
                      {paymentTypes && paymentTypes.map((paymentType: GetResourceByTypeResponse) => {
                        return (
                          <option key={paymentType.id} value={paymentType.id}>
                            {paymentType.text}
                          </option>
                        )
                      })}
                    </Form.Control>
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
          <Form.Group controlId="additionalInfo" as={Row}>
            <Form.Label column md={8}>Lisainfo:</Form.Label>
            <Col md={16}>
              <Controller
                name="AdditionalInfo"
                control={control}
                defaultValue=""
                rules={{
                  maxLength: {value: 1500, message: 'Maksimaalselt 1500 tähemärki'}
                }}
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
                    <div className="text-count justify-content-end d-flex py-1 px-2">
                      <span>{charCount}/1500</span>
                    </div>
                    {fieldState.error && (
                      <div className="invalid-feedback">{fieldState.error.message}</div>
                    )}
                  </>
                )}
              />
            </Col>
          </Form.Group>
        </Stack>
        <Row>
          <Col sm={6}>
            <Button variant={"secondary"} onClick={() => navigate("/")} className={"w-100 mb-4 mb-sm-0"}>
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
    </>
  );
}
