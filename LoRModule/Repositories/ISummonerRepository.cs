using LoRModule.Models;

namespace LoRModule.Repositories;

public interface ISummonerRepository
{
    public Task<SummonerResponse?> GetSummonerByName(string summonerName);
    public Task<SummonerResponse?> GetSummonerByPuuid(string puuid);
    public Task Create(SummonerResponse summonerResponse);
}