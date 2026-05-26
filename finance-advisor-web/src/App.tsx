import { useState, useEffect } from "react";
import { RecommendationCard } from "./components/RecommendationCard";
import { getRecommendation } from "./services/api";
import type { RecommendationResponse } from "./types/api";

function App() {
  const [recommendation, setRecommendation] =
    useState<RecommendationResponse | null>(null);

  useEffect(() => {
    getRecommendation("c3beaa9c-f3e6-4f7e-b8d5-6b218ce106b5").then((data) =>
      setRecommendation(data),
    );
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

export default App;
