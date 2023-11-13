using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using WebApi.Entities;
using WebApi.Models.Jobs;
using Xunit;
using Assert = Xunit.Assert;


namespace WebApiTests
{
    [TestClass()]
    public class IntegrationTest : IClassFixture<CustomFactory>
    {
        private readonly CustomFactory _customFactory;

        public IntegrationTest(CustomFactory customFactory)
        {
            _customFactory = customFactory;
        }

        [Fact]
        public async Task HalloWeltTest()
        {
            var client = _customFactory.CreateClient();
            var result = await client.GetAsync("/tibber-developer-test/hallo-welt");
            Assert.Equal("Hallo Welt", await result.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task EnterPathTest()
        {
            var command1 = new Command() { Steps = 5, Direction = "north" };
            var commands = new List<Command>() { command1 };
            var enterPath = new EnterPathRequest(commands);

            var client = _customFactory.CreateClient();
            var body = new StringContent(JsonConvert.SerializeObject(enterPath), Encoding.UTF8, "application/json");
            var result = await client.PostAsync("/tibber-developer-test/enter-path", body);
            var resultString = await result.Content.ReadAsStringAsync();
            var resultJob = JsonConvert.DeserializeObject<Job>(resultString);
            Assert.NotNull(resultJob);
            Assert.Equal(1, resultJob.Id);
            Assert.Equal(6, resultJob.Result);
            Assert.Equal(1, resultJob.Commands);
        }
    }
}