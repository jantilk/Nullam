import baseApi, {getData, Response} from "./baseApi.ts";
import {UpdateSocialEventPersonRequest} from "./socialEventPersonsApi.ts";
import {SocialEvent} from "../types/SocialEvent.ts";
import SocialEventFormData from "../types/SocialEventFormData.ts";

const baseUrl = '/api/v1/social-events';

export interface FilterDto {
  SearchTerm?: string;
  StartDate?: Date;
  EndDate?: Date;
}

export enum SortingOption {
  DateAsc = "DateAsc",
  DateDesc = "DateDesc"
}

const socialEventsApi = {
  add: (formData: SocialEventFormData) =>
    baseApi
      .post<Response<boolean>>(`${baseUrl}`, formData)
      .then(getData),
  get: async (orderBy: SortingOption, filter: FilterDto) => {
    const queryParams = new URLSearchParams();

    if (orderBy) {
      queryParams.append("orderBy", orderBy);
    }

    if (filter) {
      if (filter.SearchTerm) {
        queryParams.append("filter.SearchTerm", filter.SearchTerm);
      }
      if (filter.StartDate) {
        queryParams.append("filter.StartDate", filter.StartDate.toISOString());
      }
      if (filter.EndDate) {
        queryParams.append("filter.EndDate", filter.EndDate.toISOString());
      }
    }

    const response = await baseApi.get<Response<SocialEvent[]>>(`${baseUrl}?${queryParams.toString()}`);
    return getData(response);
  },
  getById: (socialEventId: string | undefined) =>
    baseApi
      .get<Response<SocialEvent>>(`${baseUrl}/${socialEventId}`)
      .then(getData),
  update: (socialEventId: string, personId: string, formData: UpdateSocialEventPersonRequest) =>
    baseApi
      .put<Response<boolean>>(`${baseUrl}/${socialEventId}/participants/persons/${personId}`, formData)
      .then(getData),
  delete: async (socialEventId: string) =>
    baseApi
      .delete<Response<boolean>>(`${baseUrl}/${socialEventId}`)
      .then(getData)
}

export default socialEventsApi;
