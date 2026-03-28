import { useState, useEffect } from 'react';
import { Category, CreateCategoryRequest } from '../types/category';
import CategoryList from '../components/categories/CategoryList';
import CategoryForm from '../components/categories/CategoryForm';
import Modal from '../components/shared/Modal';

const API = 'http://localhost:5276';

export default function CategoriesPage() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [editingCategory, setEditingCategory] = useState<Category | null>(null);

  useEffect(() => {
    fetch(`${API}/api/category`)
      .then((res) => { if (!res.ok) throw new Error('Failed to fetch categories'); return res.json(); })
      .then(setCategories)
      .catch(console.error);
  }, []);

  async function handleCreate(data: CreateCategoryRequest) {
    try {
      const res = await fetch(`${API}/api/category`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
      });
      if (!res.ok) throw new Error('Failed to create category');
      const created = await res.json();
      setCategories((prev) => [...prev, created]);
      setShowCreateModal(false);
    } catch (err) { console.error(err); }
  }

  async function handleUpdate(data: CreateCategoryRequest) {
    if (!editingCategory) return;
    try {
      const res = await fetch(`${API}/api/category/${editingCategory.id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
      });
      if (!res.ok) throw new Error('Failed to update category');
      const updated = await res.json();
      setCategories((prev) => prev.map((c) => (c.id === updated.id ? updated : c)));
      setEditingCategory(null);
    } catch (err) { console.error(err); }
  }

  async function handleDelete(id: number) {
    try {
      const res = await fetch(`${API}/api/category/${id}`, { method: 'DELETE' });
      if (!res.ok) throw new Error('Failed to delete category');
      setCategories((prev) => prev.filter((c) => c.id !== id));
    } catch (err) { console.error(err); }
  }

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1 className="page-title">Categories</h1>
          <p className="page-subtitle">{categories.length} total</p>
        </div>
        <button className="btn btn-primary" onClick={() => setShowCreateModal(true)}>
          + New Category
        </button>
      </div>

      <CategoryList
        categories={categories}
        onEdit={setEditingCategory}
        onDelete={handleDelete}
      />

      {showCreateModal && (
        <Modal title="Create Category" onClose={() => setShowCreateModal(false)}>
          <CategoryForm
            mode="create"
            onSubmit={handleCreate}
            onCancel={() => setShowCreateModal(false)}
          />
        </Modal>
      )}

      {editingCategory && (
        <Modal title="Edit Category" onClose={() => setEditingCategory(null)}>
          <CategoryForm
            mode="edit"
            category={editingCategory}
            onSubmit={handleUpdate}
            onCancel={() => setEditingCategory(null)}
          />
        </Modal>
      )}
    </div>
  );
}
