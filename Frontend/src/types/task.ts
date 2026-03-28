export interface TaskCategory {
  categoryId: number | null;
  categoryName: string | null;
  categoryColor: string | null;
}

export interface TaskItem {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: string;
  category: TaskCategory | null;
}

export interface CreateTaskItemRequest {
  title: string;       // max 20
  description: string; // max 200
  categoryId?: number | null;
}

export interface UpdateTaskItemRequest {
  title: string;       // max 20
  description: string; // max 200
  isCompleted: boolean;
  categoryId?: number | null;
}
