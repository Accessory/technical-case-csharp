using WebApi.Models.Jobs;

namespace WebApi.Helpers;

public class Line
{
    public Position Start;
    public Position End;

    public Line(Position start, Position end)
    {
        Start = start;
        End = end;
    }

    public long Intersections(Line other)
    {
        var selfIsHorizontal = this.IsHorizontal();
        var otherIsHorizontal = other.IsHorizontal();

        return selfIsHorizontal switch
        {
            true when otherIsHorizontal => Start.Y == other.Start.Y ? other.GetHorizontalOverlap(this) : 0,
            false when !otherIsHorizontal => Start.X == other.Start.X ? GetVerticalOverlap(other) : 0,
            true when !otherIsHorizontal => this.HasIntersection(other) ? 1 : 0,
            _ => other.HasIntersection(this) ? 1 : 0
        };
    }

    private bool HasIntersection(Line other)
    {
        var thisStartX = Math.Min(this.Start.X, this.End.X);
        var thisEndX = Math.Max(this.Start.X, this.End.X);
        if (thisStartX > other.Start.X || thisEndX < other.Start.X)
        {
            return false;
        }

        var otherStartY = Math.Min(other.Start.Y, other.End.Y);
        var otherEndY = Math.Max(other.Start.Y, other.End.Y);

        return otherStartY < this.Start.Y && this.Start.Y < otherEndY;
    }

    private long GetVerticalOverlap(Line other)
    {
        var start = Math.Min(Start.Y, End.Y);
        var end = Math.Max(Start.Y, End.Y);
        var otherStart = Math.Min(other.Start.Y, other.End.Y);
        var otherEnd = Math.Max(other.Start.Y, other.End.Y);

        return Overlapping(start, end, otherStart, otherEnd);
    }

    private long GetHorizontalOverlap(Line other)
    {
        var start = Math.Min(Start.X, End.X);
        var end = Math.Max(Start.X, End.X);
        var otherStart = Math.Min(other.Start.X, other.End.X);
        var otherEnd = Math.Max(other.Start.X, other.End.X);

        return Overlapping(start, end, otherStart, otherEnd);
    }

    private long Overlapping(long start, long end, long otherStart, long otherEnd)
    {
        var startMax = Math.Max(otherStart, start);
        var endMin = Math.Min(otherEnd, end);
        var overlap = endMin - startMax;
        
        return overlap == 0 ? 1 : Math.Max(0, overlap + 1);
    }

    private bool IsHorizontal()
    {
        return Start.Y == End.Y;
    }
}