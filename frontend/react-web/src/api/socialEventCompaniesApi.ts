import baseApi, {GetCompaniesBySocialEventIdResponse, getData, Response} from "./baseApi.ts";
import {GetResourceByTypeResponse} from "./resourceApi.ts";

const baseUrl = '/api/v1/social-events';

export enum PaymentType {
  Cash,
  BankTransaction
}

export type AddSocialEventCompanyRequest = {
  Name: string;
  RegisterCode: number | undefined;
  NumberOfParticipants: number | undefined;
  PaymentTypeId: string;
  AdditionalInfo?: string;
};

export type CompanyResponse = {
  id: string;
  createdAt: Date
  name: string
  registerCode: number
}

export type GetSocialEventCompanyResponse = {
  socialEventId: string;
  companyId: string;
  numberOfParticipants: number;
  createdAt: Date;
  paymentType: GetResourceByTypeResponse;
  additionalInfo?: string;
  company: CompanyResponse;
}

export type UpdateSocialEventCompanyRequest = {
  Name: string;
  RegisterCode: number;
  PaymentTypeId: string;
  NumberOfParticipants: number;
  AdditionalInfo?: string;
};

const socialEventCompaniesApi = {
  add: (socialEventId: string, formData: AddSocialEventCompanyRequest) =>
    baseApi
      .post<Response<boolean>>(`${baseUrl}/${socialEventId}/participants/companies`, formData)
      .then(getData),
  getBySocialEventId: (socialEventId: string) =>
    baseApi
      .get<Response<Array<GetCompaniesBySocialEventIdResponse>>>(`${baseUrl}/${socialEventId}/participants/companies`)
      .then(getData),
  getByCompanyId: (socialEventId: string, companyId: string) =>
    baseApi
      .get<Response<GetSocialEventCompanyResponse>>(`${baseUrl}/${socialEventId}/participants/companies/${companyId}`)
      .then(getData),
  update: (socialEventId: string, companyId: string, formData: UpdateSocialEventCompanyRequest) =>
    baseApi
      .put<Response<boolean>>(`${baseUrl}/${socialEventId}/participants/companies/${companyId}`, formData)
      .then(getData),
  delete: (socialEventId: string, companyId: string) =>
    baseApi
      .delete<Response<boolean>>(`${baseUrl}/${socialEventId}/participants/companies/${companyId}`)
      .then(getData)
}

export default socialEventCompaniesApi;

