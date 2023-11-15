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

    public Int64 Intersections(Line other)
    {
        var selfIsHorizontal = this.IsHorizontal();
        var otherIsHorizontal = other.IsHorizontal();

        if (selfIsHorizontal && otherIsHorizontal)
        {
            return Start.Y == other.Start.Y ? other.GetHorizontalOverlap(this) : 0;
        }
        
        if (!selfIsHorizontal && !otherIsHorizontal)
        {
            return Start.X == other.Start.X ? GetVerticalOverlap(other) : 0;
        }

        if (selfIsHorizontal && !otherIsHorizontal)
        {
            return this.HasIntersection(other) ? 1 : 0;
        }

        return other.HasIntersection(this) ? 1 : 0;
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

    private Int64 GetVerticalOverlap(Line other)
    {
        var start = Math.Min(Start.Y, End.Y);
        var end = Math.Max(Start.Y, End.Y);
        var otherStart = Math.Min(other.Start.Y, other.End.Y);
        var otherEnd = Math.Max(other.Start.Y, other.End.Y);

        return Overlapping(start, end, otherStart, otherEnd);
    }

    private Int64 GetHorizontalOverlap(Line other)
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
    
    static bool LineIntersect(Position a1, Position a2, Position b1, Position b2)
    {
        var dx = a2.X - a1.X;
        var dy = a2.Y - a1.Y;
        var da = b2.X - b1.X;
        var db = b2.Y - b1.Y;

        if (da * dy - db * dx == 0) {
            return false;
        }

        var s = (dx * (b1.Y - a1.Y) + dy * (a1.X - b1.X)) / (da * dy - db * dx);
        var t = (da * (a1.Y - b1.Y) + db * (b1.X - a1.X)) / (db * dx - da * dy);

        return s is >= 0 and <= 1 && t is >= 0 and <= 1;
    }

    public long Intersections2(Line other)
    {
        var startX = Math.Min(Start.X, End.X);
        var endX = Math.Max(Start.X, End.X);
        var otherStartX= Math.Min(other.Start.X, other.End.X);
        var otherEndX = Math.Max(other.Start.X, other.End.X);
        
        var startY = Math.Min(Start.Y, End.Y);
        var endY = Math.Max(Start.Y, End.Y);
        var otherStartY = Math.Min(other.Start.Y, other.End.Y);
        var otherEndY = Math.Max(other.Start.Y, other.End.Y);

        if(this.IsHorizontal() == other.IsHorizontal())
        {
            long rtn = 0;
            for (long x = otherStartX; x <= otherEndX; x++)
            {
                for (long y = otherStartY; y <= otherEndY; y++)
                {
                    if (startX <= x && x <= endX && startY <= y && y <= endY)
                    {
                        rtn += 1;
                    }
                }
            }
        
            return rtn;
        }

        return LineIntersect(new Position() { X = startX, Y = startY }, new Position() { X = endX, Y = endY },
            new Position() { X = otherStartX, Y = otherStartY },
            new Position() { X = otherEndX, Y = otherEndY }) ? 1 : 0;
    }
}