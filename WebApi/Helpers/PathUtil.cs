using System.Collections;
using System.Diagnostics;
using Advanced.Algorithms.Geometry;
using WebApi.Entities;
using WebApi.Models.Jobs;

namespace WebApi.Helpers;

internal static class PathUtil
{
    internal static Job CreateJobFromEnterPathRequest(EnterPathRequest request)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        var currentPosition = request.Start;

        var sum = request.Commands.Sum(r => r.Steps);
        var positions = new List<Int64>(sum + 1) { currentPosition.ToInt64() };
        foreach (var command in request.Commands)
        {
            for (var i = 0; i < command.Steps; i++)
            {
                switch (command.Direction.ToLower())
                {
                    case "north":
                        --currentPosition.Y;
                        break;
                    case "south":
                        ++currentPosition.Y;
                        break;
                    case "west":
                        --currentPosition.X;
                        break;
                    case "east":
                        ++currentPosition.X;
                        break;
                }

                positions.Add(currentPosition.ToInt64());
            }
        }

        positions.Sort();

        var lastNumber = long.MaxValue;
        var doubleNumber = 0;
        for (int i = positions.Count - 1; i >= 0; i--)
        {
            if (lastNumber == positions[i])
            {
                ++doubleNumber;
            }

            lastNumber = positions[i];
        }

        watch.Stop();
        return new Job
        {
            Timestamp = DateTime.Now,
            Result = positions.Count - doubleNumber,
            Duration = watch.Elapsed.TotalSeconds,
            Commands = request.Commands.Count
        };
    }

    internal static Job CreateJobFromEnterPathRequest2(EnterPathRequest request)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        var currentPosition = request.Start;

        var sum = request.Commands.Sum(r => r.Steps);
        var positions = new HashSet<Position>(sum + 1) { currentPosition };
        foreach (var command in request.Commands)
        {
            var subPositions = new List<Position>(command.Steps);
            for (var i = 0; i < command.Steps; i++)
            {
                switch (command.Direction.ToLower())
                {
                    case "north":
                        --currentPosition.Y;
                        break;
                    case "south":
                        ++currentPosition.Y;
                        break;
                    case "west":
                        --currentPosition.X;
                        break;
                    case "east":
                        ++currentPosition.X;
                        break;
                }

                subPositions.Add(currentPosition);
            }

            positions.UnionWith(subPositions);
        }

        watch.Stop();

        return new Job
        {
            Timestamp = DateTime.Now,
            Result = positions.Count,
            Duration = watch.Elapsed.TotalSeconds,
            Commands = request.Commands.Count
        };
    }

    // Inspired by https://github.com/dscherdi/tibber_challenge
    internal static Job CreateJobFromEnterPathRequest3(EnterPathRequest request)
    {
        var watch = Stopwatch.StartNew();
        var currentPoint = (request.Start.X, request.Start.Y);

        var lines = new List<Advanced.Algorithms.Geometry.Line>();
        var edges = new HashSet<(long, long)>
        {
            (currentPoint.X, currentPoint.Y)
        };

        var count = 1;
        foreach (var command in request.Commands)
        {
            // Calculate the movement
            int dx = 0, dy = 0;
            switch (command.Direction.ToLower())
            {
                case "east":
                    dx = 1;
                    break;
                case "west":
                    dx = -1;
                    break;
                case "north":
                    dy = 1;
                    break;
                case "south":
                    dy = -1;
                    break;
                default:
                    throw new ArgumentException($"Invalid direction: {command.Direction}");
            }

            var endX = currentPoint.X + (dx * command.Steps);
            var endY = currentPoint.Y + (dy * command.Steps);

            (long X, long Y) nextPoint = (endX, endY);

            var newLine = new Advanced.Algorithms.Geometry.Line(
                new Advanced.Algorithms.Geometry.Point(currentPoint.X, currentPoint.Y),
                new Advanced.Algorithms.Geometry.Point(nextPoint.X, nextPoint.Y));
            if (newLine.IsHorizontal == lines.LastOrDefault()?.IsHorizontal ||
                newLine.IsVertical == lines.LastOrDefault()?.IsVertical)
            {
                var lastLine = lines.LastOrDefault();
                if (lastLine != null && newLine.IsHorizontal && newLine.Left.Y == lastLine.Left.Y)
                {
                    if (Math.Max(newLine.Left.X, lastLine.Left.X) <= Math.Min(newLine.Right.X, lastLine.Right.X))
                    {
                        var nonOverlappingLength =
                            Math.Abs((int)(newLine.Right.X - Math.Max(newLine.Left.X, lastLine.Right.X)));
                        count += nonOverlappingLength;
                    }
                }
                else if (lastLine != null && newLine.IsVertical && newLine.Left.X == lastLine.Left.X)
                {
                    if (Math.Max(newLine.Left.Y, lastLine.Left.Y) <= Math.Min(newLine.Right.Y, lastLine.Right.Y))
                    {
                        var nonOverlappingLength =
                            Math.Abs((int)(newLine.Right.Y - Math.Max(newLine.Left.Y, lastLine.Right.Y)));
                        count += nonOverlappingLength;
                    }
                }
            }
            else
            {
                if (edges.Contains((nextPoint.X, nextPoint.Y)))
                {
                    count--;
                }

                count += Math.Abs((int)(newLine.Right.X - newLine.Left.X)) +
                         Math.Abs((int)(newLine.Right.Y - newLine.Left.Y));
            }

            currentPoint = nextPoint;

            edges.Add((nextPoint.X, nextPoint.Y));
            lines.Add(newLine);
        }

        var min = (edges.Min(x => x.Item1), edges.Min(x => x.Item2));
        long offset = 0;
        if (min.Item1 < 0 || min.Item2 < 0)
        {
            offset = Math.Abs(min.Item1) > Math.Abs(min.Item2) ? Math.Abs(min.Item1) : Math.Abs(min.Item2);
            edges = new HashSet<(long, long)>(edges.Select(x => (x.Item1 + offset, x.Item2 + offset)));
            lines = new List<Advanced.Algorithms.Geometry.Line>(lines.Select(x => new Advanced.Algorithms.Geometry.Line(
                new Point(x.Left.X + offset, x.Left.Y + offset),
                new Point(x.Right.X + offset, x.Right.Y + offset))));
        }

        var bentleyOttmannAlgorithm = new BentleyOttmann();
        var intersections = bentleyOttmannAlgorithm.FindIntersections(lines.ToArray())
            .Where((p, _) => !edges.Contains(((int)p.Key.X, (int)p.Key.Y))).ToList();

        count -= intersections.Count;
        watch.Stop();

        return new Job()
        {
            Timestamp = DateTime.Now,
            Result = count,
            Duration = watch.Elapsed.TotalSeconds,
            Commands = request.Commands.Count
        };
    }

    internal static Job CreateJobFromEnterPathRequest4(EnterPathRequest request)
    {
        var watch = Stopwatch.StartNew();
        var currentPosition = request.Start;

        var lines = new List<Helpers.Line>();

        var sum = 1;
        var toCheck = request.Commands.Count - 1;

        for (var i = 0; i < request.Commands.Count; i++)
        {
            var command = request.Commands[i];
            sum += command.Steps;
            switch (command.Direction.ToLower())
            {
                case "north":
                    lines.Add(new Line(currentPosition,
                        currentPosition with { Y = currentPosition.Y + (command.Steps - (toCheck == i ? 0 : 1)) }));
                    currentPosition.Y += command.Steps;
                    break;
                case "south":
                    lines.Add(new Line(currentPosition,
                        currentPosition with { Y = currentPosition.Y - (command.Steps - (toCheck == i ? 0 : 1)) }));
                    currentPosition.Y -= command.Steps;
                    break;
                case "west":
                    lines.Add(new Line(currentPosition,
                        currentPosition with { X = currentPosition.X - (command.Steps - (toCheck == i ? 0 : 1)) }));
                    currentPosition.X -= command.Steps;
                    break;
                case "east":
                    lines.Add(new Line(currentPosition,
                        currentPosition with { X = currentPosition.X + (command.Steps - (toCheck == i ? 0 : 1)) }));
                    currentPosition.X += command.Steps;
                    break;
            }
        }

        Int64 intersections = 0;
        for (int i = 0; i < lines.Count; i++)
        {
            var l1 = lines[i];
            for (int j = i + 1; j < lines.Count; j++)
            {
                var l2 = lines[j];
                var toAdd = l1.Intersections(l2);
                // if (toAdd == 1)
                // {
                //     Console.WriteLine("Intersection: " + l1.Start.X + " " + l1.Start.Y + " " + l1.End.X + " " + l1.End.Y +
                //                       " " + l2.Start.X + " " + l2.Start.Y + " " + l2.End.X + " " + l2.End.Y);
                // }
                intersections += toAdd;
            }
        }

        watch.Stop();
        return new Job()
        {
            Timestamp = DateTime.Now,
            Result = (int)(sum - intersections),
            Duration = watch.Elapsed.TotalSeconds,
            Commands = request.Commands.Count
        };
    }

    internal static Job CreateJobFromEnterPathRequest5(EnterPathRequest request)
    {
        var watch = Stopwatch.StartNew();
        var currentPosition = new Point(request.Start.X + 100000, request.Start.Y + 100000);

        var lines = new List<Advanced.Algorithms.Geometry.Line>();

        var sum = 1;

        foreach (var command in request.Commands)
        {
            sum += command.Steps;
            switch (command.Direction.ToLower())
            {
                case "north":
                    lines.Add(new Advanced.Algorithms.Geometry.Line(currentPosition,
                        new Point(currentPosition.X, currentPosition.Y + command.Steps - 1)));
                    currentPosition = new Point(currentPosition.X, currentPosition.Y + command.Steps);
                    break;
                case "south":
                    lines.Add(new Advanced.Algorithms.Geometry.Line(currentPosition, new Point(currentPosition.X,
                        currentPosition.Y - (command.Steps - 1))));
                    currentPosition = new Point(currentPosition.X, currentPosition.Y - command.Steps);
                    break;
                case "west":
                    lines.Add(new Advanced.Algorithms.Geometry.Line(currentPosition,
                        new Point(currentPosition.X - (command.Steps - 1), currentPosition.Y)));
                    currentPosition = new Point(currentPosition.X - command.Steps, currentPosition.Y);
                    break;
                case "east":
                    lines.Add(new Advanced.Algorithms.Geometry.Line(currentPosition,
                        new Point(currentPosition.X + (command.Steps - 1), currentPosition.Y)));
                    currentPosition = new Point(currentPosition.X + command.Steps, currentPosition.Y);
                    break;
            }
        }

        var bentleyOttmannAlgorithm = new BentleyOttmann();
        var intersections = bentleyOttmannAlgorithm.FindIntersections(lines).Count;

        watch.Stop();
        return new Job()
        {
            Timestamp = DateTime.Now,
            Result = (int)(sum - intersections),
            Duration = watch.Elapsed.TotalSeconds,
            Commands = request.Commands.Count
        };
    }


    internal static Job CreateJobFromEnterPathRequest6(EnterPathRequest request)
    {
        var watch = Stopwatch.StartNew();
        var currentPosition = request.Start;

        var lines = new List<Helpers.Line>();

        var sum = 1;

        var toCheck = request.Commands.Count - 1;

        for (var i = 0; i < request.Commands.Count; i++)
        {
            var command = request.Commands[i];
            sum += command.Steps;
            switch (command.Direction.ToLower())
            {
                case "north":
                    lines.Add(new Line(currentPosition,
                        currentPosition with { Y = currentPosition.Y + (command.Steps - (toCheck == i ? 0 : 1)) }));
                    currentPosition.Y += command.Steps;
                    break;
                case "south":
                    lines.Add(new Line(currentPosition,
                        currentPosition with { Y = currentPosition.Y - (command.Steps - (toCheck == i ? 0 : 1)) }));
                    currentPosition.Y -= command.Steps;
                    break;
                case "west":
                    lines.Add(new Line(currentPosition,
                        currentPosition with { X = currentPosition.X - (command.Steps - (toCheck == i ? 0 : 1)) }));
                    currentPosition.X -= command.Steps;
                    break;
                case "east":
                    lines.Add(new Line(currentPosition,
                        currentPosition with { X = currentPosition.X + (command.Steps - (toCheck == i ? 0 : 1)) }));
                    currentPosition.X += command.Steps;
                    break;
            }
        }

        Int64 intersections = 0;
        for (int i = 0; i < lines.Count; i++)
        {
            var l1 = lines[i];
            for (int j = i + 1; j < lines.Count; j++)
            {
                var l2 = lines[j];
                var toAdd = l1.Intersections2(l2);
                intersections += toAdd;
            }
        }

        watch.Stop();
        return new Job()
        {
            Timestamp = DateTime.Now,
            Result = (int)(sum - intersections),
            Duration = watch.Elapsed.TotalSeconds,
            Commands = request.Commands.Count
        };
    }
}