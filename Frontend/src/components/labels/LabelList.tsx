import { Label } from '../../types/label';
import LabelCard from './LabelCard';

interface LabelListProps {
  labels: Label[];
  onEdit: (label: Label) => void;
  onDelete: (id: number) => void;
}

export default function LabelList({ labels, onEdit, onDelete }: LabelListProps) {
  if (labels.length === 0) {
    return (
      <div className="empty-state">
        <p>No labels yet. Create your first label!</p>
      </div>
    );
  }

  return (
    <div className="label-list">
      {labels.map((label) => (
        <LabelCard key={label.id} label={label} onEdit={onEdit} onDelete={onDelete} />
      ))}
    </div>
  );
}
