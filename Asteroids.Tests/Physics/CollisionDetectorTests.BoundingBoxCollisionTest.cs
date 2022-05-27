using Asteroids.Physics;
using Silk.NET.Maths;
using Xunit;

namespace Asteroids.Tests.Physics;

public partial class CollisionDetectorTests
{
    [Theory]
    [InlineData(0.0f, 0.0f, 1.0f, 1.0f, 0.25f, 0.25f, 0.75f, 0.75f)] // box inside box
    [InlineData(0.0f, 0.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.5f, 1.5f)] // box overlap box
    [InlineData(0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 2.0f)] // vertex overlap
    [InlineData(0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f, 2.0f, 1.0f)] // edge overlap
    public void ShouldPassBoundingBoxCollisionTest(
        float ax1, float ay1, float ax2, float ay2,
        float bx1, float by1, float bx2, float by2
    )
    {
        Box2D<float> a = new Box2D<float>(ax1, ay1, ax2, ay2);
        Box2D<float> b = new Box2D<float>(bx1, by1, bx2, by2);

        bool result = CollisionDetector.BoundingBoxCollisionTest(a, b);

        Assert.True(result);
    }

    [Theory]
    [InlineData(0.0f, 0.0f, 1.0f, 1.0f, 2.0f, 2.0f, 3.0f, 3.0f)]
    [InlineData(0.0f, 0.0f, 1.0f, 1.0f, 1.0000001f, 0.0f, 2.0f, 2.0f)]
    public void ShouldNotPassBoundingBoxCollisionTest(
        float ax1, float ay1, float ax2, float ay2,
        float bx1, float by1, float bx2, float by2
    )
    {
        Box2D<float> a = new Box2D<float>(ax1, ay1, ax2, ay2);
        Box2D<float> b = new Box2D<float>(bx1, by1, bx2, by2);

        bool result = CollisionDetector.BoundingBoxCollisionTest(a, b);

        Assert.False(result);
    }
}