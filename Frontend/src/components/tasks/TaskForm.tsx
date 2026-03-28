import { useState } from 'react';
import { CreateTaskItemRequest, UpdateTaskItemRequest, TaskItem } from '../../types/task';
import { Category } from '../../types/category';

interface CreateTaskFormProps {
  mode: 'create';
  categories: Category[];
  onSubmit: (data: CreateTaskItemRequest) => void;
  onCancel: () => void;
}

interface EditTaskFormProps {
  mode: 'edit';
  task: TaskItem;
  categories: Category[];
  onSubmit: (data: UpdateTaskItemRequest) => void;
  onCancel: () => void;
}

type TaskFormProps = CreateTaskFormProps | EditTaskFormProps;

export default function TaskForm(props: TaskFormProps) {
  const isEdit = props.mode === 'edit';

  const [title, setTitle] = useState(isEdit ? props.task.title : '');
  const [description, setDescription] = useState(isEdit ? props.task.description : '');
  const [isCompleted, setIsCompleted] = useState(isEdit ? props.task.isCompleted : false);
  const [categoryId, setCategoryId] = useState<number | null>(
    isEdit ? (props.task.category?.categoryId ?? null) : null
  );
  const [errors, setErrors] = useState<{ title?: string; description?: string }>({});

  function validate(): boolean {
    const newErrors: { title?: string; description?: string } = {};
    if (!title.trim()) newErrors.title = 'Title is required';
    else if (title.length > 20) newErrors.title = 'Title must be 20 characters or less';
    if (!description.trim()) newErrors.description = 'Description is required';
    else if (description.length > 200) newErrors.description = 'Description must be 200 characters or less';
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!validate()) return;

    if (props.mode === 'create') {
      props.onSubmit({ title: title.trim(), description: description.trim(), categoryId });
    } else {
      props.onSubmit({ title: title.trim(), description: description.trim(), isCompleted, categoryId });
    }
  }

  return (
    <form className="form" onSubmit={handleSubmit}>
      <div className="form-group">
        <label className="form-label">
          Title <span className="char-count">({title.length}/20)</span>
        </label>
        <input
          className={`form-input ${errors.title ? 'input-error' : ''}`}
          type="text"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          maxLength={20}
          placeholder="Task title"
        />
        {errors.title && <span className="error-msg">{errors.title}</span>}
      </div>

      <div className="form-group">
        <label className="form-label">
          Description <span className="char-count">({description.length}/200)</span>
        </label>
        <textarea
          className={`form-textarea ${errors.description ? 'input-error' : ''}`}
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          maxLength={200}
          placeholder="Task description"
          rows={4}
        />
        {errors.description && <span className="error-msg">{errors.description}</span>}
      </div>

      <div className="form-group">
        <label className="form-label">Category</label>
        <select
          className="form-input"
          value={categoryId ?? ''}
          onChange={(e) => setCategoryId(e.target.value ? Number(e.target.value) : null)}
        >
          <option value="">No category</option>
          {props.categories.map((cat) => (
            <option key={cat.id} value={cat.id}>
              {cat.name}
            </option>
          ))}
        </select>
      </div>

      {isEdit && (
        <div className="form-group form-checkbox-group">
          <label className="form-checkbox-label">
            <input
              type="checkbox"
              checked={isCompleted}
              onChange={(e) => setIsCompleted(e.target.checked)}
            />
            Mark as completed
          </label>
        </div>
      )}

      <div className="form-actions">
        <button type="button" className="btn btn-secondary" onClick={props.onCancel}>
          Cancel
        </button>
        <button type="submit" className="btn btn-primary">
          {isEdit ? 'Save Changes' : 'Create Task'}
        </button>
      </div>
    </form>
  );
}
