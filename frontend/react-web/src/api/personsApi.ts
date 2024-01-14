import baseApi, {getData, Response} from "./baseApi.ts";
import {FilterDto} from "./socialEventsApi.ts";
import GetPersonsResponse from "../types/GetPersonsResponse.ts";

const baseUrl = '/api/v1/persons';

const personsApi = {
  get: async (filter: FilterDto) => {
    const queryParams = new URLSearchParams();

    if (filter) {
      if (filter.SearchTerm) {
        queryParams.append("Filter.SearchTerm", filter.SearchTerm);
      }
    }

    const response = await baseApi.get<Response<GetPersonsResponse[]>>(`${baseUrl}?${queryParams.toString()}`);
    return getData(response);
  }
}

export default personsApi;
