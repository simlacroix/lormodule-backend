namespace LoRModule.Models;

public class LoRDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string LorMatchesCollectionName { get; set; } = null!;
    public string SummonerCollectionName { get; set; } = null!;
}