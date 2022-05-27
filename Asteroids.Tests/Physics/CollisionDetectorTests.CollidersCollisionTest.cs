using System.Collections.Generic;
using System.Numerics;
using Asteroids.Physics;
using Xunit;

namespace Asteroids.Tests.Physics;

public partial class CollisionDetectorTests
{
    [Theory]
    [MemberData(nameof(CorrectCollidersCollisionTestData))]
    public void ShouldPassCollidersCollisionTest(Collider a, Collider b)
    {
        bool result = CollisionDetector.CollidersCollisionTest(a, b);

        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IncorrectCollidersCollisionTestData))]
    public void ShouldNotPassCollidersCollisionTest(Collider a, Collider b)
    {
        bool result = CollisionDetector.CollidersCollisionTest(a, b);

        Assert.False(result);
    }

    public static IEnumerable<object[]> CorrectCollidersCollisionTestData()
    {
        // same vertex
        yield return new object[]
        {
            new Collider(
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(0.0f, 1.0f)
            ),
            new Collider(
                new Vector2(1.0f, 0.0f),
                new Vector2(2.0f, 0.0f),
                new Vector2(2.0f, 1.0f)
            )
        };

        // same edge
        yield return new object[]
        {
            new Collider(
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(0.0f, 1.0f)
            ),
            new Collider(
                new Vector2(-1.0f, 0.0f),
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f)
            )
        };
        yield return new object[]
        {
            new Collider(
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(0.0f, 1.0f)
            ),
            new Collider(
                new Vector2(1.0f, 0.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(0.0f, 1.0f)
            )
        };

        // vertex inside collider
        yield return new object[]
        {
            new Collider(
                new Vector2(0.0f, 0.5f),
                new Vector2(1.0f, 0.0f),
                new Vector2(1.0f, 1.0f)
            ),
            new Collider(
                new Vector2(0.5f, 0.75f),
                new Vector2(1.5f, 0.0f),
                new Vector2(1.5f, 1.0f)
            )
        };

        // edges intersection
        yield return new object[]
        {
            new Collider(
                new Vector2(0.0f, 0.5f),
                new Vector2(1.0f, 0.0f),
                new Vector2(1.0f, 1.0f)
            ),
            new Collider(
                new Vector2(0.5f, 0.0f),
                new Vector2(0.5f, 1.0f),
                new Vector2(1.5f, 0.5f)
            )
        };

        // collider inside collider
        yield return new object[]
        {
            new Collider(
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(0.0f, 1.0f)
            ),
            new Collider(
                new Vector2(0.1f, 0.1f),
                new Vector2(0.1f, 0.9f),
                new Vector2(0.9f, 0.1f)
            )
        };
    }

    public static IEnumerable<object[]> IncorrectCollidersCollisionTestData()
    {
        yield return new object[]
        {
            new Collider(
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(0.0f, 1.0f)
            ),
            new Collider(
                new Vector2(2.0f, 2.0f),
                new Vector2(2.0f, 0.0f),
                new Vector2(1.0f, 2.0f)
            )
        };
    }
}