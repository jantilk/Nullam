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
  name: string;
  registerCode: string;
};

const apiUrl = 'https://localhost:7168';

export const getData = <T>(response: AxiosResponse<T>) => response?.data;

const defaultInstance = axios.create({
  baseURL: apiUrl,
});

export default defaultInstance;
