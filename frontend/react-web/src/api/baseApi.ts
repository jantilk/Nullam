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

const apiUrl = import.meta.env.MODE == "development" ? 'http://localhost:5145' : 'http://localhost:8000';

export const getData = <T>(response: AxiosResponse<T>) => response?.data;

const defaultInstance = axios.create({
  baseURL: apiUrl,
});

export default defaultInstance;
