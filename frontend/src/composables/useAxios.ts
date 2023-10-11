import { AxiosError } from 'axios';
import { Notify } from 'quasar';
import { _axios } from '@/services/axios';
import { CustomError } from '@/types/models';

type httpMethod = 'GET' | 'POST' | 'PUT' | 'DELETE';

export async function useAxios<TData, TResponse>(
  url: string,
  method: httpMethod,
  data?: TData,
  signal?: AbortSignal
) {
  try {
    const result = await _axios<TResponse>(url, {
      method,
      data,
      signal,
    });
    return result;
  } catch (err) {
    const error = err as AxiosError;
    const errorData = error.response?.data as CustomError;
    Notify.create({
      message: `Błąd ${errorData.status ?? error.response?.status}`,
      caption: errorData.message,
      icon: 'priority_high',
      color: 'deep-orange-8',
      textColor: 'white',
      timeout: 2000,
      position: 'top-right',
    });
    return null;
  }
}
