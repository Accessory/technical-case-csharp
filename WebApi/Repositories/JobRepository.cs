namespace WebApi.Repositories;

using Dapper;
using WebApi.Entities;
using WebApi.Helpers;

public interface IJobRepository
{
    Task<Job> Create(Job job);
}

public class JobRepository : IJobRepository
{
    private readonly DataContext _context;

    public JobRepository(DataContext context)
    {
        _context = context;
    }

 public async Task<Job> Create(Job job)
    {
        using var connection = _context.CreateConnection();
        job.Id = (long)(await connection.ExecuteScalarAsync("Select nextval('job_id_seq')"))!;
        
        const string sql = "INSERT INTO public.job(id, \"timestamp\", commands, result, duration) VALUES (@Id, @TimeStamp, @Commands, @Result, @Duration);";
        await connection.ExecuteAsync(sql, job);
        return job;
    }
}