import { useState } from "react";
import { useParams } from "react-router-dom";
import { createGoal } from "../services/api";
import { useGoals } from "../hooks/useGoals";

const currency = new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL",
});

export default function GoalsPage() {
  const { userId } = useParams();
  const { goals, loading, error, reload } = useGoals(userId);

  const [description, setDescription] = useState("");
  const [targetAmount, setTargetAmount] = useState("");
  const [deadlineMonths, setDeadlineMonths] = useState("");
  const [type, setType] = useState(1);
  const [saving, setSaving] = useState(false);
  const [formError, setFormError] = useState("");

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!userId) return;

    setSaving(true);
    setFormError("");

    try {
      await createGoal({
        userId,
        type,
        description,
        targetAmount: Number(targetAmount),
        deadlineMonths: Number(deadlineMonths),
      });

      setDescription("");
      setTargetAmount("");
      setDeadlineMonths("");

      await reload();
    } catch {
      setFormError("Não foi possível criar a meta. Tente novamente.");
    } finally {
      setSaving(false);
    }
  }

  if (loading) {
    return <p className="p-8 text-slate-500">Carregando metas...</p>;
  }

  if (error) {
    return <p className="p-8 text-red-600">{error}</p>;
  }

  return (
    <section className="p-8">
      <header className="mb-8">
        <p className="text-sm font-semibold text-indigo-600">Planejamento</p>
        <h1 className="text-3xl font-bold text-slate-900">Suas metas</h1>
      </header>

      <form
        onSubmit={handleSubmit}
        className="mb-8 grid gap-4 rounded-xl bg-white p-6 shadow-sm md:grid-cols-2"
      >
        <input
          required
          value={description}
          onChange={(event) => setDescription(event.target.value)}
          placeholder="Ex.: Comprar um computador"
          className="rounded-lg border border-slate-200 px-3 py-2"
        />

        <select
          value={type}
          onChange={(event) => setType(Number(event.target.value))}
          className="rounded-lg border border-slate-200 px-3 py-2"
        >
          <option value={0}>Reserva de emergência</option>
          <option value={1}>Compra</option>
          <option value={2}>Viagem</option>
          <option value={3}>Aposentadoria</option>
          <option value={4}>Educação</option>
          <option value={5}>Outro</option>
        </select>

        <input
          required
          min="1"
          type="number"
          value={targetAmount}
          onChange={(event) => setTargetAmount(event.target.value)}
          placeholder="Valor da meta (R$)"
          className="rounded-lg border border-slate-200 px-3 py-2"
        />

        <input
          required
          min="1"
          type="number"
          value={deadlineMonths}
          onChange={(event) => setDeadlineMonths(event.target.value)}
          placeholder="Prazo em meses"
          className="rounded-lg border border-slate-200 px-3 py-2"
        />

        <div className="md:col-span-2">
          {formError && (
            <p className="mb-3 text-sm text-red-600">{formError}</p>
          )}

          <button
            disabled={saving}
            className="rounded-lg bg-indigo-600 px-4 py-2 font-semibold text-white disabled:opacity-60"
          >
            {saving ? "Salvando..." : "Adicionar meta"}
          </button>
        </div>
      </form>

      {goals.length === 0 ? (
        <p className="rounded-xl bg-white p-6 text-slate-500 shadow-sm">
          Você ainda não cadastrou nenhuma meta.
        </p>
      ) : (
        <div className="grid gap-4 md:grid-cols-2">
          {goals.map((goal) => (
            <article
              key={goal.id}
              className="rounded-xl bg-white p-6 shadow-sm"
            >
              <div className="mb-4 flex items-start justify-between gap-4">
                <h2 className="text-lg font-bold text-slate-900">
                  {goal.description}
                </h2>
                <span className="text-sm font-semibold text-indigo-600">
                  {goal.progressPercentage.toFixed(0)}%
                </span>
              </div>

              <div className="mb-4 h-2 overflow-hidden rounded-full bg-slate-100">
                <div
                  className="h-full rounded-full bg-indigo-600"
                  style={{
                    width: `${Math.min(goal.progressPercentage, 100)}%`,
                  }}
                />
              </div>

              <p className="text-sm text-slate-600">
                {currency.format(goal.currentAmount)} de{" "}
                {currency.format(goal.targetAmount)}
              </p>
              <p className="mt-2 text-sm text-slate-500">
                Faltam {currency.format(goal.remainingAmount)}
              </p>
              <p className="mt-4 text-sm font-medium text-slate-700">
                Aporte sugerido:{" "}
                {currency.format(goal.monthlyContributionNeeded)}/mês
              </p>
            </article>
          ))}
        </div>
      )}
    </section>
  );
}
