namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Shared;

public class LearningOutcomeDTO
{
        public string Description { get; set; }
        public string Code { get; set; }
        public string Number { get; set; }
        //public string Order { get; set; }

        public override string ToString() {
            return Description;
        }
}
