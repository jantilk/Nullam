import axios from 'axios';
import {AxiosResponse} from 'axios';

export interface Response<T> {
  data: T;
  success: boolean;
  error?: string;
}

export enum PaymentType {
  Cash,
  BankTransaction
}

export type GetCompaniesBySocialEventIdResponse = {
  id: string;
  createdAt: Date;
  name: string;
  registerCode: number;
};

export type GetPersonsBySocialEventIdResponse = {
  id: string;
  createdAt: Date;
  firstName: string;
  lastName: string;
  idCode: string;
};

const apiUrl = 'http://localhost:8000';

export const getData = <T>(response: AxiosResponse<T>) => response?.data;

const defaultInstance = axios.create({
  baseURL: apiUrl,
});

// defaultInstance.interceptors.request.use((config) => {
//   if (config.data && typeof config.data === 'object') {
//     Object.keys(config.data).forEach(key => {
//       if (config.data[key] instanceof Date) {
//         config.data[key] = config.data[key].toISOString();
//       }
//     });
//   }
//   return config;
// });

export default defaultInstance;
