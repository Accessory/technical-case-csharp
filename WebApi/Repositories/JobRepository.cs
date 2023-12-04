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
        const string sql = "INSERT INTO public.job(\"timestamp\", commands, result, duration) VALUES (@TimeStamp, @Commands, @Result, @Duration) RETURNING *;";
        long jobId = (long) (await connection.ExecuteScalarAsync(sql, job)!)!;
        job.Id = jobId;
        return job;
    }
}