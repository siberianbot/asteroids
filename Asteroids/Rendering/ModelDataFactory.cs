using System.Numerics;
using Asteroids.Components;
using Asteroids.Engine;
using Asteroids.Physics;
using Asteroids.Utils;

namespace Asteroids.Rendering;

public class ModelDataFactory
{
    private readonly Vars _vars;

    public ModelDataFactory(Vars vars)
    {
        _vars = vars;
    }

    public IEnumerable<ModelData> CreateFrom(
        ModelComponent modelComponent,
        PositionComponent positionComponent,
        ColliderComponent? colliderComponent)
    {
        Matrix4x4 transformMatrix = positionComponent.TransformMatrix;

        if (_vars.GetVar(Constants.Vars.PhysicsShowActivationRadius, false) && colliderComponent != null)
        {
            yield return CreateForActivationRadius(colliderComponent.Radius, transformMatrix);
        }

        if (_vars.GetVar(Constants.Vars.PhysicsShowCollider, false) && colliderComponent != null)
        {
            foreach (Collider collider in colliderComponent.Colliders)
            {
                yield return CreateForCollider(collider, transformMatrix);
            }
        }

        yield return CreateForModelComponent(modelComponent, transformMatrix);
    }

    private static ModelData CreateForActivationRadius(float radius, Matrix4x4 transformMatrix)
    {
        return new ModelData(
            GenerationUtils.GenerateCircleVerticesData(radius, 48).ToArray(),
            GenerationUtils.GenerateIndicesData(48).ToArray(),
            48,
            Constants.Colors.DarkGray,
            transformMatrix
        );
    }

    private static ModelData CreateForCollider(Collider collider, Matrix4x4 transformMatrix)
    {
        return new ModelData(
            Collider.DataOf(collider).ToArray(),
            GenerationUtils.GenerateIndicesData(3).ToArray(),
            3,
            Constants.Colors.DarkGray,
            transformMatrix
        );
    }

    private static ModelData CreateForModelComponent(ModelComponent modelComponent, Matrix4x4 transformMatrix)
    {
        return new ModelData(
            modelComponent.VerticesData,
            modelComponent.IndicesData,
            modelComponent.Count,
            modelComponent.Color,
            transformMatrix
        );
    }
}