export type UserProfile = {
    id: string;
    name: string;
    salary: number;
    createdAt: string;
}

export type FixedExpense = {
    id: string;
    category: number;
    description: string;
    amount: number;
    isActive: boolean;
}

export type RecommendationResponse = {
    recommendationId: string;
    totalIncome: number;
    totalFixedExpenses: number;
    surplus: number;
    amountToInvest: number;
    emergencyFundTarget: number;
    emergencyFundCurrent: number;
    emergencyFundComplete: boolean;
    priority: string;
    allocations: AssetAllocation[]
    insights: string[]
}

export type AssetAllocation = {
    asset: string;
    amount: number;
    percentage: number;
    reason: string;
}