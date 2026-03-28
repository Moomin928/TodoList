import { Category } from '../../types/category';

interface CategoryCardProps {
  category: Category;
  onEdit: (category: Category) => void;
  onDelete: (id: number) => void;
}

export default function CategoryCard({ category, onEdit, onDelete }: CategoryCardProps) {
  return (
    <div className="category-card">
      <div className="category-card-left">
        <span className="category-tag" style={{ backgroundColor: category.color }}>
          {category.name}
        </span>
        {category.description && (
          <span className="category-description">{category.description}</span>
        )}
      </div>
      <div className="label-actions">
        <button className="btn btn-sm btn-secondary" onClick={() => onEdit(category)}>
          Edit
        </button>
        <button className="btn btn-sm btn-danger" onClick={() => onDelete(category.id)}>
          Delete
        </button>
      </div>
    </div>
  );
}
