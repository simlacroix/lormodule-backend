namespace LoRModule.Models;

public class BasicStats
{
    public BasicStats(SummonerResponse summoner, string playerName, int profileIconId, List<MatchResponse> matchHistory)
    {
        this.summoner = summoner;
        this.playerName = playerName;
        this.profileIconId = profileIconId;
        this.matchHistory = matchHistory;
    }

    public SummonerResponse summoner { get; set; }
    public string playerName { get; set; }
    public int profileIconId { get; set; }
    public double winRatio { get; set; }
    
    public double totalMatches { get; set; }
    public double wins { get; set; }
    public double losses { get; set; }
    public int? rank { get; set; }
    public double? points { get; set; }
    public List<MatchResponse?> matchHistory { get; set; }
}