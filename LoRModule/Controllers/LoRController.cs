using LoRModule.Exceptions;
using LoRModule.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LoRModule.Controllers;

[ApiController]
[Route("[controller]")]
public class LoRController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IMatchService _matchService;
    private readonly IPlayerService _playerService;

    public LoRController(IPlayerService playerService, IMatchService matchService, ILogger<LoRController> logger)
    {
        _playerService = playerService;
        _matchService = matchService;
        _logger = logger;
    }

    [HttpGet("getStatsForPlayer")]
    public async Task<IActionResult> GetStatsForPlayer(string summonerName)
    {
        _logger.LogInformation($"Getting Stats for summoner: {summonerName}");

        try
        {
            var summoner = await _playerService.getSummonerByName(summonerName);

            if (summoner is null)
            {
                _logger.LogInformation("Summoner is null, throwing exception");
                throw new Exception();
            }

            var stats = await _matchService.GenerateBasicStats(summoner);
            return Ok(JsonConvert.SerializeObject(stats));
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"exception while getting stats for: {summonerName}");
            return BadRequest(e.Message);
        }
    }

    [HttpGet("userExists")]
    public async Task<IActionResult> UserExists(string summonerName)
    {
        try
        {
            return Ok(await _playerService.checkIfSummonerExists(summonerName));
        }
        catch (UserNotFoundException e)
        {
            _logger.LogInformation(e, $"{summonerName} was not found in Riot'S API");
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"exception while verifying if summoner: {summonerName} exists");
            return BadRequest(e.Message);
        }
    }
}