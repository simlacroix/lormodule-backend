using LoRModule.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LoRModule.Repositories.Impl;

public class MatchRepository : Repository, IMatchRepository
{
    private readonly IMongoCollection<MatchResponse> _matchCollection;

    public MatchRepository(IOptions<LoRDatabaseSettings> lorDatabaseSettings) : base(lorDatabaseSettings)
    {
        _matchCollection =
            MongoDatabase.GetCollection<MatchResponse>(lorDatabaseSettings.Value.LorMatchesCollectionName);
    }

    public async Task Create(MatchResponse match)
    {
        await _matchCollection.InsertOneAsync(match);
    }


    public async Task<List<MatchResponse>> GetAllBySummoner(string summonerPuuid)
    {
        return await _matchCollection.Find(x => x.info.players.Any(y => y.puuid == summonerPuuid)).ToListAsync();
    }
}