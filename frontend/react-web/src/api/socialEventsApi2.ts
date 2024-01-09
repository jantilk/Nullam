import baseApi, {getData, GetPersonsBySocialEventIdResponse, Response} from "./baseApi.ts";
import {AddSocialEventPersonRequest, GetSocialEventPersonResponse, UpdateSocialEventPersonRequest} from "./socialEventPersonsApi.ts";

const baseUrl = '/api/v1/social-events';

const socialEventsApi2 = {
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
}

export default socialEventsApi2;
