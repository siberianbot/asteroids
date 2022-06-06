using System.Collections.Generic;
using System.Numerics;
using Asteroids.Physics;
using Xunit;

namespace Asteroids.Tests.Physics;

public partial class CollisionDetectorTests
{
    [Theory]
    [MemberData(nameof(CorrectCirclesCollisionTestData))]
    public void ShouldPassCirclesCollisionTest(Vector2 leftPos, float leftRadius, Vector2 rightPos, float rightRadius)
    {
        bool result = CollisionDetector.CirclesCollisionTest(leftPos, leftRadius, rightPos, rightRadius);

        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IncorrectCirclesCollisionTestData))]
    public void ShouldNotPassCirclesCollisionTest(Vector2 leftPos, float leftRadius, Vector2 rightPos, float rightRadius)
    {
        bool result = CollisionDetector.CirclesCollisionTest(leftPos, leftRadius, rightPos, rightRadius);

        Assert.False(result);
    }

    public static IEnumerable<object[]> CorrectCirclesCollisionTestData()
    {
        yield return new object[] { new Vector2(0, 0), 1f, new Vector2(1, 0), 1f };
        yield return new object[] { new Vector2(0, 0), 1f, new Vector2(2, 0), 1f };
        yield return new object[] { new Vector2(0, 0), 1f, new Vector2(0, 0), 1f };
    }

    public static IEnumerable<object[]> IncorrectCirclesCollisionTestData()
    {
        yield return new object[] { new Vector2(0, 0), 1f, new Vector2(2, 2), 1f };
    }
}