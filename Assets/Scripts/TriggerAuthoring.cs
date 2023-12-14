using UnityEngine;
using Unity.Entities;

public class TriggerAuthoring : MonoBehaviour
{
    public float size;

    public class TriggerBaker : Baker<TriggerAuthoring>
    {
        public override void Bake(TriggerAuthoring authoring)
        {
            Entity triggerAuthoring = GetEntity(TransformUsageFlags.None);

            AddComponent(triggerAuthoring, new TriggerComponent { size = authoring.size });
        }
    }
}
