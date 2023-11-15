namespace WebApi.Services;

using AutoMapper;
using WebApi.Entities;
using WebApi.Repositories;

public interface IJobService
{
    Task<Job> Create(Job job);
}

public class JobService : IJobService
{
    private IJobRepository _jobRepository;

    public JobService(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<Job> Create(Job job)
    {
        return await _jobRepository.Create(job);
    }
}