import { signal } from '@angular/core';
import { IPagination } from '../models/apiResponse';

export const paginationSignal = () => {
  return signal<IPagination>({
    totalPages: 0,
    pageSize: 10,
    page: 1,
    hasPreviousPage: false,
    hasNextPage: false,
    totalCount: 0,
  });
};
