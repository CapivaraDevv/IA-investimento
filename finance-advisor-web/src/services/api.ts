import axios from 'axios'
import type { RecommendationResponse } from '../types/api'

const api = axios.create({
    baseURL: 'http://localhost:5077'
})

export async function getRecommendation(userId: string) : Promise<RecommendationResponse> {
    const response = api.get<RecommendationResponse>(`/api/recommendations/${userId}`)
    return (await response).data
}

export default api