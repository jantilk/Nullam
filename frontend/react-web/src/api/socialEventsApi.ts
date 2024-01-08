import {Response} from "./Response.ts";
import {SocialEvent} from "../types/SocialEvent.ts";
import {SocialEventFormData} from "../routes/AddSocialEvent";

// TODO: move this
export interface FilterDto {
  Keyword?: string;
  StartDate?: Date;
  EndDate?: Date;
}

export enum SortingOption {
  DateAsc = "DateAsc",
  DateDesc = "DateDesc"
}

interface GetSocialEventsParams {
  orderBy?: SortingOption;
  filter?: FilterDto;
}

// TODO: configure cors policy for development?
const socialEventsApi = {
  add: async (data: SocialEventFormData) => {
    try {
      const response = await fetch("http://localhost:8000/api/v1/social-events", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });

      if (!response.ok) {
        throw new Error("Network response was not ok");
      }

      const result = await response.json();
      return result;
    } catch (e) {
      console.error(e);
      throw e;
    }
  },
  get: async ({orderBy, filter}: GetSocialEventsParams = {}): Promise<SocialEvent[] | undefined> => {
    try {
      const queryParams = new URLSearchParams();

      if (orderBy) {
        queryParams.append("orderBy", orderBy);
      }

      if (filter) {
        if (filter.Keyword) {
          queryParams.append("filter.Keyword", filter.Keyword);
        }
        if (filter.StartDate) {
          queryParams.append("filter.StartDate", filter.StartDate.toISOString());
        }
        if (filter.EndDate) {
          queryParams.append("filter.EndDate", filter.EndDate.toISOString());
        }
      }

      const response = await fetch(`http://localhost:8000/api/v1/social-events?${queryParams.toString()}`);
      console.log(response)
      if (!response.ok) {
        console.log(response.statusText)
        throw new Error('Network response was not ok');
      }

      const result: Response<SocialEvent[]> = await response.json();
      return result.data;
    } catch (e) {
      console.log(e);
    }
  },
  getById: async (id: string): Promise<SocialEvent | undefined> => {
    try {
      const response = await fetch(`http://localhost:8000/api/v1/social-events/${id}`);

      if (!response.ok) {
        console.log(response.statusText)
        throw new Error('Network response was not ok');
      }

      const result: Response<SocialEvent> = await response.json();
      return result.data;
    } catch (e) {
      console.log(e);
    }
  },
  // update: async () => {
  //
  // }
  delete: async (id: string): Promise<boolean> => {
    try {
      const response = await fetch(`http://localhost:8000/api/v1/social-events/${id}`, {
        method: "DELETE",
      });

      if (!response.ok) {
        console.log(response.statusText);
        throw new Error('Network response was not ok');
      }

      return true;
    } catch (e) {
      console.error(e);
      return false;
    }
  },
}

export default socialEventsApi;
