import { useState } from 'react';
import { CreateLabelRequest, UpdateLabelRequest, Label } from '../../types/label';

const PRESET_COLORS = ['#4f46e5', '#0ea5e9', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6', '#ec4899'];

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
  const [name, setName] = useState(isEdit ? (props.label.name ?? '') : '');
  const [description, setDescription] = useState(isEdit ? (props.label.description ?? '') : '');
  const [color, setColor] = useState(isEdit ? (props.label.color ?? PRESET_COLORS[0]) : PRESET_COLORS[0]);
  const [error, setError] = useState('');

  function validate(): boolean {
    if (!name.trim()) { setError('Name is required'); return false; }
    if (name.length > 20) { setError('Name must be 20 characters or less'); return false; }
    setError('');
    return true;
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!validate()) return;
    props.onSubmit({ name: name.trim(), description: description.trim(), color });
  }

  return (
    <form className="form" onSubmit={handleSubmit}>
      <div className="form-group">
        <label className="form-label">
          Name <span className="char-count">({name.length}/20)</span>
        </label>
        <input
          className={`form-input ${error ? 'input-error' : ''}`}
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          maxLength={20}
          placeholder="Label name"
        />
        {error && <span className="error-msg">{error}</span>}
      </div>

      <div className="form-group">
        <label className="form-label">Description</label>
        <input
          className="form-input"
          type="text"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          placeholder="Short description (optional)"
        />
      </div>

      <div className="form-group">
        <label className="form-label">Color</label>
        <div className="color-picker">
          {PRESET_COLORS.map((c) => (
            <button
              key={c}
              type="button"
              className={`color-swatch ${color === c ? 'color-swatch--selected' : ''}`}
              style={{ backgroundColor: c }}
              onClick={() => setColor(c)}
            />
          ))}
          <input
            type="color"
            className="color-input-custom"
            value={color}
            onChange={(e) => setColor(e.target.value)}
            title="Custom color"
          />
        </div>
        <div className="color-preview">
          <span className="category-tag" style={{ backgroundColor: color }}>
            {name || 'Preview'}
          </span>
        </div>
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
