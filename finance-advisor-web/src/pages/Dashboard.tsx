import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { RecommendationCard } from "../components/RecommendationCard";
import { getRecommendation } from "../services/api";
import type { RecommendationResponse } from "../types/api";

export default function Dashboard() {
  const [recommendation, setRecommendation] =
    useState<RecommendationResponse | null>(null);

  const { userId } = useParams<{ userId: string }>();

  useEffect(() => {
    if (!userId) return;

    getRecommendation(userId).then((data) => setRecommendation(data));
  
    }, []);

  return (
    <div className="bg-black">
      {recommendation === null ? (
        <p>Carregando...</p>
      ) : (
        <RecommendationCard data={recommendation} />
      )}
    </div>
  );
}
