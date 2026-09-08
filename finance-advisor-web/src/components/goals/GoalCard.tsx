import { useState } from "react";
import { updateGoalProgress } from "../../services/api";
import type { GoalResponse } from "../../types/api";

const currency = new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL",
});

type GoalCardProps = {
  goal: GoalResponse;
  onProgressSaved: () => Promise<void>;
};

export function GoalCard({ goal, onProgressSaved }: GoalCardProps) {
  const [amount, setAmount] = useState("");
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  async function handleProgressSubmit(
    event: React.FormEvent<HTMLFormElement>
  ) {
    event.preventDefault();

    const parsedAmount = Number(amount);

    if (parsedAmount <= 0) {
      setError("Informe um aporte maior que zero.");
      return;
    }

    setSaving(true);
    setError("");

    try {
      await updateGoalProgress(goal.id, parsedAmount);
      setAmount("");
      await onProgressSaved();
    } catch {
      setError("Não foi possível registrar o aporte.");
    } finally {
      setSaving(false);
    }
  }

  return (
    <article className="rounded-xl bg-white p-6 shadow-sm">
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
          style={{ width: `${Math.min(goal.progressPercentage, 100)}%` }}
        />
      </div>

      <p className="text-sm text-slate-600">
        {currency.format(goal.currentAmount)} de {currency.format(goal.targetAmount)}
      </p>
      <p className="mt-2 text-sm text-slate-500">
        Faltam {currency.format(goal.remainingAmount)}
      </p>
      <p className="mt-4 text-sm font-medium text-slate-700">
        Aporte sugerido: {currency.format(goal.monthlyContributionNeeded)}/mês
      </p>

      <form onSubmit={handleProgressSubmit} className="mt-5 flex flex-wrap gap-2">
        <input
          required
          min="0.01"
          step="0.01"
          type="number"
          value={amount}
          onChange={(event) => setAmount(event.target.value)}
          placeholder="Valor do aporte"
          className="min-w-0 flex-1 rounded-lg border border-slate-200 px-3 py-2"
        />

        <button
          disabled={saving}
          className="rounded-lg bg-indigo-600 px-4 py-2 font-semibold text-white disabled:opacity-60"
        >
          {saving ? "Salvando..." : "Adicionar aporte"}
        </button>

        {error && <p className="w-full text-sm text-red-600">{error}</p>}
      </form>
    </article>
  );
}
