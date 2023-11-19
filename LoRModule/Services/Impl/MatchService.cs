using System.Net;
using LoRModule.Models;
using LoRModule.Models.Dto;
using LoRModule.Repositories;
using Newtonsoft.Json;

namespace LoRModule.Services.Impl;

public class MatchService : IMatchService
{
    private readonly HttpClient _client;
    private readonly ILogger _logger;
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerService _playerService;
    private PlayerDto masterPlayers;

    public MatchService(IMatchRepository matchRepository, ILogger<MatchService> logger, IPlayerService playerService)
    {
        _logger = logger;
        _matchRepository = matchRepository;
        _client = new HttpClient();
        _client.BaseAddress = new Uri(Globals.BaseAmericaUrl);
        _playerService = playerService;
    }

    public async Task<BasicStats> GenerateBasicStats(SummonerResponse summoner)
    {
        _logger.LogInformation($"Generating basic stats for summoner: {summoner.name}");

        if (masterPlayers is null)
            masterPlayers = await GetMasterPlayers();
        var matchIds = await GetMatchIdsFromSummonerPuuid(summoner.puuid);
        var matches = await _matchRepository.GetAllBySummoner(summoner.puuid);

        if (matchIds != null)
            foreach (var id in matchIds)
                if (matches.All(x => x.Id != id))
                {
                    var match = await GetMatchFromId(id);
                    if (match != null)
                    {
                        match.focus_player_win =
                            match.info.players.Find(x => x.puuid == summoner.puuid).game_outcome == "win";
                        match.focus_player_deck_code =
                            match.info.players.Find(x => x.puuid == summoner.puuid).deck_code;
                        string? opponentPuiid = match.info.players.Find(x => x.puuid != summoner.puuid)?.puuid;
                        if (opponentPuiid is null)
                            match.opponent_name = "Bot";
                        else
                        {
                            var opponent = await _playerService.getSummonerByPuuid(opponentPuiid);
                            match.opponent_name = opponent != null ? opponent.name : "unavailable";                        
                        }
                        match.Id = id;
                        await _matchRepository.Create(match);
                        matches.Add(match);
                    }
                }
        matches.Sort((x, y) => y.info.game_start_time_utc.CompareTo(x.info.game_start_time_utc));
        var response = new BasicStats(summoner, summoner.name, summoner.profileIconId, matches);
        UpdateCountStats(response);
        SetMasterStats(response);

        _logger.LogDebug($"Response - {summoner.name}");
        return response;
    }

    private void UpdateCountStats(BasicStats stats)
    {
        double winCount = 0;
        double totalGames = stats.matchHistory.Count;
        foreach (var match in stats.matchHistory)
            if (match.focus_player_win)
                winCount++;
        stats.wins = winCount;
        stats.losses = totalGames - winCount;
        stats.totalMatches = totalGames;
        stats.winRatio = totalGames != 0 ? winCount / totalGames : 0;
    }

    private void SetMasterStats(BasicStats stats)
    {
        MasterPlayer? player = masterPlayers.players.Find(x => x.name == stats.summoner.name);
        if (!(player is null))
        {
            stats.points = player.lp;
            stats.rank = player.rank;
        }
    }
    

    private async Task<PlayerDto> GetMasterPlayers()
    {
        var callUrl = $"/lor/ranked/v1/leaderboards?api_key={Globals.ApiKey}";
        var response = await _client.GetAsync(callUrl);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PlayerDto>(content);
        }
        throw new Exception($"Error {response.StatusCode}: {response.RequestMessage}");
    }

    private async Task<MatchResponse?> GetMatchFromId(string matchId)
    {
        _logger.LogInformation($"Getting match: {matchId} infos");

        var callUrl = $"/lor/match/v1/matches/{matchId}?api_key={Globals.ApiKey}";
        var response = await _client.GetAsync(callUrl);
        while (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            Thread.Sleep(2000);
            response = await _client.GetAsync(callUrl);
        }

        MatchResponse? match;
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            match = JsonConvert.DeserializeObject<MatchResponse>(content);
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        else
        {
            throw new Exception($"Error {response.StatusCode}: {response.RequestMessage}");
        }

        _logger.LogDebug($"Response - {match}");
        return match;
    }

    private async Task<List<string>?> GetMatchIdsFromSummonerPuuid(string puuid)
    {
        _logger.LogInformation($"Getting matches id for summoner: {puuid}");

        var callUrl = $"/lor/match/v1/matches/by-puuid/{puuid}/ids?api_key={Globals.ApiKey}";
        var response = await _client.GetAsync(callUrl);
        List<string>? matches;
        while (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            Thread.Sleep(2000);
            response = await _client.GetAsync(callUrl);
        }

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            matches = JsonConvert.DeserializeObject<List<string>>(content);
        }
        else
        {
            throw new Exception($"Error {response.StatusCode}: {response.RequestMessage}");
        }

        _logger.LogDebug($"Response - {matches}");
        return matches;
    }

    private double calculateWinRatio(List<MatchResponse> matches, string puuid)
    {
        double winCount = 0;
        double totalGames = matches.Count;
        foreach (var match in matches)
            if (match.info.players.First(x => x.puuid == puuid).game_outcome == "win")
                winCount++;

        return totalGames != 0 ? winCount / totalGames : 0;
    }
}