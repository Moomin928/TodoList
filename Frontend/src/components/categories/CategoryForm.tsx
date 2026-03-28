import { useState } from 'react';
import { Category, CreateCategoryRequest } from '../../types/category';

const PRESET_COLORS = ['#4f46e5', '#0ea5e9', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6', '#ec4899'];

interface CreateCategoryFormProps {
  mode: 'create';
  onSubmit: (data: CreateCategoryRequest) => void;
  onCancel: () => void;
}

interface EditCategoryFormProps {
  mode: 'edit';
  category: Category;
  onSubmit: (data: CreateCategoryRequest) => void;
  onCancel: () => void;
}

type CategoryFormProps = CreateCategoryFormProps | EditCategoryFormProps;

export default function CategoryForm(props: CategoryFormProps) {
  const isEdit = props.mode === 'edit';
  const [name, setName] = useState(isEdit ? props.category.name : '');
  const [description, setDescription] = useState(isEdit ? props.category.description : '');
  const [color, setColor] = useState(isEdit ? props.category.color : PRESET_COLORS[0]);
  const [errors, setErrors] = useState<{ name?: string }>({});

  function validate(): boolean {
    const newErrors: { name?: string } = {};
    if (!name.trim()) newErrors.name = 'Name is required';
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!validate()) return;
    props.onSubmit({ name: name.trim(), description: description.trim(), color });
  }

  return (
    <form className="form" onSubmit={handleSubmit}>
      <div className="form-group">
        <label className="form-label">Name</label>
        <input
          className={`form-input ${errors.name ? 'input-error' : ''}`}
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          placeholder="e.g. Work, Study"
        />
        {errors.name && <span className="error-msg">{errors.name}</span>}
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
          {isEdit ? 'Save Changes' : 'Create Category'}
        </button>
      </div>
    </form>
  );
}
