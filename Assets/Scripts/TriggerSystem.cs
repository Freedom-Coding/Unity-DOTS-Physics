using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics;
using Unity.Transforms;

public partial struct TriggerSystem : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;

        NativeArray<Entity> entities = entityManager.GetAllEntities(Allocator.Temp);

        foreach (Entity entity in entities)
        {
            if (entityManager.HasComponent<TriggerComponent>(entity))
            {
                RefRW<LocalToWorld> triggerTransform = SystemAPI.GetComponentRW<LocalToWorld>(entity);
                RefRO<TriggerComponent> triggerComponent = SystemAPI.GetComponentRO<TriggerComponent>(entity);

                float size = triggerComponent.ValueRO.size;
                triggerTransform.ValueRW.Value.c0 = new float4(size, 0, 0, 0);
                triggerTransform.ValueRW.Value.c1 = new float4(0, size, 0, 0);
                triggerTransform.ValueRW.Value.c2 = new float4(0, 0, size, 0);

                PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

                NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);

                physicsWorld.SphereCastAll(triggerTransform.ValueRO.Position, triggerComponent.ValueRO.size / 2,
                    float3.zero, 1, ref hits, new CollisionFilter 
                    { BelongsTo = (uint)CollisionLayer.Player, CollidesWith = (uint)CollisionLayer.Collectible });

                foreach (ColliderCastHit hit in hits)
                {
                    entityManager.DestroyEntity(hit.Entity);
                }

                hits.Dispose();
            }
        }

        entities.Dispose();
    }
}

public enum CollisionLayer
{
    Player = 1 << 6,
    Collectible = 1 << 7
}