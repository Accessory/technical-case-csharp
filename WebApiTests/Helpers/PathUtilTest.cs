using System.Reflection;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using WebApi.Helpers;
using WebApi.Models.Jobs;

namespace WebApiTests.Helpers;

[TestClass]
[TestSubject(typeof(PathUtil))]
public class PathUtilTest
{
    private static string ReadEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    [TestMethod]
    public void CreateJobFromEnterPathRequestTest()
    {
        EnterPathRequest request = new EnterPathRequest(new List<Command>()
        {
            new Command() { Direction = "north", Steps = 100_000 },
            new Command() { Direction = "east", Steps = 100_000 },
            new Command() { Direction = "south", Steps = 100_000 },
            new Command() { Direction = "south", Steps = 100_000 },
            new Command() { Direction = "west", Steps = 100_000 },
            new Command() { Direction = "west", Steps = 100_000 },
            new Command() { Direction = "north", Steps = 100_000 },
            new Command() { Direction = "north", Steps = 100_000 },
        })
        {
            Start = new Position() { X = 0, Y = 0 }
        };

        var job = PathUtil.CreateJobFromEnterPathRequest(request);
        var job2 = PathUtil.CreateJobFromEnterPathRequest2(request);
        var job3 = PathUtil.CreateJobFromEnterPathRequest3(request);
        var job4 = PathUtil.CreateJobFromEnterPathRequest4(request);
        var job5 = PathUtil.CreateJobFromEnterPathRequest4(request);
        var job6 = PathUtil.CreateJobFromEnterPathRequest4(request);
        Assert.AreEqual(800001, job.Result);
        Assert.AreEqual(800001, job2.Result);
        Assert.AreEqual(800001, job3.Result);
        Assert.AreEqual(800001, job4.Result);
        Assert.AreEqual(800001, job5.Result);
        Assert.AreEqual(800001, job6.Result);
    }

    [TestMethod]
    public void CreateJobFromEnterPathRequest2Test()
    {
        EnterPathRequest request = new EnterPathRequest(new List<Command>()
        {
            new Command() { Direction = "north", Steps = 100_000 },
            new Command() { Direction = "south", Steps = 100_000 },
        })
        {
            Start = new Position() { X = 0, Y = 0 }
        };

        var job = PathUtil.CreateJobFromEnterPathRequest(request);
        var job2 = PathUtil.CreateJobFromEnterPathRequest2(request);
        var job3 = PathUtil.CreateJobFromEnterPathRequest3(request);
        var job4 = PathUtil.CreateJobFromEnterPathRequest4(request);
        var job5 = PathUtil.CreateJobFromEnterPathRequest5(request);
        var job6 = PathUtil.CreateJobFromEnterPathRequest6(request);
        Assert.AreEqual(100001, job.Result);
        Assert.AreEqual(100001, job2.Result);
        Assert.AreEqual(100001, job3.Result);
        Assert.AreEqual(100001, job4.Result);
        Assert.AreEqual(200000, job5.Result);
        Assert.AreEqual(100001, job6.Result);
    }

    [TestMethod]
    public void CreateJobFromEnterPathRequest3Test()
    {
        EnterPathRequest request = new EnterPathRequest(new List<Command>()
        {
            new Command() { Direction = "north", Steps = 10 },
            new Command() { Direction = "east", Steps = 10 },
            new Command() { Direction = "south", Steps = 10 },
            new Command() { Direction = "west", Steps = 10 },
            new Command() { Direction = "north", Steps = 10 },
        })
        {
            Start = new Position() { X = 0, Y = 0 }
        };

        var job = PathUtil.CreateJobFromEnterPathRequest(request);
        var job2 = PathUtil.CreateJobFromEnterPathRequest2(request);
        var job3 = PathUtil.CreateJobFromEnterPathRequest3(request);
        var job4 = PathUtil.CreateJobFromEnterPathRequest4(request);
        var job5 = PathUtil.CreateJobFromEnterPathRequest5(request);
        var job6 = PathUtil.CreateJobFromEnterPathRequest6(request);
        Assert.AreEqual(40, job.Result);
        Assert.AreEqual(40, job2.Result);
        Assert.AreEqual(49, job3.Result);
        Assert.AreEqual(41, job4.Result);
        Assert.AreEqual(50, job5.Result);
        Assert.AreEqual(35, job6.Result);
    }

    [TestMethod]
    public void CreateJobFromEnterPathRequest4Test()
    {
        EnterPathRequest request = new EnterPathRequest(new List<Command>()
        {
            new Command() { Direction = "east", Steps = 2 },
            new Command() { Direction = "north", Steps = 1 },
        })
        {
            Start = new Position() { X = 0, Y = 0 }
        };

        var job = PathUtil.CreateJobFromEnterPathRequest(request);
        var job2 = PathUtil.CreateJobFromEnterPathRequest2(request);
        var job3 = PathUtil.CreateJobFromEnterPathRequest3(request);
        var job4 = PathUtil.CreateJobFromEnterPathRequest4(request);
        var job5 = PathUtil.CreateJobFromEnterPathRequest5(request);
        var job6 = PathUtil.CreateJobFromEnterPathRequest6(request);
        Assert.AreEqual(4, job.Result);
        Assert.AreEqual(4, job2.Result);
        Assert.AreEqual(4, job3.Result);
        Assert.AreEqual(4, job4.Result);
        Assert.AreEqual(4, job5.Result);
        Assert.AreEqual(4, job6.Result);
    }

    [TestMethod]
    public void CreateJobFromEnterPathRequest5Test()
    {
        EnterPathRequest request = new EnterPathRequest(new List<Command>()
        {
            new Command() { Direction = "north", Steps = 10 },
            new Command() { Direction = "east", Steps = 5 },
            new Command() { Direction = "south", Steps = 5 },
            new Command() { Direction = "west", Steps = 10 },
            new Command() { Direction = "north", Steps = 2 },
            new Command() { Direction = "east", Steps = 8 },
            new Command() { Direction = "south", Steps = 8 },
        })
        {
            Start = new Position() { X = 15, Y = 15 }
        };

        var job = PathUtil.CreateJobFromEnterPathRequest(request);
        var job2 = PathUtil.CreateJobFromEnterPathRequest2(request);
        var job3 = PathUtil.CreateJobFromEnterPathRequest3(request);
        var job4 = PathUtil.CreateJobFromEnterPathRequest4(request);
        var job5 = PathUtil.CreateJobFromEnterPathRequest5(request);
        var job6 = PathUtil.CreateJobFromEnterPathRequest6(request);
        Assert.AreEqual(46, job.Result);
        Assert.AreEqual(46, job2.Result);
        Assert.AreEqual(46, job3.Result);
        Assert.AreEqual(46, job4.Result);
        Assert.AreEqual(46, job5.Result);
        Assert.AreEqual(39, job6.Result);
    }

    [TestMethod]
    public void CreateJobFromEnterPathRequestTest2()
    {
        string contents = ReadEmbeddedResource("WebApiTests.TestData.robotcleanerpathheavy.json");
        EnterPathRequest request = JsonConvert.DeserializeObject<EnterPathRequest>(contents)!;
        var job = PathUtil.CreateJobFromEnterPathRequest(request);
        Assert.AreEqual(993737501, job.Result);
    }

    [TestMethod]
    public void CreateJobFromEnterPathRequestTest3()
    {
        string contents = ReadEmbeddedResource("WebApiTests.TestData.robotcleanerpathheavy.json");
        EnterPathRequest request = JsonConvert.DeserializeObject<EnterPathRequest>(contents)!;
        var job = PathUtil.CreateJobFromEnterPathRequest3(request);
        Assert.AreEqual(993737501, job.Result);
    }

    [TestMethod]
    public void CreateJobFromEnterPathRequestTest4()
    {
        string contents = ReadEmbeddedResource("WebApiTests.TestData.robotcleanerpathheavy.json");
        EnterPathRequest request = JsonConvert.DeserializeObject<EnterPathRequest>(contents)!;
        var job = PathUtil.CreateJobFromEnterPathRequest4(request);
        Assert.AreEqual(993737501, job.Result);
    }

    [TestMethod]
    public void CreateJobFromEnterPathRequestTest5()
    {
        string contents = ReadEmbeddedResource("WebApiTests.TestData.robotcleanerpathheavy.json");
        EnterPathRequest request = JsonConvert.DeserializeObject<EnterPathRequest>(contents)!;
        var job = PathUtil.CreateJobFromEnterPathRequest5(request);
        Assert.AreEqual(993737501, job.Result);
    }

    // [TestMethod]
    // public void CreateJobFromEnterPathRequestTest6()
    // {
    //     string contents = ReadEmbeddedResource("WebApiTests.TestData.robotcleanerpathheavy.json");
    //     EnterPathRequest request = JsonConvert.DeserializeObject<EnterPathRequest>(contents)!;
    //     var job = PathUtil.CreateJobFromEnterPathRequest6(request);
    //     Assert.AreEqual(993737501, job.Result);
    // }
}