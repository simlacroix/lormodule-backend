namespace LoRModule.Models.Dto;

public class PlayerDto
{
    public List<MasterPlayer> players { get; set; }
}

public class MasterPlayer
{
    public string name { get; set; }
    public int rank { get; set; }
    public double lp { get; set; }
}