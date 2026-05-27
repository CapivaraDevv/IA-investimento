import { Outlet, NavLink, useParams } from "react-router-dom";

const NAV_ITEMS = [
  { to: "dashboard", label: "Visão geral" },
  { to: "goals", label: "Metas" },
  { to: "simulate", label: "Simulação" },
];

export default function AppShell() {
  const { userId } = useParams();

  return (
    <div className="flex min-h-screen bg-[#07101F]">
      {/* Sidebar */}
      <aside className="w-60 p-8">
        {/* Logo */}
        <div className="text-white font-bold text-xl mb-4">Finance Advisor</div>

        {/* Nav */}
        <nav className="w-full flex flex-col text-white space-y-2">
          {NAV_ITEMS.map((item) => (
            <NavLink
              key={item.to}
              to={`/${userId}/${item.to}`}
              className={`
                  relative
                  w-full
                  font-semibold
                  transition-transform
                  hover:translate-x-3
          
                  `}
            >
              {({ isActive }) => (
                <>
                  {isActive && (
                    <span className="absolute left-0 w-1 h-full bg-blue-600 rounded-full" />
                  )}
                  <span
                    className={`pl-4 ${isActive ? "text-white" : "text-gray-400"}`}
                  >
                    {item.label}
                  </span>
                </>
              )}
            </NavLink>
          ))}
        </nav>
      </aside>

      {/* Conteúdo */}
      <main className="flex-1 overflow-auto bg-slate-50">
        <Outlet />
      </main>
    </div>
  );
}
