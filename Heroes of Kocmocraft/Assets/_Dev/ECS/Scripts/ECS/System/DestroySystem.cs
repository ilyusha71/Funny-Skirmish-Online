using Unity.Collections;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Minecraft
{
    public class DestroySystem : ComponentSystem
    {
        //系統需要撈出所有帶有Position和BlockTag資料的Entity
        struct BlockGroup
        {
            [ReadOnly] public readonly int Length;
            [ReadOnly] public EntityArray entity;
            [ReadOnly] public ComponentDataArray<Position> positions;
            [ReadOnly] public ComponentDataArray<BlockTag> tags;
        }

        //系統需要撈出射線打到後該位置的Entity
        struct DestoryBlockGroup
        {
            [ReadOnly] public readonly int Length;
            [ReadOnly] public EntityArray entity;
            [ReadOnly]public ComponentDataArray<Position> positions;
            [ReadOnly] public ComponentDataArray<DestroyTag> tags;
        }

        //系統需要撈出所有地表物件的座標，用來清除消失方塊的地表物件
        struct SurfacePlantGroup
        {
            [ReadOnly] public readonly int Length;
            [ReadOnly] public EntityArray entity;
            [ReadOnly] public ComponentDataArray<Position> positions;
            [ReadOnly] public ComponentDataArray<SurfacePlantTag> tags;
        }
        [Inject] BlockGroup targetBlocks;
        [Inject] DestoryBlockGroup sourceBlock;
        [Inject] SurfacePlantGroup surfaceplants;

        protected override void OnUpdate()
        {
            for (int i = 0; i < sourceBlock.Length; i++)
            {
                for (int j = 0; j < targetBlocks.Length; j++)
                {
                    Vector3 offset = targetBlocks.positions[j].Value- sourceBlock.positions[i].Value;
                    float sqrLen = offset.sqrMagnitude;

                    //找到同座標的方塊並刪除
                    if (sqrLen == 0)
                   {
                        //如果它的上面有帶有SufacePlantTag的物件，也一併刪除;
                        for (int k = 0; k < surfaceplants.Length;k++)
                        {
                            float3 tmpPos = new float3(surfaceplants.positions[k].Value.x, surfaceplants.positions[k].Value.y+Vector3.down.y, surfaceplants.positions[k].Value.z);
                            offset = targetBlocks.positions[j].Value - tmpPos;
                            sqrLen = offset.sqrMagnitude;

                            if (sqrLen == 0)
                            {
                                PostUpdateCommands.DestroyEntity(surfaceplants.entity[k]);
                            }
                        }

                        //用PostUpdateCommands安全的移除方塊
                        PostUpdateCommands.DestroyEntity(sourceBlock.entity[i]);
                        PostUpdateCommands.DestroyEntity(targetBlocks.entity[j]);
                    }
                }
            }
        }
    }
}
