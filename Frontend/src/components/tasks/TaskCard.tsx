import { TaskItem } from '../../types/task';

interface TaskCardProps {
  task: TaskItem;
  onEdit: (task: TaskItem) => void;
  onDelete: (id: number) => void;
  onToggleComplete: (task: TaskItem) => void;
}

export default function TaskCard({ task, onEdit, onDelete, onToggleComplete }: TaskCardProps) {
  const createdDate = new Date(task.createdAt).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  });

  return (
    <div className={`task-card ${task.isCompleted ? 'task-card--completed' : ''}`}>
      <div className="task-card-header">
        <div className="task-card-title-row">
          <input
            type="checkbox"
            className="task-checkbox"
            checked={task.isCompleted}
            onChange={() => onToggleComplete(task)}
          />
          <h3 className={`task-title ${task.isCompleted ? 'task-title--completed' : ''}`}>
            {task.title}
          </h3>
        </div>
        <span className={`task-badge ${task.isCompleted ? 'badge-done' : 'badge-pending'}`}>
          {task.isCompleted ? 'Done' : 'Pending'}
        </span>
      </div>

      <p className="task-description">{task.description}</p>

      <div className="task-card-footer">
        <div className="task-footer-left">
          <span className="task-date">Created {createdDate}</span>
          {task.category?.categoryName ? (
            <span
              className="category-tag"
              style={{ backgroundColor: task.category.categoryColor ?? '#94a3b8' }}
            >
              {task.category.categoryName}
            </span>
          ) : (
            <span className="category-tag category-tag--none">No category</span>
          )}
        </div>
        <div className="task-actions">
          <button className="btn btn-sm btn-secondary" onClick={() => onEdit(task)}>
            Edit
          </button>
          <button className="btn btn-sm btn-danger" onClick={() => onDelete(task.id)}>
            Delete
          </button>
        </div>
      </div>
    </div>
  );
}
