namespace LoRModule;

public class LoRApiClient : HttpClient
{
    public LoRApiClient()
    {
        BaseAddress = new Uri(Globals.BaseUrl);
    }
}