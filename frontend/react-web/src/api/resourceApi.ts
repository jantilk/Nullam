import baseApi, {getData, Response} from "./baseApi.ts";

const baseUrl = '/api/v1/resources';

export type GetResourceByTypeResponse = {
  id: string;
  type: string;
  text: string
}

export type AddResourceRequest = {
  type: string;
  text: string
}

export type UpdateResourceRequest = {
  type: string;
  text: string
}

export const resourceTypes = {
  PAYMENT_TYPE: "PaymentType"
}

const resourceApi = {
  add: (formData: AddResourceRequest) =>
    baseApi
      .post<Response<boolean>>(`${baseUrl}`, formData)
      .then(getData),
  getByType: (type: string) =>
    baseApi
      .get<Response<GetResourceByTypeResponse[]>>(`${baseUrl}?type=${type}`)
      .then(getData),
  update: (paymentTypeId: string, formData: UpdateResourceRequest) =>
    baseApi
      .put<Response<boolean>>(`${baseUrl}/${paymentTypeId}`, formData)
      .then(getData),
  delete: async (resourceId: string) =>
    baseApi
      .delete<Response<boolean>>(`${baseUrl}/${resourceId}`)
      .then(getData)
}

export default resourceApi;
