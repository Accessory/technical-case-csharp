using System.Diagnostics;
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

        var positions = new HashSet<Position> { currentPosition };
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

                positions.Add(currentPosition);
            }
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
}