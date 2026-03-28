import { Category } from '../../types/category';
import CategoryCard from './CategoryCard';

interface CategoryListProps {
  categories: Category[];
  onEdit: (category: Category) => void;
  onDelete: (id: number) => void;
}

export default function CategoryList({ categories, onEdit, onDelete }: CategoryListProps) {
  if (categories.length === 0) {
    return <p className="empty-state">No categories yet. Create one to get started.</p>;
  }

  return (
    <div className="label-list">
      {categories.map((cat) => (
        <CategoryCard key={cat.id} category={cat} onEdit={onEdit} onDelete={onDelete} />
      ))}
    </div>
  );
}
