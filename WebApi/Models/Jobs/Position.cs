namespace WebApi.Models.Jobs;

public struct Position
{
    public Int64 X { get; set; }
    public Int64 Y { get; set; }

    public long ToInt64()
    {
        return X + Y * 200_001;
    }
}