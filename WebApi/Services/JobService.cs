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
    private readonly IMapper _mapper;

    public JobService(
        IJobRepository jobRepository,
        IMapper mapper)
    {
        _jobRepository = jobRepository;
        _mapper = mapper;
    }

    public async Task<Job> Create(Job job)
    {
        return await _jobRepository.Create(job);
    }
}