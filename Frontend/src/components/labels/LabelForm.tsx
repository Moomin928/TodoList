import { useState } from 'react';
import { CreateLabelRequest, UpdateLabelRequest, Label } from '../../types/label';

interface CreateLabelFormProps {
  mode: 'create';
  onSubmit: (data: CreateLabelRequest) => void;
  onCancel: () => void;
}

interface EditLabelFormProps {
  mode: 'edit';
  label: Label;
  onSubmit: (data: UpdateLabelRequest) => void;
  onCancel: () => void;
}

type LabelFormProps = CreateLabelFormProps | EditLabelFormProps;

export default function LabelForm(props: LabelFormProps) {
  const isEdit = props.mode === 'edit';
  const [name, setName] = useState(isEdit ? props.label.name : '');
  const [error, setError] = useState('');

  function validate(): boolean {
    if (!name.trim()) { setError('Name is required'); return false; }
    if (name.length > 10) { setError('Name must be 10 characters or less'); return false; }
    setError('');
    return true;
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!validate()) return;
    props.onSubmit({ name: name.trim() });
  }

  return (
    <form className="form" onSubmit={handleSubmit}>
      <div className="form-group">
        <label className="form-label">
          Name <span className="char-count">({name.length}/10)</span>
        </label>
        <input
          className={`form-input ${error ? 'input-error' : ''}`}
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          maxLength={10}
          placeholder="Label name"
        />
        {error && <span className="error-msg">{error}</span>}
      </div>

      <div className="form-actions">
        <button type="button" className="btn btn-secondary" onClick={props.onCancel}>
          Cancel
        </button>
        <button type="submit" className="btn btn-primary">
          {isEdit ? 'Save Changes' : 'Create Label'}
        </button>
      </div>
    </form>
  );
}
