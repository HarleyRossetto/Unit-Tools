using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CosmosDbTest;

public class CosmosRecord<T>
{
    public CosmosRecord(T data) => Record = data;

    [JsonPropertyName("id")]
    public string Id { get; set; }
    public T Record { get; set; }
}
