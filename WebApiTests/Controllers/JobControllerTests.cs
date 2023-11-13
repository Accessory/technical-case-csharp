using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApi.Controllers;
using WebApi.Entities;
using WebApi.Models.Jobs;
using WebApi.Services;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace WebApiTests.Controllers
{
    [TestClass]
    public class JobControllerTests
    {
        private readonly JobController _sut;
        private readonly Mock<IJobService> _jobRepositoryMock;

        public JobControllerTests()
        {
            
            _jobRepositoryMock = new Mock<IJobService>();
            _sut = new JobController(_jobRepositoryMock.Object);
        }


        [Fact]
        public void HalloWeltTest()
        {
            var result = _sut.HalloWelt();
            Assert.AreEqual(((result.Result as OkObjectResult)!).Value, "Hallo Welt");
        }

        [Fact]
        public void EnterPathTest()
        {
            var command1 = new Command() { Steps = 5, Direction = "north" };
            var commands = new List<Command>() { command1 };
            var enterPath = new EnterPathRequest(commands);

            var job = new Job()
            {
                Id = 1,
                Result = 6,
                Duration = 0.001,
                Commands = 1,
                Timestamp = DateTime.Now,
            };

            _jobRepositoryMock.Setup(x => x.Create(It.IsAny<Job>())).Returns(Task.FromResult(job));
            
            var result = _sut.EnterPath(enterPath);
            Assert.AreEqual(job, ((result.Result as OkObjectResult)!).Value);
        }
    }
}