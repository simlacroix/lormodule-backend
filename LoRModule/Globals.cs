namespace LoRModule;

public class Globals
{
    public const string BaseUrl = "https://na1.api.riotgames.com/";

    public static readonly string ApiKey = Environment.GetEnvironmentVariable("API_KEY_LOR") ??
                                           throw new Exception("LOR API KEY NOT SET");

    public static string BaseAmericaUrl = "https://americas.api.riotgames.com/";
}