import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { motion, AnimatePresence } from "framer-motion";
import axios from "axios";

const API = import.meta.env.VITE_API_URL ?? "http://localhost:5077";

const STEPS = [
  { label: "Dados pessoais", subtitle: "Vamos começar com o básico sobre você" },
  { label: "Perfil financeiro", subtitle: "Como você enxerga risco e retorno?" },
  { label: "Despesas mensais", subtitle: "Para onde vai o seu dinheiro todo mês?" },
];

// Maps frontend labels to backend enum values
const RISK_MAP = { conservador: 0, moderado: 1, arrojado: 2 } as const;
const KNOWLEDGE_MAP = { beginner: 0, intermediate: 1, advanced: 2 } as const;

type RiskKey = keyof typeof RISK_MAP;
type KnowledgeKey = keyof typeof KNOWLEDGE_MAP;

type FormData = {
  name: string;
  salary: string;
  riskTolerance: RiskKey | "";
  knowledgeLevel: KnowledgeKey | "";
  horizonMonths: number | null;
  rent: string;
  food: string;
  transport: string;
  others: string;
};

const RISK_PROFILES = [
  {
    id: "conservador" as RiskKey,
    label: "Conservador",
    description: "Priorizo a segurança do capital, mesmo com retornos mais modestos.",
    tag: "Baixo risco",
  },
  {
    id: "moderado" as RiskKey,
    label: "Moderado",
    description: "Busco equilíbrio entre proteção do capital e crescimento do patrimônio.",
    tag: "Risco médio",
  },
  {
    id: "arrojado" as RiskKey,
    label: "Arrojado",
    description: "Aceito oscilações significativas em troca de maiores retornos no longo prazo.",
    tag: "Alto risco",
  },
];

const KNOWLEDGE_LEVELS = [
  { id: "beginner" as KnowledgeKey, label: "Iniciante" },
  { id: "intermediate" as KnowledgeKey, label: "Intermediário" },
  { id: "advanced" as KnowledgeKey, label: "Avançado" },
];

const HORIZON_OPTIONS = [
  { months: 12, label: "1 ano" },
  { months: 36, label: "3 anos" },
  { months: 60, label: "5 anos" },
  { months: 120, label: "10 anos" },
];

function parseBRL(v: string): number {
  if (!v) return 0;
  return parseFloat(v.replace(/\./g, "").replace(",", ".")) || 0;
}

const slide = {
  enter: (d: number) => ({ x: d > 0 ? 28 : -28, opacity: 0 }),
  center: { x: 0, opacity: 1 },
  exit: (d: number) => ({ x: d > 0 ? -28 : 28, opacity: 0 }),
};

function Field({
  label,
  placeholder,
  type = "text",
  prefix,
  value,
  onChange,
}: {
  label: string;
  placeholder: string;
  type?: string;
  prefix?: string;
  value: string;
  onChange: (v: string) => void;
}) {
  return (
    <div>
      <label className="block text-[10px] font-bold text-slate-400 uppercase tracking-[0.14em] mb-2.5">
        {label}
      </label>
      <div className="flex items-center border-b-2 border-slate-200 focus-within:border-indigo-600 transition-colors duration-200">
        {prefix && <span className="text-slate-400 text-sm pr-2 pb-2.5">{prefix}</span>}
        <input
          type={type}
          value={value}
          onChange={(e) => onChange(e.target.value)}
          placeholder={placeholder}
          className="w-full bg-transparent pb-2.5 text-slate-900 text-[15px] focus:outline-none placeholder:text-slate-300"
        />
      </div>
    </div>
  );
}

function StepPersonal({
  data,
  update,
}: {
  data: FormData;
  update: (p: Partial<FormData>) => void;
}) {
  return (
    <div className="space-y-9">
      <Field
        label="Nome completo"
        placeholder="João da Silva"
        value={data.name}
        onChange={(v) => update({ name: v })}
      />
      <Field
        label="Renda mensal bruta"
        placeholder="5.000,00"
        prefix="R$"
        value={data.salary}
        onChange={(v) => update({ salary: v })}
      />
    </div>
  );
}

function StepProfile({
  data,
  update,
}: {
  data: FormData;
  update: (p: Partial<FormData>) => void;
}) {
  return (
    <div className="space-y-5">
      {/* Risk tolerance */}
      <div className="space-y-2.5">
        {RISK_PROFILES.map((p) => {
          const selected = data.riskTolerance === p.id;
          return (
            <button
              key={p.id}
              type="button"
              onClick={() => update({ riskTolerance: p.id })}
              className={`w-full text-left p-4 rounded-2xl border-2 transition-all duration-200 ${
                selected
                  ? "border-indigo-600 bg-indigo-50"
                  : "border-slate-100 bg-slate-50 hover:border-slate-200 hover:bg-slate-100/50"
              }`}
            >
              <div className="flex items-start justify-between gap-4">
                <div>
                  <p className={`text-sm font-bold mb-0.5 ${selected ? "text-indigo-700" : "text-slate-800"}`}>
                    {p.label}
                  </p>
                  <p className="text-xs text-slate-500 leading-relaxed">{p.description}</p>
                </div>
                <span
                  className={`shrink-0 text-[10px] font-bold px-2.5 py-1 rounded-full tracking-wide transition-colors duration-200 ${
                    selected ? "bg-indigo-600 text-white" : "bg-slate-200 text-slate-500"
                  }`}
                >
                  {p.tag}
                </span>
              </div>
            </button>
          );
        })}
      </div>

      {/* Knowledge level */}
      <div>
        <p className="text-[10px] font-bold text-slate-400 uppercase tracking-[0.14em] mb-2.5">
          Nível de conhecimento em investimentos
        </p>
        <div className="flex gap-2">
          {KNOWLEDGE_LEVELS.map((k) => {
            const selected = data.knowledgeLevel === k.id;
            return (
              <button
                key={k.id}
                type="button"
                onClick={() => update({ knowledgeLevel: k.id })}
                className={`flex-1 py-2.5 rounded-xl text-xs font-semibold border-2 transition-all duration-200 ${
                  selected
                    ? "border-indigo-600 bg-indigo-50 text-indigo-700"
                    : "border-slate-100 bg-slate-50 text-slate-500 hover:border-slate-200"
                }`}
              >
                {k.label}
              </button>
            );
          })}
        </div>
      </div>

      {/* Investment horizon */}
      <div>
        <p className="text-[10px] font-bold text-slate-400 uppercase tracking-[0.14em] mb-2.5">
          Horizonte de investimento
        </p>
        <div className="flex gap-2">
          {HORIZON_OPTIONS.map((h) => {
            const selected = data.horizonMonths === h.months;
            return (
              <button
                key={h.months}
                type="button"
                onClick={() => update({ horizonMonths: h.months })}
                className={`flex-1 py-2.5 rounded-xl text-xs font-semibold border-2 transition-all duration-200 ${
                  selected
                    ? "border-indigo-600 bg-indigo-50 text-indigo-700"
                    : "border-slate-100 bg-slate-50 text-slate-500 hover:border-slate-200"
                }`}
              >
                {h.label}
              </button>
            );
          })}
        </div>
      </div>
    </div>
  );
}

function StepExpenses({
  data,
  update,
}: {
  data: FormData;
  update: (p: Partial<FormData>) => void;
}) {
  return (
    <div className="space-y-9">
      <Field
        label="Moradia (aluguel ou financiamento)"
        placeholder="1.200,00"
        prefix="R$"
        value={data.rent}
        onChange={(v) => update({ rent: v })}
      />
      <Field
        label="Alimentação"
        placeholder="800,00"
        prefix="R$"
        value={data.food}
        onChange={(v) => update({ food: v })}
      />
      <Field
        label="Transporte"
        placeholder="400,00"
        prefix="R$"
        value={data.transport}
        onChange={(v) => update({ transport: v })}
      />
      <Field
        label="Outros gastos fixos"
        placeholder="600,00"
        prefix="R$"
        value={data.others}
        onChange={(v) => update({ others: v })}
      />
    </div>
  );
}

export default function RegisterPage() {
  const navigate = useNavigate();
  const [step, setStep] = useState(0);
  const [dir, setDir] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [data, setData] = useState<FormData>({
    name: "",
    salary: "",
    riskTolerance: "",
    knowledgeLevel: "",
    horizonMonths: null,
    rent: "",
    food: "",
    transport: "",
    others: "",
  });

  const update = (patch: Partial<FormData>) =>
    setData((prev) => ({ ...prev, ...patch }));

  const next = () => {
    if (step < STEPS.length - 1) {
      setDir(1);
      setStep((s) => s + 1);
    }
  };

  const prev = () => {
    if (step > 0) {
      setDir(-1);
      setStep((s) => s - 1);
    }
  };

  const handleSubmit = async () => {
    if (loading) return;
    setLoading(true);
    setError("");

    try {
      // 1. Create user profile — POST /api/users
      const { data: user } = await axios.post(`${API}/api/users`, {
        name: data.name,
        salary: parseBRL(data.salary),
      });
      const userId: string = user.id;

      // 2. Upsert investment profile — PUT /api/investment-profile
      await axios.put(`${API}/api/investment-profile`, {
        userId,
        riskTolerance: RISK_MAP[data.riskTolerance as RiskKey],
        knowledgeLevel: KNOWLEDGE_MAP[data.knowledgeLevel as KnowledgeKey],
        investmentHorizonMonths: data.horizonMonths ?? 12,
      });

      // 3. Create expenses — POST /api/expenses (one per category, skip zeros)
      const expenses = [
        { category: 0, description: "Moradia", amount: parseBRL(data.rent) },
        { category: 5, description: "Alimentação", amount: parseBRL(data.food) },
        { category: 1, description: "Transporte", amount: parseBRL(data.transport) },
        { category: 8, description: "Outros", amount: parseBRL(data.others) },
      ].filter((e) => e.amount > 0);

      await Promise.all(
        expenses.map((e) =>
          axios.post(`${API}/api/expenses`, { userId, ...e })
        )
      );

      navigate(`/dashboard/${userId}`);
    } catch {
      setError("Ocorreu um erro ao salvar seus dados. Verifique e tente novamente.");
      setLoading(false);
    }
  };

  const isLast = step === STEPS.length - 1;

  return (
    <div className="min-h-screen flex">
      {/* ── Left panel ── */}
      <aside className="hidden lg:flex w-[42%] bg-[#07101F] flex-col justify-between p-14 relative overflow-hidden select-none">
        <div className="pointer-events-none absolute inset-0">
          <div className="absolute -top-32 -right-32 w-[28rem] h-[28rem] rounded-full bg-indigo-600/10 blur-3xl" />
          <div className="absolute -bottom-24 -left-24 w-80 h-80 rounded-full bg-emerald-500/10 blur-3xl" />
        </div>
        <div
          className="pointer-events-none absolute inset-0 opacity-[0.035]"
          style={{
            backgroundImage: "radial-gradient(circle, white 1px, transparent 1px)",
            backgroundSize: "28px 28px",
          }}
        />

        <div className="relative z-10">
          <div className="inline-flex items-center gap-2.5">
            <div className="w-7 h-7 rounded-lg bg-indigo-500 flex items-center justify-center">
              <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="white" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                <polyline points="22 7 13.5 15.5 8.5 10.5 2 17" />
                <polyline points="16 7 22 7 22 13" />
              </svg>
            </div>
            <span className="text-white text-sm font-semibold tracking-wide">Finance Advisor</span>
          </div>
        </div>

        <div className="relative z-10">
          <h1 className="text-[2.5rem] font-bold text-white leading-[1.18] tracking-tight mb-5">
            Tome controle da sua vida financeira.
          </h1>
          <p className="text-slate-400 text-[13px] leading-relaxed max-w-[280px]">
            Em menos de 3 minutos, criamos um plano de investimento personalizado baseado na sua realidade.
          </p>
        </div>

        <div className="relative z-10">
          {STEPS.map((s, i) => {
            const done = i < step;
            const active = i === step;
            return (
              <div key={s.label} className="flex items-start gap-4">
                <div className="flex flex-col items-center">
                  <div
                    className={`w-8 h-8 rounded-full flex items-center justify-center text-xs font-bold transition-all duration-500 ${
                      done
                        ? "bg-emerald-500 text-white"
                        : active
                        ? "bg-indigo-500 text-white ring-4 ring-indigo-500/20"
                        : "bg-white/[0.07] text-white/30"
                    }`}
                  >
                    {done ? (
                      <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                        <polyline points="20 6 9 17 4 12" />
                      </svg>
                    ) : (
                      i + 1
                    )}
                  </div>
                  {i < STEPS.length - 1 && (
                    <div className={`w-px h-10 my-1 transition-all duration-500 ${done ? "bg-emerald-500/40" : "bg-white/[0.07]"}`} />
                  )}
                </div>
                <div className="pt-1">
                  <p className={`text-sm font-medium leading-none transition-colors duration-300 ${active ? "text-white" : done ? "text-white/50" : "text-white/22"}`}>
                    {s.label}
                  </p>
                  {active && (
                    <motion.p
                      initial={{ opacity: 0, y: 4 }}
                      animate={{ opacity: 1, y: 0 }}
                      transition={{ duration: 0.3 }}
                      className="text-[11px] text-indigo-400 mt-1.5"
                    >
                      {s.subtitle}
                    </motion.p>
                  )}
                </div>
              </div>
            );
          })}
        </div>
      </aside>

      {/* ── Right panel ── */}
      <main className="flex-1 flex items-center justify-center bg-white px-8 py-12 lg:px-20">
        <div className="w-full max-w-[420px]">
          <div className="flex gap-1.5 mb-10 lg:hidden">
            {STEPS.map((_, i) => (
              <div
                key={i}
                className={`h-1 flex-1 rounded-full transition-all duration-500 ${i <= step ? "bg-indigo-600" : "bg-slate-100"}`}
              />
            ))}
          </div>

          <p className="text-[10px] font-bold text-indigo-600 uppercase tracking-[0.16em] mb-3">
            Etapa {step + 1} de {STEPS.length}
          </p>

          <AnimatePresence mode="wait" custom={dir}>
            <motion.div
              key={`title-${step}`}
              custom={dir}
              variants={slide}
              initial="enter"
              animate="center"
              exit="exit"
              transition={{ duration: 0.2, ease: "easeInOut" }}
            >
              <h2 className="text-[2rem] font-bold text-slate-900 tracking-tight leading-tight mb-1.5">
                {STEPS[step].label}
              </h2>
              <p className="text-sm text-slate-500 mb-8">{STEPS[step].subtitle}</p>
            </motion.div>
          </AnimatePresence>

          <AnimatePresence mode="wait" custom={dir}>
            <motion.div
              key={`form-${step}`}
              custom={dir}
              variants={slide}
              initial="enter"
              animate="center"
              exit="exit"
              transition={{ duration: 0.2, ease: "easeInOut" }}
            >
              {step === 0 && <StepPersonal data={data} update={update} />}
              {step === 1 && <StepProfile data={data} update={update} />}
              {step === 2 && <StepExpenses data={data} update={update} />}
            </motion.div>
          </AnimatePresence>

          {error && (
            <p className="mt-6 text-xs text-red-500 text-center">{error}</p>
          )}

          <div className="flex items-center gap-3 mt-8">
            {step > 0 && (
              <motion.button
                initial={{ opacity: 0, x: -8 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ duration: 0.18 }}
                type="button"
                onClick={prev}
                disabled={loading}
                className="px-5 py-3.5 text-sm font-semibold text-slate-500 hover:text-slate-900 transition-colors rounded-xl border border-slate-200 hover:border-slate-300 disabled:opacity-40"
              >
                Voltar
              </motion.button>
            )}
            <button
              type="button"
              onClick={isLast ? handleSubmit : next}
              disabled={loading}
              className="flex-1 py-3.5 bg-indigo-600 hover:bg-indigo-700 active:bg-indigo-800 text-white font-semibold rounded-xl transition-all duration-200 text-sm tracking-wide shadow-sm hover:shadow-lg hover:shadow-indigo-200 disabled:opacity-60 disabled:cursor-not-allowed"
            >
              {loading ? "Salvando..." : isLast ? "Gerar meu plano" : "Continuar"}
            </button>
          </div>

          <p className="text-[11px] text-slate-400 text-center mt-6 leading-relaxed">
            Seus dados são usados apenas para gerar recomendações personalizadas.
          </p>
        </div>
      </main>
    </div>
  );
}
