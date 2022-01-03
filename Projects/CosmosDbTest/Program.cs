using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CosmosDbTest;
using Macquarie.Handbook.Data;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MQHandbookLib.src.Helpers;
using MQHandbookLib.src.Macquarie.Handbook;
using MQHandbookLib.src.Macquarie.Handbook.JSON;
using Newtonsoft.Json;

class Program
{
    public static async Task Main(string[] args) {
        var primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        var endpointUrl = "https://localhost:8081";

        var databaseId = "HandbookDb";
        var containerId = "UnitGuideContainer";

        Console.WriteLine("Creating client...");
        CosmosClient cosmosClient = new(endpointUrl, primaryKey);

        Console.WriteLine("Creating Database...");
        //Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        Database database = cosmosClient.GetDatabase(databaseId);

        Console.WriteLine("Creating Container...");
        var partitionKeyPath = nameof(MacquarieUnit.CodeSubject);
        //Container container = await database.CreateContainerIfNotExistsAsync(containerId, partitionKeyPath);
        Container container = database.GetContainer(containerId);

/*
        try {
            Console.WriteLine("Creating record...");
            var data = await new MacquarieHandbook(new NullLogger<MacquarieHandbook>(),
                                                   new NullLogger<JsonSerialisationHelper>(),
                                                   new DateTimeProvider()).GetUnit("CHEM1001");

            var unitCodeSubjectComponent = data.Code[..4];
            var partitionKey = new PartitionKey(unitCodeSubjectComponent);

            ItemResponse<MacquarieUnit> itemResponse = await container.CreateItemAsync(data,
                                                                                       partitionKey);

            Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n",
                              itemResponse.Resource.Id,
                              itemResponse.RequestCharge);
        } catch (Exception ex) {
            Console.WriteLine(ex.ToString());
        }
        */

        var result = await container.ReadItemAsync<MacquarieUnit>("CHEM1001.2021", new("CHEM"));
        Console.WriteLine("Retrieved item in database with id: {0} Operation consumed {1} RUs.\n",
                              result.Resource.Id,
                              result.RequestCharge);

    }
}