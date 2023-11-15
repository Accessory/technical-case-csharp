namespace WebApi.Models.Jobs;

public struct Position
{
    public long X { get; set; }
    public long Y { get; set; }

    public long Tolong()
    {
        return X + Y * 200_001;
    }
}