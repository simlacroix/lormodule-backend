using LoRModule.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LoRModule.Repositories.Impl;

public class SummonerRepository : Repository, ISummonerRepository
{
    private readonly IMongoCollection<SummonerResponse> _playerAccountCollection;

    public SummonerRepository(IOptions<LoRDatabaseSettings> lorDatabaseSettings) : base(lorDatabaseSettings)
    {
        _playerAccountCollection =
            MongoDatabase.GetCollection<SummonerResponse>(lorDatabaseSettings.Value.SummonerCollectionName);
    }

    public async Task<SummonerResponse?> GetSummonerByName(string summonerName)
    {
        return await _playerAccountCollection
            .Find(x => string.Equals(x.name, summonerName, StringComparison.CurrentCultureIgnoreCase))
            .FirstOrDefaultAsync();
    }

    public async Task<SummonerResponse?> GetSummonerByPuuid(string puuid)
    {
        return await _playerAccountCollection
            .Find(x => string.Equals(x.puuid, puuid, StringComparison.CurrentCultureIgnoreCase))
            .FirstOrDefaultAsync();
    }

    public async Task Create(SummonerResponse summonerResponse)
    {
        await _playerAccountCollection.InsertOneAsync(summonerResponse);
    }
}