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

        long intersections = 0;
        for (var i = 0; i < lines.Count; i++)
        {
            var l1 = lines[i];
            for (var j = i + 1; j < lines.Count; j++)
            {
                var l2 = lines[j];
                var toAdd = l1.Intersections(l2);
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