import baseApi, {getData, GetPersonsBySocialEventIdResponse, Response} from "./baseApi.ts";
import {GetResourceByTypeResponse} from "./resourceApi.ts";

const baseUrl = '/api/v1/social-events';

export type AddSocialEventPersonRequest = {
  FirstName: string;
  LastName: string;
  IdCode: string;
  PaymentTypeId: string;
  AdditionalInfo?: string;
};

export type GetSocialEventPersonResponse = {
  socialEventId: string;
  companyId: string;
  createdAt: Date;
  paymentType: GetResourceByTypeResponse;
  additionalInfo?: string;
  person: PersonResponse;
}

export type PersonResponse = {
  id: string;
  createdAt: Date
  firstName: string
  lastName: string
  idCode: string
}

export type UpdateSocialEventPersonRequest = {
  FirstName: string;
  LastName: string;
  IdCode: string;
  PaymentTypeId: string;
  AdditionalInfo?: string;
};

const socialEventPersonsApi = {
  add: (socialEventId: string, formData: AddSocialEventPersonRequest) =>
    baseApi
      .post<Response<boolean>>(`${baseUrl}/${socialEventId}/participants/persons`, formData)
      .then(getData),
  getBySocialEventId: (socialEventId: string) =>
    baseApi
      .get<Response<Array<GetPersonsBySocialEventIdResponse>>>(`${baseUrl}/${socialEventId}/participants/persons`)
      .then(getData),
  getByPersonId: (socialEventId: string, personId: string) =>
    baseApi
      .get<Response<GetSocialEventPersonResponse>>(`${baseUrl}/${socialEventId}/participants/persons/${personId}`)
      .then(getData),
  update: (socialEventId: string, personId: string, formData: UpdateSocialEventPersonRequest) =>
    baseApi
      .put<Response<boolean>>(`${baseUrl}/${socialEventId}/participants/persons/${personId}`, formData)
      .then(getData),
  delete: (socialEventId: string, personId: string) =>
    baseApi
      .delete<Response<boolean>>(`${baseUrl}/${socialEventId}/participants/persons/${personId}`)
      .then(getData)
}

export default socialEventPersonsApi;
