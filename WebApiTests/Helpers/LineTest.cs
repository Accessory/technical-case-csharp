using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Helpers;
using WebApi.Models.Jobs;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace WebApiTests.Helpers;

[TestClass]
[TestSubject(typeof(Line))]
public class LineTest
{
    [Fact]
    public void TestIntersectionsHorizontal1()
    {
        var leftZero = new Position(){X = -5, Y = 0};
        var rightZero = new Position(){X = 5, Y = 0};
        var rightBottom = new Position(){X = -5, Y = -5};
        var leftBottom = new Position(){X = 5, Y = -5};

        var l1 = new Line(leftZero, rightZero);
        var l2 = new Line(leftBottom, rightBottom);
        var result = l1.Intersections(l2);
        Assert.AreEqual(0, result);
    }
    
    [Fact]
    public void TestIntersectionsHorizontal2()
    {
        var leftZero = new Position(){X = -5, Y = 0};
        var rightZero = new Position(){X = 5, Y = 0};

        var l1 = new Line(leftZero, rightZero);
        var l2 = new Line(leftZero, rightZero);
        var result = l1.Intersections(l2);
        Assert.AreEqual(11, result);
    }
    
    [Fact]
    public void TestIntersectionsHorizontal3()
    {
        var leftZero = new Position(){X = -5, Y = 0};
        var rightZero = new Position(){X = 5, Y = 0};
        var right2Zero = new Position(){X = 10, Y = 0};

        var l1 = new Line(leftZero, rightZero);
        var l2 = new Line(leftZero, right2Zero);
        var result = l1.Intersections(l2);
        Assert.AreEqual(11, result);
    }
    
    [Fact]
    public void TestIntersectionsHorizontal4()
    {
        var leftZero = new Position(){X = -5, Y = 0};
        var rightZero = new Position(){X = 5, Y = 0};
        var rightBottom = new Position(){X = 6, Y = 0};
        var leftBottom = new Position(){X = 15, Y = 0};

        var l1 = new Line(leftZero, rightZero);
        var l2 = new Line(leftBottom, rightBottom);
        var result = l1.Intersections(l2);
        Assert.AreEqual(0, result);
    }
    
    [Fact]
    public void TestIntersectionsHorizontal5()
    {
        var leftZero = new Position(){X = -5, Y = 0};
        var rightZero = new Position(){X = 5, Y = 0};
        var rightBottom = new Position(){X = 4, Y = 0};
        var leftBottom = new Position(){X = 15, Y = 0};

        var l1 = new Line(leftZero, rightZero);
        var l2 = new Line(leftBottom, rightBottom);
        var result = l1.Intersections(l2);
        Assert.AreEqual(2, result);
    }
    
    [Fact]
    public void TestIntersectionsVertical1()
    {
        var topZero = new Position(){X = 0, Y = -5};
        var bottomZero = new Position(){X = 0, Y = 5};
        var topOne = new Position(){X = 1, Y = -5};
        var bottomOne = new Position(){X = 1, Y = 5};

        var l1 = new Line(topZero, bottomZero);
        var l2 = new Line(bottomOne, topOne);
        var result = l1.Intersections(l2);
        Assert.AreEqual(0, result);
    }
    
    [Fact]
    public void TestIntersectionsVertical2()
    {
        var topZero = new Position(){X = 0, Y = -5};
        var bottomZero = new Position(){X = 0, Y = 5};

        var l1 = new Line(topZero, bottomZero);
        var l2 = new Line(bottomZero, topZero);
        var result = l1.Intersections(l2);
        Assert.AreEqual(11, result);
    }
    
    [Fact]
    public void TestIntersectionsVertical3()
    {
        var topZero = new Position(){X = 0, Y = -5};
        var bottomZero = new Position(){X = 0, Y = 5};
        var bottom2Zero = new Position(){X = 0, Y = 10};

        var l1 = new Line(topZero, bottomZero);
        var l2 = new Line(bottomZero, bottom2Zero);
        var result = l1.Intersections(l2);
        Assert.AreEqual(1, result);
    }
    
    [Fact]
    public void TestIntersectionsVertical4()
    {
        var topZero = new Position(){X = 0, Y = -5};
        var bottomZero = new Position(){X = 0, Y = 5};

        var l1 = new Line(topZero, bottomZero);
        var l2 = new Line(topZero, bottomZero);
        var result = l1.Intersections(l2);
        Assert.AreEqual(11, result);
    }
    
    [Fact]
    public void TestIntersectionsVertical5()
    {
        var topZero = new Position(){X = 0, Y = -5};
        var bottomZero = new Position(){X = 0, Y = 5};
        var topOne = new Position(){X = 0, Y = 6};
        var bottomOne = new Position(){X = 0, Y = 165};

        var l1 = new Line(topZero, bottomZero);
        var l2 = new Line(bottomOne, topOne);
        var result = l1.Intersections(l2);
        Assert.AreEqual(0, result);
    }
    
    [Fact]
    public void LineCase()
    {
        var p1 = new Position(){X = 10000, Y = 10000};
        var p2 = new Position(){X = 10000, Y = 1};
        var p3 = new Position(){X = 10000, Y = 0};
        var p4 = new Position(){X = 10000, Y = -99999};

        var l1 = new Line(p1, p2);
        var l2 = new Line(p3, p4);
        var result = l1.Intersections(l2);
        Assert.AreEqual(0, result);
    }
    
    [Fact]
    public void IntersectionCase()
    {
        var p1 = new Position(){X = -10, Y = 0};
        var p2 = new Position(){X = 10, Y = 0};
        var p3 = new Position(){X = 0, Y = -10};
        var p4 = new Position(){X = 0, Y = 10};

        var l1 = new Line(p1, p2);
        var l2 = new Line(p3, p4);
        var result = l1.Intersections(l2);
        Assert.AreEqual(1, result);
    }
    
    [Fact]
    public void IntersectionCase2()
    {
        var p1 = new Position(){X = -4, Y = 10};
        var p2 = new Position(){X = 0, Y = 10};
        var p3 = new Position(){X = -5, Y = 15};
        var p4 = new Position(){X = -5, Y = 5};

        var l1 = new Line(p1, p2);
        var l2 = new Line(p3, p4);
        var result = l1.Intersections(l2);
        Assert.AreEqual(0, result);
    }
    
    [Fact]
    public void IntersectionCase3()
    {
        var p1 = new Position(){X = 15, Y = 15};
        var p2 = new Position(){X = 15, Y = 24};
        var p3 = new Position(){X = 15, Y = 25};
        var p4 = new Position(){X = 19, Y = 25};

        var l1 = new Line(p1, p2);
        var l2 = new Line(p3, p4);
        var result = l1.Intersections(l2);
        Assert.AreEqual(0, result);
    }
    
    [Fact]
    public void IntersectionCase4()
    {
        var p1 = new Position(){X = 20, Y = 25};
        var p2 = new Position(){X = 20, Y = 21};
        var p3 = new Position(){X = 10, Y = 22};
        var p4 = new Position(){X = 17, Y = 22};

        var l1 = new Line(p1, p2);
        var l2 = new Line(p3, p4);
        var result = l1.Intersections(l2);
        Assert.AreEqual(0, result);
    }
    
}