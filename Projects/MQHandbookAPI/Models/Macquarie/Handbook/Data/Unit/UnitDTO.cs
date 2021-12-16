namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit;

using global::Macquarie.Handbook.Data.Unit;
using MQHandbookAPI.Models.Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

public class UnitDTO : MacquarieMetadataDTO
{
    //[JsonConverter(typeof(MacquarieEmbeddedJsonConverter<MacquarieUnitData>))]
    public UnitDataDTO UnitData { get; set; }
    public ushort CreditPoints { get; set; }
    public string Description { get; set; }
    public string Level { get; set; }
    public ushort PublishedInHandbook { get; set; }
    public string LevelDisplay { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public string Status { get; set; }
    public string Version { get; set; }
    public string Type { get; set; }
    //public override string ToString() => $"{UnitData.Code} {ImplementationYear}";
}
