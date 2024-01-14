import baseApi, {getData, Response} from "./baseApi.ts";
import FilterDto from "../types/FilterDto.ts";
import GetCompaniesResponse from "../types/GetCompaniesResponse.ts";

const baseUrl = '/api/v1/companies';

const companiesApi = {
  get: async (filter: FilterDto) => {
    const queryParams = new URLSearchParams();

    if (filter) {
      if (filter.SearchTerm) {
        queryParams.append("Filter.SearchTerm", filter.SearchTerm);
      }
    }

    const response = await baseApi.get<Response<GetCompaniesResponse[]>>(`${baseUrl}?${queryParams.toString()}`);
    return getData(response);
  }
}

export default companiesApi;
