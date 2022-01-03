using System;
using MQHandbookLib.src.Macquarie.Handbook.Data;
using Newtonsoft.Json;

namespace MQHandbookLib.src.Macquarie.Handbook.Converters;

public class StringToUnitCodeConverter : JsonConverter<UnitCode>
{
    public override UnitCode ReadJson(JsonReader reader, Type objectType, UnitCode existingValue, bool hasExistingValue, JsonSerializer serializer) {
        return new UnitCode(reader.ReadAsString());
    }

    public override void WriteJson(JsonWriter writer, UnitCode value, JsonSerializer serializer) {
        serializer.Serialize(writer, value.ToString());
    }
}
