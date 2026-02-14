export interface ApiResponse<T = any> {
  success: boolean;
  data?: T;
  message?: string;
}

export interface PaginatedApiResponse<T = any> {
  success: boolean;
  data?: PaginatedResponse<T>;
  message?: string;
}

export interface PaginatedResponse<T = any> {
  items?: T[];
  totalPages: number;
  pageSize: number;
  page: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
