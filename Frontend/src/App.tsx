import { useState } from 'react';
import NavBar from './components/shared/NavBar';
import TasksPage from './pages/TasksPage';
import LabelsPage from './pages/LabelsPage';
import './App.css';

type Page = 'tasks' | 'labels';

export default function App() {
  const [activePage, setActivePage] = useState<Page>('tasks');

  return (
    <div className="app">
      <NavBar activePage={activePage} onNavigate={setActivePage} />
      <main className="main-content">
        {activePage === 'tasks' && <TasksPage />}
        {activePage === 'labels' && <LabelsPage />}
      </main>
    </div>
  );
}
