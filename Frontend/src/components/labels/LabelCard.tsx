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
        <span className="label-chip">{label.name}</span>
        <span className="label-id">#{label.id}</span>
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
