import {Button, Col, Container, Form, Modal, Row, Stack, Table} from "react-bootstrap";
import {useNavigate} from "react-router-dom";
import {InvalidateQueryFilters, useQuery, useQueryClient} from "@tanstack/react-query";
import queryKeys from "../../api/queryKeys.ts";
import resourceApi, {AddResourceRequest, resourceTypes, UpdateResourceRequest} from "../../api/resourceApi.ts";
import {Controller, useForm} from "react-hook-form";
import {toast} from "sonner";
import {useState} from "react";
import axios from "axios";
import constants from "../../utils/constants.ts";

export default function Settings() {
  const {control, handleSubmit, reset} = useForm<AddResourceRequest>();
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  const {data: paymentTypes} = useQuery({
    queryKey: [queryKeys.RESOURCES_BY_TYPE],
    queryFn: () => {
      return resourceApi.getByType(resourceTypes.PAYMENT_TYPE)
    },
    select: (response) => {
      return response.success ? response.data : [];
    }
  });

  const onSubmit = async (data: AddResourceRequest) => {
    try {
      await resourceApi.add({type: resourceTypes.PAYMENT_TYPE, text: data.text});
      toast.success('Makseviisi lisamine õnnestus');
      await queryClient.invalidateQueries([queryKeys.RESOURCES_BY_TYPE] as InvalidateQueryFilters);
      reset();
    } catch (er) {
      toast.error(constants.ERROR_TEXT.SOMETHING_WENT_WRONG);
    }
  };

  const [showModal, setShowModal] = useState(false);
  const [currentPaymentTypeId, setCurrentPaymentTypeId] = useState<string | null>(null);
  const {control: modalControl, handleSubmit: modalHandleSubmit, reset: resetModalForm} = useForm<UpdateResourceRequest>();

  const onEditClick = (paymentTypeId: string, currentText: string) => {
    setCurrentPaymentTypeId(paymentTypeId);
    resetModalForm({type: resourceTypes.PAYMENT_TYPE, text: currentText});
    setShowModal(true);
  };

  const handleModalSubmit = async (data: UpdateResourceRequest) => {
    if (currentPaymentTypeId) {
      try {
        await resourceApi.update(currentPaymentTypeId, {...data, type: resourceTypes.PAYMENT_TYPE});
        toast.success('Makseviisi uuendamine õnnestus');
        setShowModal(false);
        await queryClient.invalidateQueries([queryKeys.RESOURCES_BY_TYPE] as InvalidateQueryFilters);
      } catch (er) {
        toast.error(constants.ERROR_TEXT.SOMETHING_WENT_WRONG);
      }
    }
  };

  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [currentDeletingPaymentTypeId, setCurrentDeletingPaymentTypeId] = useState<string | null>(null);

  const onDeleteClick = (paymentTypeId: string) => {
    setCurrentDeletingPaymentTypeId(paymentTypeId);
    setShowDeleteModal(true);
  };

  const handleDelete = async () => {
    if (currentDeletingPaymentTypeId) {
      try {
        await resourceApi.delete(currentDeletingPaymentTypeId);
        toast.success('Makseviisi kustutamine õnnestus');
        setShowDeleteModal(false);
        await queryClient.invalidateQueries([queryKeys.RESOURCES_BY_TYPE] as InvalidateQueryFilters);
      } catch (er) {
        if (axios.isAxiosError(er) && er.response?.status === 409) {
          toast.error('Ei saa kustutada. Maksmisviis on kasutuses.');
          return;
        }
        toast.error(constants.ERROR_TEXT.SOMETHING_WENT_WRONG);
      } finally {
        setShowDeleteModal(false);
      }
    }
  };

  return (
    <Container className={"shadow-sm bg-white"}>
      <Row>
        <Col className={"bg-primary d-flex align-items-center justify-content-center"} md={6}>
          <h1 className={"text-white"}>Seaded</h1>
        </Col>
        <Col className={"background-image-col"}/>
      </Row>
      <Row className={"justify-content-center"}>
        <Col sm={24} md={16} lg={12} className={"p-4"}>
          <Row><Col><h2 className={"text-primary"}>Makseviisid</h2></Col></Row>
          <Row>
            <Col md={12}>
              <Table borderless>
                <tbody>
                {paymentTypes && paymentTypes.map((paymentType, index) => (
                  <tr key={paymentType.id}>
                    <th scope={"row"} className={"px-0 table-col-min-width"}>{index + 1}.</th>
                    <td>{paymentType.text}</td>
                    <td className={"col-2"}>
                      <Button type={"button"} className={"btn btn-link py-0"} onClick={() => onEditClick(paymentType.id, paymentType.text)}>MUUDA</Button>
                    </td>
                    <td className={"table-col-min-width"}>
                      <Button
                        type={"button"}
                        className={"btn btn-link py-0 custom-delete-button"}
                        onClick={() => onDeleteClick(paymentType.id)}
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
          <Row className={"mt-5"}><Col><h2 className={"text-primary"}>Makseviisi lisamine</h2></Col></Row>
          <Row>
            <Col>
              <Form onSubmit={handleSubmit(onSubmit)}>
                <Stack gap={2} className={"mt-3 mb-5"}>
                  <Form.Group controlId="text" as={Row}>
                    <Form.Label column sm={24} md={8}>Nimetus:*</Form.Label>
                    <Col md={16}>
                      <Controller
                        name="text"
                        control={control}
                        defaultValue=""
                        rules={{
                          required: "kohustuslik",
                          maxLength: {value: 50, message: "Kuni 50 tähemärki"},
                        }}
                        render={({field, fieldState}) => (
                          <>
                            <Form.Control className={`form-control ${fieldState.error ? 'is-invalid' : ''}`} type="text" {...field}/>
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
                    <Button variant={"secondary"} onClick={() => navigate("/")} className={"w-100 mb-4 mb-sm-0"}>Tagasi</Button>
                  </Col>
                  <Col sm={6}>
                    <Button variant="primary" type="submit" className={"w-100"}>Lisa</Button>
                  </Col>
                </Row>
              </Form>
            </Col>
          </Row>
        </Col>
      </Row>
      <Modal show={showDeleteModal} onHide={() => setShowDeleteModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Kustuta Makseviis</Modal.Title>
        </Modal.Header>
        <Modal.Body>Oled kindel, et soovid seda makseviisi kustutada?</Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowDeleteModal(false)}>Tühista</Button>
          <Button variant="danger" onClick={handleDelete}>Kustuta</Button>
        </Modal.Footer>
      </Modal>
      <Modal show={showModal} onHide={() => setShowModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Makseviisi muutmine</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={modalHandleSubmit(handleModalSubmit)}>
            <Form.Group controlId="editText">
              <Form.Label>Nimetus:*</Form.Label>
              <Controller
                name="text"
                control={modalControl}
                defaultValue=""
                rules={{required: "Kohustuslik väli"}}
                render={({field, fieldState}) => (
                  <>
                    <Form.Control type="text"{...field} isInvalid={!!fieldState.error}/>
                    {modalControl._formState.errors.text && (
                      <Form.Control.Feedback type="invalid">
                        {modalControl._formState.errors.text.message}
                      </Form.Control.Feedback>
                    )}
                  </>
                )}
              />
              <div className={"d-flex gap-3 justify-content-end mt-3"}>
                <Button variant={"secondary"} onClick={() => setShowModal(false)}>Tagasi</Button>
                <Button variant="primary" type="submit">Salvesta</Button>
              </div>
            </Form.Group>
          </Form>
        </Modal.Body>
      </Modal>
    </Container>
  );
}
