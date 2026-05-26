import type { RecommendationResponse } from "../types/api";

type Props = {
  data: RecommendationResponse;
};

export function RecommendationCard({ data }: Props) {
  const formatBRL = (value: number) =>
    value.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });

  const emergencyProgress =
    (data.emergencyFundCurrent / data.emergencyFundTarget) * 100;

  return (
    <div className="min-h-screen bg-slate-50 p-8">
      <div className="max-w-2xl mx-auto bg-white rounded-2xl shadow-lg p-6 space-y-6">
        {/* Header */}
        <div>
          <h1 className="text-2xl font-bold text-slate-800">
            Assistente Financeiro
          </h1>
          <p className="text-sm text-slate-500 mt-1">{data.priority}</p>
        </div>

        {/* grid de metricas */}
        <div className="grid grid-cols-3 gap-4">
          <div className="bg-blue-50 rounded-xl p-4">
            <p className="text-sm text-blue-500 font-bold">Renda Total</p>
            <p className="text-xl font-bold text-slate-800">
              {formatBRL(data.totalIncome)}
            </p>
          </div>
          <div className="bg-red-50 rounded-xl p-4">
            <p className="text-sm text-red-500 font-bold">Despesas fixas</p>
            <p className="text-xl font-bold text-slate-800">
              {formatBRL(data.totalFixedExpenses)}
            </p>
          </div>
          <div className="bg-green-50 rounded-xl p-4">
            <p className="text-sm text-green-500 font-bold">
              Valor para investir
            </p>
            <p className="text-xl font-bold text-slate-800">
              {formatBRL(data.amountToInvest)}
            </p>
          </div>
        </div>

        <div className="border-t border-slate-300" />

        {/* Reserva de emergência */}
        <div className="space-y-2">
          <div className="flex justify-between">
            <p className="text-sm font-bold text-slate-700">
              Reserva de emergência
            </p>
            <p className="text-sm text-slate-500">
              {formatBRL(data.emergencyFundCurrent)} /{" "}
              {formatBRL(data.emergencyFundTarget)}
            </p>
          </div>
          <div className="w-full bg-slate-200 rounded-full h-5">
            <div
              className="bg-green-500 h-4 rounded-full"
              style={{ width: `${emergencyProgress}%` }}
            />
          </div>
        </div>

        <div className="border-t border-slate-300" />

        {/* Alocações */}
        <div className="space-y-2">
          <p className="text-sm font-bold text-slate-700">
            Alocações sugeridas
          </p>
          {data.allocations.map((allocation) => (
            <div
              key={allocation.asset}
              className="flex justify-between items-center bg-slate-50 rounded-xl p-3"
            >
              <p className="text-sm text-slate-700">{allocation.asset}</p>
              <div className="flex gap-3">
                <span className="text-sm font-bold text-green-600">
                  {allocation.percentage}%
                </span>
                <span className="text-sm text-slate-500">
                  {formatBRL(allocation.amount)}
                </span>
              </div>
            </div>
          ))}
        </div>

        <div className="border-t border-slate-300" />

        {/* Insights */}
        <div className="space-y-2">
          <p className="text-sm font-bold text-slate-700">Insights</p>
          {data.insights.map((insight) => (
            <p
              key={insight}
              className="text-sm text-slate-600 bg-amber-50 rounded-xl p-3"
            >
              {insight}
            </p>
          ))}
        </div>

        <div className="border-t border-slate-300" />
      </div>
    </div>
  );
}
