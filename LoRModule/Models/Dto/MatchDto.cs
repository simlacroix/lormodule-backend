namespace LoRModule.Models.Dto;

public class MatchDto
{
    public Metadata metadata { get; set; }
    public Info info { get; set; }
}

public class Info
{
    public string game_mode { get; set; }
    public string game_type { get; set; }
    public DateTime game_start_time_utc { get; set; }
    public string game_version { get; set; }
    public List<Player> players { get; set; }
    public int total_turn_count { get; set; }
}

public class Metadata
{
    public string data_version { get; set; }
    public string match_id { get; set; }
    public List<string> participants { get; set; }
}

public class Player
{
    public string puuid { get; set; }
    public string deck_id { get; set; }
    public string deck_code { get; set; }
    public List<string> factions { get; set; }
    public string game_outcome { get; set; }
    public int order_of_play { get; set; }
}

public class Root
{
    public Metadata metadata { get; set; }
    public Info info { get; set; }
}