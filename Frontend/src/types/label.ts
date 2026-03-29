export interface Label {
  id: number;
  name: string;
  description: string;
  color: string;
}

export interface CreateLabelRequest {
  name: string;
  description: string;
  color: string;
}

export interface UpdateLabelRequest {
  name: string;
  description: string;
  color: string;
}
