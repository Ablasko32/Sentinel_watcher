export type IApiResponse<T> =
  | { success: true; data: T; message?: string }
  | { success: false; data?: never; message: string };

export interface IPaginatedApiResponse<T = any> {
  success: boolean;
  data: IPaginatedResponse<T>;
  message?: string;
}

export interface IPaginatedResponse<T = any> extends IPagination {
  items: T[];
}

export interface IPagination {
  totalPages: number;
  totalCount: number;
  pageSize: number;
  page: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
