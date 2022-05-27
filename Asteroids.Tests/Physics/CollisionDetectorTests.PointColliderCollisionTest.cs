using System.Collections.Generic;
using System.Numerics;
using Asteroids.Physics;
using Xunit;

namespace Asteroids.Tests.Physics;

public partial class CollisionDetectorTests
{
    [Theory]
    [MemberData(nameof(CorrectPointColliderCollisionTestData))]
    public void ShouldPassPointColliderCollisionTest(Collider collider, Vector2 point)
    {
        bool result = CollisionDetector.PointColliderCollisionTest(collider, point);

        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IncorrectPointColliderCollisionTestData))]
    public void ShouldNotPassPointColliderCollisionTest(Collider collider, Vector2 point)
    {
        bool result = CollisionDetector.PointColliderCollisionTest(collider, point);

        Assert.False(result);
    }

    public static IEnumerable<object[]> CorrectPointColliderCollisionTestData()
    {
        Collider rightCollider = new Collider(
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 1.0f)
        );

        Collider obtuseCollider = new Collider(
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(-0.25f, 1.0f)
        );

        Collider acuteCollider = new Collider(
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 0.5f)
        );

        // point inside collider
        yield return new object[] { rightCollider, new Vector2(0.1f, 0.1f) };
        yield return new object[] { obtuseCollider, new Vector2(0.0f, 0.2f) };
        yield return new object[] { acuteCollider, new Vector2(0.5f, 0.1f) };

        // point is collider vertex
        yield return new object[] { rightCollider, new Vector2(0.0f, 0.0f) };
        yield return new object[] { rightCollider, new Vector2(1.0f, 0.0f) };
        yield return new object[] { rightCollider, new Vector2(0.0f, 1.0f) };

        // point on collider edge
        yield return new object[] { rightCollider, new Vector2(0.0f, 0.5f) };
        yield return new object[] { rightCollider, new Vector2(0.0f, 0.25f) };
        yield return new object[] { rightCollider, new Vector2(0.0f, 0.75f) };
        yield return new object[] { rightCollider, new Vector2(0.5f, 0.0f) };
        yield return new object[] { rightCollider, new Vector2(0.25f, 0.0f) };
        yield return new object[] { rightCollider, new Vector2(0.75f, 0.0f) };
        yield return new object[] { rightCollider, new Vector2(0.5f, 0.5f) };
        yield return new object[] { rightCollider, new Vector2(0.25f, 0.75f) };
        yield return new object[] { rightCollider, new Vector2(0.75f, 0.25f) };
    }

    public static IEnumerable<object[]> IncorrectPointColliderCollisionTestData()
    {
        Collider rightCollider = new Collider(
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 1.0f)
        );

        Collider obtuseCollider = new Collider(
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(-0.25f, 1.0f)
        );

        Collider acuteCollider = new Collider(
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 0.5f)
        );

        // point outside collider
        yield return new object[] { rightCollider, new Vector2(1.0f, 1.0f) };
        yield return new object[] { obtuseCollider, new Vector2(1.0f, 1.0f) };
        yield return new object[] { acuteCollider, new Vector2(1.0f, 1.0f) };
    }
}