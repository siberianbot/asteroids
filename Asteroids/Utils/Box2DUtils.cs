using System.Numerics;
using Silk.NET.Maths;

namespace Asteroids.Utils;

public static class Box2DUtils
{
    public static Box2D<float> Union(Box2D<float> left, Box2D<float> right)
    {
        return new Box2D<float>(
            left.Min.X < right.Min.X ? left.Min.X : right.Min.X,
            left.Min.Y < right.Min.Y ? left.Min.Y : right.Min.Y,
            left.Max.X < right.Max.X ? left.Max.X : right.Max.X,
            left.Max.Y < right.Max.Y ? left.Max.Y : right.Max.Y
        );
    }

    public static Box2D<float> FromVerticesCloud(IEnumerable<Vector2> vertices)
    {
        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);

        foreach (Vector2 vertex in vertices)
        {
            if (vertex.X < min.X)
            {
                min.X = vertex.X;
            }

            if (vertex.Y < min.Y)
            {
                min.Y = vertex.Y;
            }

            if (vertex.X > max.X)
            {
                max.X = vertex.X;
            }

            if (vertex.Y > max.Y)
            {
                max.Y = vertex.Y;
            }
        }

        return new Box2D<float>(min.X, min.Y, max.X, max.Y);
    }
}