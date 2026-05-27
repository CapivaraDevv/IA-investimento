type Props = {
  steps: string[];
  currentStep: number;
};

export default function StepIndicator({ steps, currentStep }: Props) {
  return (
    <div className="flex gap-6 items-center">
      {steps.map((step, index) => (
        <div key={step} className="flex items-center gap-2">
          <div
            className={`rounded-full w-8 h-8 flex items-center justify-center font-bold text-sm ${
              index === currentStep
                ? "bg-blue-600 text-white"
                : "bg-slate-200 text-slate-500"
            }`}
          >
            {index + 1}
          </div>
          <p
            className={`text-sm ${
              index === currentStep
                ? "text-blue-600 font-bold"
                : "text-slate-400"
            }`}
          >
            {step}
          </p>
        </div>
      ))}
    </div>
  );
}
