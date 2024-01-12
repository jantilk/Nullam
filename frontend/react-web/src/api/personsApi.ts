import baseApi, {getData, Response} from "./baseApi.ts";
import {SearchResult} from "../routes/Participants/components/AddParticipants/components/AddPersonParticipants";

const baseUrl = '/api/v1/persons';

export interface FilterDto {
  SearchTerm?: string;
  StartDate?: Date;
  EndDate?: Date;
}

const personsApi = {
  get: async (filter: FilterDto) => {
    const queryParams = new URLSearchParams();

    if (filter) {
      if (filter.SearchTerm) {
        queryParams.append("filter.SearchTerm", filter.SearchTerm);
      }
    }

    const response = await baseApi.get<Response<SearchResult[]>>(`${baseUrl}?${queryParams.toString()}`);
    return getData(response);
  }
}

export default personsApi;
