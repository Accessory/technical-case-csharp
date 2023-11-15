using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Jobs;

public class EnterPathRequest
{
    public EnterPathRequest(List<Command> commands)
    {
        this.Commands = commands;
    }

    [Required]
    public Position Start { get; init; }

    [Required]
    public List<Command> Commands { get; }
}