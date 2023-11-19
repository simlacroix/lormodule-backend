using LoRModule.Models;

namespace LoRModule.Services;

public interface IPlayerService
{
    public Task<string> checkIfSummonerExists(string name);
    public Task<SummonerResponse?> getSummonerByName(string name);
    public Task<SummonerResponse?> getSummonerByPuuid(string puuid);
}