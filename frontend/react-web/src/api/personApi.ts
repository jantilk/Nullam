import baseApi, {getData, Response} from "./baseApi.ts";
import {PersonResponse} from "./socialEventPersonsApi.ts";

const baseUrl = '/api/v1/persons';

const personApi = {
  getByPersonId: (id: string) =>
    baseApi
      .get<Response<PersonResponse>>(`${baseUrl}/${id}`)
      .then(getData)
}

export default personApi;
