export type IApiResponse<T> =
  | { success: true; data: T; message?: string }
  | { success: false; data?: never; message: string };

export interface IPaginatedApiResponse<T = any> {
  success: boolean;
  data: IPaginatedResponse<T>;
  message?: string;
}

export interface IPaginatedResponse<T = any> {
  items: T[];
  totalPages: number;
  pageSize: number;
  page: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
