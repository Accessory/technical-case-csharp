namespace WebApi.Entities;

public class Job
{
    public long Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int Commands { get; init; }
    public int Result { get; init; }
    public double Duration { get; set; }
}