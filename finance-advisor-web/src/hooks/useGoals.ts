import { useCallback, useEffect, useState } from "react";
import { getGoals } from "../services/api";
import type { GoalResponse } from "../types/api";

export function useGoals(userId?: string) {
  const [goals, setGoals] = useState<GoalResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const reload = useCallback(async () => {
    if (!userId) {
      setLoading(false);
      return;
    }

    setLoading(true);
    setError("");

    try {
      const data = await getGoals(userId);
      setGoals(data);
    } catch {
      setError("Não foi possível carregar suas metas.");
    } finally {
      setLoading(false);
    }
  }, [userId]);

  useEffect(() => {
    void reload();
  }, [reload]);

  return { goals, loading, error, reload };
}