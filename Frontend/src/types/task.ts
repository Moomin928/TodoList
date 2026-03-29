export interface TaskLabel {
  labelId: number | null;
  labelName: string | null;
  labelColor: string | null;
}

export interface TaskItem {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: string;
  label: TaskLabel | null;
}

export interface CreateTaskItemRequest {
  title: string;
  description: string;
  labelId?: number | null;
}

export interface UpdateTaskItemRequest {
  title: string;
  description: string;
  isCompleted: boolean;
  labelId?: number | null;
}
