namespace WebApi.Entities;

public class Job
{
    public long Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int Commands { get; set; }
    public int Result { get; set; }
    public double Duration { get; set; }
}