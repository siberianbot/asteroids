using System.Collections.Generic;
using System.Numerics;
using Asteroids.Physics;
using Xunit;

namespace Asteroids.Tests.Physics;

public partial class CollisionDetectorTests
{
    [Theory]
    [MemberData(nameof(CorrectLineSegmentCollisionTestData))]
    public void ShouldPassLineSegmentCollisionTest(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        bool result = CollisionDetector.LineSegmentCollisionTest(a1, a2, b1, b2);

        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IncorrectLineSegmentCollisionTestData))]
    public void ShouldNotPassLineSegmentCollisionTest(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        bool result = CollisionDetector.LineSegmentCollisionTest(a1, a2, b1, b2);

        Assert.False(result);
    }

    public static IEnumerable<object[]> CorrectLineSegmentCollisionTestData()
    {
        // crossing (different angles)
        yield return new object[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f)
        };
        yield return new object[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(2.0f, 1.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(2.0f, 0.0f)
        };
        yield return new object[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.1f, 1.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(0.1f, 0.0f)
        };

        // point on segment
        yield return new object[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(0.0f, 0.5f),
            new Vector2(1.0f, 0.5f)
        };

        // same point
        yield return new object[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f)
        };
        yield return new object[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 1.0f)
        };

        // on same line
        yield return new object[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(-0.5f, -0.5f),
            new Vector2(0.5f, 0.5f)
        };
        yield return new object[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.25f, 0.25f),
            new Vector2(0.75f, 0.75f)
        };
        yield return new object[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
        };
    }

    public static IEnumerable<object[]> IncorrectLineSegmentCollisionTestData()
    {
        // no intersection
        yield return new object[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.5f, 0.4f),
            new Vector2(1.0f, 0.0f)
        };

        // collinear, but no intersection 
        yield return new object[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.01f, 0.01f),
            new Vector2(1.01f, 1.01f)
        };

        // on same line, but no intersection
        yield return new object[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.01f, 1.01f),
            new Vector2(2.01f, 2.01f)
        };
    }
}