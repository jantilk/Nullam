import {Button, Col, Form, ListGroup, Row, Stack} from "react-bootstrap";
import {Controller, useForm} from "react-hook-form";
import {useNavigate} from "react-router-dom";
import {InvalidateQueryFilters, useMutation, useQuery, useQueryClient} from "@tanstack/react-query";
import {toast} from "sonner";
import {SocialEvent} from "../../../../../../types/SocialEvent.ts";
import queryKeys from "../../../../../../api/queryKeys.ts";
import socialEventCompaniesApi, {AddSocialEventCompanyRequest} from "../../../../../../api/socialEventCompaniesApi.ts";
import {ChangeEvent, useCallback, useEffect, useState} from "react";
import resourceApi, {GetResourceByTypeResponse, resourceTypes} from "../../../../../../api/resourceApi.ts";
import constants from "../../../../../../utils/constants.ts";
import GetCompaniesResponse from "../../../../../../types/GetCompaniesResponse.ts";
import {debounce} from "lodash";
import companiesApi from "../../../../../../api/companiesApi.ts";

interface ComponentProps {
  socialEvent?: SocialEvent | null;
}

export default function AddCompanyParticipant({socialEvent}: ComponentProps) {
  const {control, handleSubmit, reset, setValue} = useForm<AddSocialEventCompanyRequest>();
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
      toast.error(constants.ERROR_TEXT.SOMETHING_WENT_WRONG);
    }
  })

  const onSubmit = async (formData: AddSocialEventCompanyRequest) => {
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

  const [searchTerm, setSearchTerm] = useState('');
  const [searchResults, setSearchResults] = useState<GetCompaniesResponse[]>([]);


  const handleSelectCompany = (company: GetCompaniesResponse) => {
    reset({
      Name: company.name,
      RegisterCode: company.registerCode
    });
    setValue('PaymentTypeId', "");
    setSearchResults([]);
  }

  const handleSearch = async (searchTerm: string) => {
    try {
      const response = await companiesApi.get({SearchTerm: searchTerm});
      if (response.success && response.data) {
        setSearchResults(response.data);
      } else {
        setSearchResults([]);
        toast.error('No results found');
      }
    } catch (error) {
      toast.error('Error fetching search results');
    }
  };

  const debounceSearch = useCallback(debounce(handleSearch, 300), []);

  useEffect(() => {
    if (searchTerm) {
      debounceSearch(searchTerm);
    } else {
      setSearchResults([]);
    }
  }, [searchTerm, debounceSearch]);

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
              rules={{
                required: "kohustuslik",
                maxLength: {
                  value: 50,
                  message: "Kuni 50 tähemärki"
                },
              }}
              render={({field, fieldState}) => (
                <>
                  <Form.Control
                    className={`form-control ${fieldState.error ? 'is-invalid' : ''}`}
                    type="text" {...field}
                    autoComplete="off"
                    {...field}
                    onBlur={() => {
                      setTimeout(() => {
                        setSearchResults([]);
                      }, 200)
                    }}
                    onChange={(e) => {
                      field.onChange(e);
                      setSearchTerm(e.target.value);
                    }}
                    onKeyDown={(e) => {
                      if (e.key === 'Escape') setSearchResults([]);
                      return;
                    }}
                  />
                  {fieldState.error && <div className="invalid-feedback">{fieldState.error.message}</div>}
                  <ListGroup className="search-results-list">
                    {searchResults.slice(0, 5).map((result) => (
                      <ListGroup.Item
                        key={result.id}
                        action
                        onClick={() => handleSelectCompany(result)}
                        className="search-result-item"
                      >
                        <div className={"d-flex justify-content-between"}>
                          <span>{result.name}</span>
                          <span>{result.registerCode}</span>
                        </div>
                      </ListGroup.Item>
                    ))}
                  </ListGroup>
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
              defaultValue={undefined}
              rules={{
                required: "kohustuslik",
                minLength: {
                  value: 8,
                  message: "Registrikood peab olema 8 numbrit pikk"
                },
                maxLength: {
                  value: 8,
                  message: "Registrikood peab olema 8 numbrit pikk"
                },
                pattern: {
                  value: /^\d{8}$/,
                  message: "Registrikood peab sisaldama ainult numbreid"
                }
              }}
              render={({field, fieldState}) => (
                <>
                  <Form.Control className={`form-control ${fieldState.error ? 'is-invalid' : ''}`} type="number" {...field}/>
                  {fieldState.error && <div className="invalid-feedback">{fieldState.error.message}</div>}
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
              defaultValue={undefined}
              rules={{
                required: "kohustuslik",
                min: {
                  value: 1,
                  message: "vähemalt 1 osaleja"
                },
                max: {
                  value: Number.MAX_SAFE_INTEGER,
                  message: "Nii palju ei saa, ei mahu ära 😄"
                }
              }}
              render={({field, fieldState}) => (
                <>
                  <Form.Control type="number" {...field} className={`form-control ${fieldState.error ? 'is-invalid' : ''}`}/>
                  {fieldState.error && <div className="invalid-feedback">{fieldState.error.message}</div>}
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
                    <option/>
                    {paymentTypes && paymentTypes.map((paymentType: GetResourceByTypeResponse) => (
                      <option key={paymentType.id} value={paymentType.id}>{paymentType.text}</option>
                    ))}
                  </Form.Control>
                  {fieldState.error && <div className="invalid-feedback">{fieldState.error.message}</div>}
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
              rules={{maxLength: {value: 4000, message: 'Maksimaalselt 4000 tähemärki'}}}
              render={({field, fieldState}) => (
                <>
                  <Form.Control
                    as={"textarea"}
                    rows={4}
                    maxLength={4000}
                    {...field}
                    className={`form-control ${fieldState.error ? 'is-invalid' : ''}`}
                    onChange={(e) => {
                      field.onChange(e);
                      handleTextChange(e);
                    }}
                  />
                  <div className="text-count justify-content-end d-flex py-1 px-2"><span>{charCount}/4000</span></div>
                  {fieldState.error && <div className="invalid-feedback">{fieldState.error.message}</div>}
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
          <Button variant="primary" type="submit" className={"w-100"}>Salvesta</Button>
        </Col>
      </Row>
    </Form>
  );
}
