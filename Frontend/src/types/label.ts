export interface Label {
  id: number;
  name: string;
}

export interface CreateLabelRequest {
  name: string; // max 10
}

export interface UpdateLabelRequest {
  name: string; // max 10
}
