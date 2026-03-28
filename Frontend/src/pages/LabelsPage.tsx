import { useState ,useEffect} from 'react';
import { Label, CreateLabelRequest, UpdateLabelRequest } from '../types/label';
import LabelList from '../components/labels/LabelList';
import LabelForm from '../components/labels/LabelForm';
import Modal from '../components/shared/Modal';


export default function LabelsPage() {
  const [labels, setLabels] = useState<Label[]>([]);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [editingLabel, setEditingLabel] = useState<Label | null>(null);
    useEffect(()=>{
  fetch("http://localhost:5276/api/label")
      .then(res=>{
        if(!res.ok){
          throw new Error("Failed to fetch labels");
        }
        return res.json();
      })
      .then(data=>{
        setLabels(data);
      })
      .catch((error)=>{
        console.log(error);
      });
},[]);
  
  
  async function handleCreate(data: CreateLabelRequest) {
      
      try {
          const res=await fetch("http://localhost:5276/api/label", {
              method: "POST",
              headers: {
                  "Content-Type": "application/json",
              },
              body: JSON.stringify(data),
          });
          if(!res.ok){
              throw new Error("Failed to create labels");
          }
          const createdLabel = await res.json();
          setLabels((prev) => [...prev, createdLabel]);
          setShowCreateModal(false);
      }
      catch(error){
          console.log(error);
      }
  }

  async function handleUpdate(data: UpdateLabelRequest) {
      if (!editingLabel) return;
      try {
          const res = await fetch(`http://localhost:5276/api/label/${editingLabel.id}`, {
              method: "PUT",
              headers: {
                  "Content-Type": "application/json",
              },
              body: JSON.stringify(data),
          });
          if (!res.ok) {
              throw new Error("Failed to update labels");
          }
          const updatedLabel = await res.json();
          setLabels((prev) =>
              prev.map((l) => (l.id === updatedLabel.id ? {...l, name: updatedLabel.name} : l))
          );
          setEditingLabel(null);
      } catch (error) {
          console.log(error);
      }
  }

 async  function handleDelete(id: number) {
     try {
         const res = await fetch(`http://localhost:5276/api/label/${id}`, {
             method: "DELETE",
         });
         if (!res.ok) {
             throw new Error("Failed to delete label");
         }
         setLabels((prev) => prev.filter((l) => l.id !== id));
     } catch (error) {
         console.log(error);
     }
 }
  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1 className="page-title">Labels</h1>
          <p className="page-subtitle">{labels.length} labels total</p>
        </div>
        <button className="btn btn-primary" onClick={() => setShowCreateModal(true)}>
          + New Label
        </button>
      </div>

      <LabelList labels={labels} onEdit={setEditingLabel} onDelete={handleDelete} />

      {showCreateModal && (
        <Modal title="Create Label" onClose={() => setShowCreateModal(false)}>
          <LabelForm
            mode="create"
            onSubmit={handleCreate}
            onCancel={() => setShowCreateModal(false)}
          />
        </Modal>
      )}

      {editingLabel && (
        <Modal title="Edit Label" onClose={() => setEditingLabel(null)}>
          <LabelForm
            mode="edit"
            label={editingLabel}
            onSubmit={handleUpdate}
            onCancel={() => setEditingLabel(null)}
          />
        </Modal>
      )}
    </div>
  );
}
