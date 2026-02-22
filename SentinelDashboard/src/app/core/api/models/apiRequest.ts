export interface IPaginatedRequest {
  skip?: number;
  take?: number;
  sortField?: string;
  sortOrder?: string;
  searchTerm?: string;
}
