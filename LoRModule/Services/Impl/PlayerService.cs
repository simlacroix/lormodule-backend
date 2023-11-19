using System.Net;
using LoRModule.Exceptions;
using LoRModule.Models;
using LoRModule.Repositories;
using Newtonsoft.Json;

namespace LoRModule.Services.Impl;

public class PlayerService : IPlayerService
{
    private readonly LoRApiClient _client;
    private readonly ILogger _logger;
    private readonly ISummonerRepository _summonerRepository;

    public PlayerService(ILogger<PlayerService> logger, ISummonerRepository summonerRepository)
    {
        _logger = logger;
        _summonerRepository = summonerRepository;
        _client = new LoRApiClient();
    }

    public async Task<string> checkIfSummonerExists(string name)
    {
        _logger.LogInformation($"Verifying that user {name} exists");
        var summoner = await getSummonerByName(name);

        if (summoner is null)
            throw new UserNotFoundException($"Summoner name doesn't exist {name}");

        _logger.LogDebug($"Response - {summoner.name}");
        return summoner.name;
    }

    public async Task<SummonerResponse?> getSummonerByPuuid(string puuid)
    {
        _logger.LogInformation($"Getting summoner with puuid {puuid} infos");
        var summoner = await _summonerRepository.GetSummonerByPuuid(puuid);

        if (summoner == null)
        {
            var callUrl = $"/lol/summoner/v4/summoners/by-puuid/{puuid}?api_key={Globals.ApiKey}";
            var response = await _client.GetAsync(callUrl);
            while (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                Thread.Sleep(2000);
                response = await _client.GetAsync(callUrl);
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                summoner = JsonConvert.DeserializeObject<SummonerResponse>(content);
                if (summoner != null) await _summonerRepository.Create(summoner);
            }
            else if (response.StatusCode != HttpStatusCode.TooManyRequests &&
                     response.StatusCode != HttpStatusCode.NotFound)
            {
                throw new Exception($"Error {response.StatusCode}: {response.RequestMessage}");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }
        
        _logger.LogDebug($"Response - {summoner}");
        return summoner;
    }

    public async Task<SummonerResponse?> getSummonerByName(string name)
    {
        _logger.LogInformation($"Getting summoner {name} infos");
        var summoner = await _summonerRepository.GetSummonerByName(name);

        if (summoner == null)
        {
            var callUrl = $"lol/summoner/v4/summoners/by-name/{name}?api_key={Globals.ApiKey}";
            var response = await _client.GetAsync(callUrl);
            while (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                Thread.Sleep(2000);
                response = await _client.GetAsync(callUrl);
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                summoner = JsonConvert.DeserializeObject<SummonerResponse>(content);
                if (summoner != null) await _summonerRepository.Create(summoner);
            }
            else if (response.StatusCode != HttpStatusCode.TooManyRequests &&
                     response.StatusCode != HttpStatusCode.NotFound)
            {
                throw new Exception($"Error {response.StatusCode}: {response.RequestMessage}");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        _logger.LogDebug($"Response - {summoner}");
        return summoner;
    }
}