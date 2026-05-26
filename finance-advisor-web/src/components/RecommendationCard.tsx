import type { RecommendationResponse } from "../types/api";

type Props = {
    data: RecommendationResponse
}

export function RecommendationCard ({ data}: Props) {
    return (
        <div className="bg-black">
            <h1>Recomendação de como investir</h1>
            <h2>Renda total: {data.totalIncome}</h2>
            <h2>Despesas Fixas: {data.totalFixedExpenses}</h2>
            <h2>Valor para investir: {data.amountToInvest}</h2>
        </div>
    )
}
