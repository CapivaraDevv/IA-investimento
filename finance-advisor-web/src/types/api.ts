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

export type GoalResponse = {
    id: string;
    type: number;
    description: string;
    targetAmount: number;
    currentAmount: number;
    remainingAmount: number;
    progressPercentage: number;
    monthlyContributionNeeded: number;
    deadlineMonths: number;
    status: number;
}

export type CreateGoalRequest = {
    userId: string;
    type: number;
    description: string;
    targetAmount: number;
    deadlineMonths: number;
}