﻿namespace webapi;

public class Game
{
    public string? Guid { get; set; }
    public string? CreatedBy { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int RoundTime { get; set; }
    public int Expiration { get; set; }
}