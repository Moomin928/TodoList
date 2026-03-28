import { useState, useEffect } from 'react';
import { TaskItem, CreateTaskItemRequest, UpdateTaskItemRequest } from '../types/task';
import { Category } from '../types/category';
import TaskList from '../components/tasks/TaskList';
import TaskForm from '../components/tasks/TaskForm';
import Modal from '../components/shared/Modal';

const API = 'http://localhost:5276';

export default function TasksPage() {
  const [tasks, setTasks] = useState<TaskItem[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [editingTask, setEditingTask] = useState<TaskItem | null>(null);

  useEffect(() => {
    fetch(`${API}/api/TaskItem`)
      .then((res) => { if (!res.ok) throw new Error('Failed to fetch tasks'); return res.json(); })
      .then(setTasks)
      .catch(console.error);

    fetch(`${API}/api/category`)
      .then((res) => { if (!res.ok) throw new Error('Failed to fetch categories'); return res.json(); })
      .then(setCategories)
      .catch(console.error);
  }, []);

  const pendingCount = tasks.filter((t) => !t.isCompleted).length;
  const completedCount = tasks.filter((t) => t.isCompleted).length;

  async function handleCreate(data: CreateTaskItemRequest) {
    try {
      const res = await fetch(`${API}/api/TaskItem`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
      });
      if (!res.ok) throw new Error('Failed to create task');
      const created = await res.json();
      setTasks((prev) => [created, ...prev]);
      setShowCreateModal(false);
    } catch (err) { console.error(err); }
  }

  async function handleUpdate(data: UpdateTaskItemRequest) {
    if (!editingTask) return;
    try {
      const res = await fetch(`${API}/api/TaskItem/${editingTask.id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
      });
      if (!res.ok) throw new Error('Failed to update task');
      const updated = await res.json();
      setTasks((prev) => prev.map((t) => (t.id === updated.id ? updated : t)));
      setEditingTask(null);
    } catch (err) { console.error(err); }
  }

  async function handleDelete(id: number) {
    try {
      const res = await fetch(`${API}/api/TaskItem/${id}`, { method: 'DELETE' });
      if (!res.ok) throw new Error('Failed to delete task');
      setTasks((prev) => prev.filter((t) => t.id !== id));
    } catch (err) { console.error(err); }
  }

  async function handleToggleComplete(task: TaskItem) {
    try {
      const res = await fetch(`${API}/api/TaskItem/${task.id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          title: task.title,
          description: task.description,
          isCompleted: !task.isCompleted,
          categoryId: task.category?.categoryId ?? null,
        }),
      });
      if (!res.ok) throw new Error('Failed to toggle task');
      const updated = await res.json();
      setTasks((prev) => prev.map((t) => (t.id === updated.id ? updated : t)));
    } catch (err) { console.error(err); }
  }

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1 className="page-title">Tasks</h1>
          <p className="page-subtitle">
            {pendingCount} pending · {completedCount} completed
          </p>
        </div>
        <button className="btn btn-primary" onClick={() => setShowCreateModal(true)}>
          + New Task
        </button>
      </div>

      <TaskList
        tasks={tasks}
        onEdit={setEditingTask}
        onDelete={handleDelete}
        onToggleComplete={handleToggleComplete}
      />

      {showCreateModal && (
        <Modal title="Create Task" onClose={() => setShowCreateModal(false)}>
          <TaskForm
            mode="create"
            categories={categories}
            onSubmit={handleCreate}
            onCancel={() => setShowCreateModal(false)}
          />
        </Modal>
      )}

      {editingTask && (
        <Modal title="Edit Task" onClose={() => setEditingTask(null)}>
          <TaskForm
            mode="edit"
            task={editingTask}
            categories={categories}
            onSubmit={handleUpdate}
            onCancel={() => setEditingTask(null)}
          />
        </Modal>
      )}
    </div>
  );
}
