using Unity.Entities;
using UnityEngine;

public class BallAuthoring : MonoBehaviour
{
    public float MoveSpeed = 10;

    public class BallAuthoringBaker : Baker<BallAuthoring>
    {
        public override void Bake(BallAuthoring authoring)
        {
            Entity ballAuthoring = GetEntity(TransformUsageFlags.None);

            AddComponent(ballAuthoring, new BallComponent { MoveSpeed = authoring.MoveSpeed });
        }
    }
}