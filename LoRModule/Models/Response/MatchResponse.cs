using LoRModule.Models.Dto;
using MongoDB.Bson.Serialization.Attributes;

namespace LoRModule.Models;

public class MatchResponse : MatchDto
{
    [BsonId] public string Id { get; set; }
    public string opponent_name { get; set; }
    public bool focus_player_win { get; set; }
    public string focus_player_deck_code { get; set; }
}