using Macquarie.Handbook.Data.Shared;

namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Shared;

public class MaterialMetadataDTO
{
        public ushort ImplementationYear { get; set; }
        public string Status { get; set; }
        public string AcademicOrganisation { get; set; }
        public string School { get; set; }
        public UInt16 CreditPoints { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string SearchTitle { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public string CreditPointsHeader { get; set; }
        public string Version { get; set; }
        public string ClassName { get; set; }
        public string Overview { get; set; }
        public string AcademicItemType { get; set; }
        public List<Requirement> InherentRequirements { get; set; }
        public List<Requirement> OtherRequirements { get; set; }
        public string ExternalProvider { get; set; }
        public List<string> Links { get; set; }
        public LabelledValue PublishedInHandbook { get; set; }
}
