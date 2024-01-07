import {Response} from "./Response.ts";
import {SocialEvent} from "../types/SocialEvent.ts";

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
    add: async (data) => {
      try {
        const response = await fetch("https://localhost:7168/api/v1/social-events", {
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
    get: async ({ orderBy, filter }: GetSocialEventsParams = {}) : Promise<SocialEvent[] | undefined> => {
        try {
          // Construct the query string
          let queryParams = new URLSearchParams();

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
          
          const response = await fetch(`https://localhost:7168/api/v1/social-events?${queryParams.toString()}`);
          console.log(response)
          if (!response.ok) {
              console.log(response.statusText)
              throw new Error('Network response was not ok');
          }

          const result : Response<SocialEvent[]> = await response.json();
          return result.data;
        } catch (e) {
          console.log(e);
        }
    },
    getById: async (id: string) : Promise<SocialEvent | undefined> => {
        try {
            const response = await fetch(`https://localhost:7168/api/v1/social-events/${id}`);

            if (!response.ok) {
                console.log(response.statusText)
                throw new Error('Network response was not ok');
            }

            const result : Response<SocialEvent> = await response.json();
            return result.data;
        } catch (e) {
            console.log(e);
        }
    },
    // update: async () => {
    //  
    // }
}

export default socialEventsApi;
