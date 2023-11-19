using LoRModule.Models;

namespace LoRModule.Services;

public interface IMatchService
{
    public Task<BasicStats> GenerateBasicStats(SummonerResponse summoner);
}