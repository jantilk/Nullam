import baseApi, {GetCompaniesBySocialEventIdResponse, getData, Response} from "./baseApi.ts";

const baseUrl = '/api/v1/social-events';

export enum PaymentType {
  Cash,
  BankTransaction
}

export type AddSocialEventCompanyRequest = {
  Name: string;
  RegisterCode: string;
  NumberOfParticipants: number;
  PaymentType: PaymentType;
  AdditionalInfo?: string;
};

const socialEventCompaniesApi = {
  add: (socialEventId: string, formData: AddSocialEventCompanyRequest) =>
    baseApi
      .post <Response<boolean>>(`${baseUrl}/${socialEventId}/participants/companies`, formData)
      .then(getData),
  getBySocialEventId: (socialEventId: string) =>
    baseApi
      .get<Response<Array<GetCompaniesBySocialEventIdResponse>>>(`${baseUrl}/${socialEventId}/participants/companies`)
      .then(getData)
}

export default socialEventCompaniesApi;

