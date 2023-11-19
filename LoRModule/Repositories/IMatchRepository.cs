using LoRModule.Models;

namespace LoRModule.Repositories;

public interface IMatchRepository
{
    public Task Create(MatchResponse match);
    public Task<List<MatchResponse>> GetAllBySummoner(string summonerPuuid);
}