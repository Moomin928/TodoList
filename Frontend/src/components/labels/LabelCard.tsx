import { Label } from '../../types/label';

interface LabelCardProps {
  label: Label;
  onEdit: (label: Label) => void;
  onDelete: (id: number) => void;
}

export default function LabelCard({ label, onEdit, onDelete }: LabelCardProps) {
  return (
    <div className="label-card">
      <div className="label-card-left">
        <span
          className="category-tag"
          style={{ backgroundColor: label.color || '#94a3b8' }}
        >
          {label.name}
        </span>
        {label.description && (
          <span className="category-description">{label.description}</span>
        )}
      </div>
      <div className="label-actions">
        <button className="btn btn-sm btn-secondary" onClick={() => onEdit(label)}>
          Edit
        </button>
        <button className="btn btn-sm btn-danger" onClick={() => onDelete(label.id)}>
          Delete
        </button>
      </div>
    </div>
  );
}
