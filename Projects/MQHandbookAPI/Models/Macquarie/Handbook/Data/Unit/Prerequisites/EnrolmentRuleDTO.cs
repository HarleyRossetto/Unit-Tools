namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit.Prerequisites
{
    public record EnrolmentRuleDTO
    {
        public string Description { get; init; }
        public string Type { get; init; }
        public ushort Order { get; init; }

        public override string ToString() => Description;
    }
}