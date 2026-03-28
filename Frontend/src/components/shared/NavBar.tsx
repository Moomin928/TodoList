type Page = 'tasks' | 'labels' | 'categories';

interface NavBarProps {
  activePage: Page;
  onNavigate: (page: Page) => void;
}

export default function NavBar({ activePage, onNavigate }: NavBarProps) {
  return (
    <nav className="navbar">
      <div className="navbar-brand">Todo App</div>
      <div className="navbar-links">
        <button
          className={`nav-link ${activePage === 'tasks' ? 'active' : ''}`}
          onClick={() => onNavigate('tasks')}
        >
          Tasks
        </button>
        <button
          className={`nav-link ${activePage === 'categories' ? 'active' : ''}`}
          onClick={() => onNavigate('categories')}
        >
          Categories
        </button>
        <button
          className={`nav-link ${activePage === 'labels' ? 'active' : ''}`}
          onClick={() => onNavigate('labels')}
        >
          Labels
        </button>
      </div>
    </nav>
  );
}
