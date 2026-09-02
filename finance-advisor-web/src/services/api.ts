import axios from 'axios'
import type { RecommendationResponse, GoalResponse, CreateGoalRequest } from '../types/api'


const api = axios.create({
    baseURL: 'http://localhost:5077'
})

export async function getRecommendation(userId: string) : Promise<RecommendationResponse> {
    const response = api.get<RecommendationResponse>(`/api/recommendations/${userId}`)
    return (await response).data
}

export async function getGoals(userId: string): Promise<GoalResponse[]> {
    const response = api.get<GoalResponse[]>(`/api/goals/user/${userId}`);
    return (await response).data
}

export async function createGoal(request: CreateGoalRequest): Promise<GoalResponse> {
    const response = await api.post<GoalResponse>("/api/goals", request);
    return response.data;
}

export async function updateGoalProgress(goalId: string, ) {
    
}

export default api