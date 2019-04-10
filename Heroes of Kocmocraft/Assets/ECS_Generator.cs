using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public class ECS_Generator : MonoBehaviour
{
    public static ECS_Generator GM;

    public static EntityArchetype BlockArchetype;

    public OceanGenerator[] generator;

    void Awake()
    {
        if (GM != null && GM != this)
            Destroy(gameObject);
        else
            GM = this;
    }

    public EntityManager manager;
    public Entity entities;

    //現在可以在函式前面加上屬性，讓他可以在特定條件前後執行，比如這個Initialize就會在場景載入之前執行
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        //呼叫一個EntityManager管理器，如果沒有就建立一個新的
        EntityManager manager = World.Active.GetOrCreateManager<EntityManager>();

        //建立一個Archetype結構的區塊，這可以確保裡面的內容在記憶體內都緊密的結合一起
        BlockArchetype = manager.CreateArchetype(
            //typeof(TransformMatrix),
            typeof(Position)
        //typeof(ColliderChecker)
        );
    }

    private void Start()
    {
        for (int i = 0; i < generator.Length; i++)
        {
            generator[i].Generate();
        }
    }
}
