namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit;

public record AssessmentDTO
{
        public string AssessmentTitle { get; init; }
        public string Type { get; init; }
        public string Weight { get; init; }
        public string Description { get; init; }
        public string AppliesToAllOfferings { get; init; }
        public string HurdleTask { get; init; }
        public string Offerings { get; init; }
        public string Individual { get; init; }

    public override string ToString() => Description;
}
