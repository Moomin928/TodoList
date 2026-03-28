export interface Category {
  id: number;
  name: string;
  description: string;
  color: string;
}

export interface CreateCategoryRequest {
  name: string;
  description: string;
  color: string;
}
