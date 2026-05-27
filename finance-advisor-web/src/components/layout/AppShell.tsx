import { label } from "framer-motion/client";
import { Outlet, NavLink, useParams } from "react-router-dom";

const NAV_ITEMS = [
  { to: "dashboard", label: "Visão geral", icon: "▣" },
  { to: "goals", label: "Metas" },
  { to: "simulate", label: "Simulação"}
];

export default function AppShell() {
  const { userId } = useParams();

  return (
    <div className="flex min-h-screen bg-[#07101F]">
      {/* Sidebar */}
      <aside className="w-60 flex flex-col /* TODO: padding e bordas */ ">
        {/* Logo */}
        <div className="/* TODO */">
          Finance Advisor
        </div>

        {/* Nav */}
        <nav className="flex-1 /* TODO */">
          {NAV_ITEMS.map((item) => (
            <NavLink
              key={item.to}
              to={`/${userId}/${item.to}`}
              className={``}
            >
              {item.label}
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
