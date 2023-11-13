namespace WebApi.Models.Jobs;

public struct Command
{
    public string Direction { get; set; }
    public int Steps { get; set; }
}