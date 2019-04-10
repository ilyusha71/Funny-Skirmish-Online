using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class OceanGenerator : MonoBehaviour
{
    //基礎的模型一般方塊，地表方塊和地面植物
    [Header("Mesh Info")]
    public Mesh oceanMesh;

    //一般方塊可能會用到的材質貼圖 - 自然類貼圖由上到下為(石頭、木頭、樹葉、表面、鵝軟石、泥土、草、花、雲)
    [Header("Nature Block Type")]
    public Material oceanMaterial;

    //當沒有找到材質，使用這個粉紅材質
    [Header("")]
    public Material pinkMaterial;

    Material maTemp;
    Mesh meshTemp;

    public float3 scale = new float3(5.0f, 1.0f, 5.0f);
    public float3 size= new float3(10000.0f, 0.5f, 10000.0f);
    public Vector3 offset = new Vector3(0, -2.1f, 0);
    public bool alignCenter =true;
    Vector3[] baseVertices;

    public void Generate()
    {
        ECS_Generator.GM.manager = World.Active.GetOrCreateManager<EntityManager>();

        meshTemp = new Mesh
        {
            vertices = oceanMesh.vertices,
            triangles = oceanMesh.triangles,
            normals = oceanMesh.normals,
            uv = oceanMesh.uv
        };

        float3 meshSize = meshTemp.bounds.size * scale;
        Debug.Log(meshSize);
        int3 amount = (int3)(size / meshSize); //new int3((int)(sizeX / meshSize.x), 1, (int)(sizeZ / meshSize.z));
        amount.y = 1;
        Debug.Log(amount);
        float3 totalHalfSize = meshSize * amount * 0.5f;
        Debug.Log(totalHalfSize);
        if (alignCenter)
            offset = offset - (Vector3)totalHalfSize;

        if (baseVertices == null)
            baseVertices = meshTemp.vertices;

        var vertices = new Vector3[baseVertices.Length];

        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = baseVertices[i];
            vertex.x *= scale.x;
            vertex.y *= scale.y;
            vertex.z *= scale.z;
            vertices[i] = vertex;
        }

        meshTemp.vertices = vertices;

        for (int yBlock = 0; yBlock < amount.y; yBlock++)
        {
            for (int xBlock = 0; xBlock < 1 * amount.x; xBlock++)
            {
                for (int zBlock = 0; zBlock < 1 * amount.z; zBlock++)
                {
                    //呼叫PerlinNoiseGenerator函式所產生的2D高度圖，對應每一個點來定義目前位置的高度
                    //hightlevel = (int)(Heightmap.GetPixel(xBlock, zBlock).r * 100) - yBlock;
                    //airChecker = false;
                    Vector3 posTemp = new Vector3(xBlock, yBlock, zBlock);

                    //meshTemp = oceanMesh;
                    maTemp = oceanMaterial;
                    if (!maTemp)
                        maTemp = pinkMaterial;

                    //產生方塊的同時，在同一個座標也產生一個碰撞器Box Collider
                    //這種做法對效能不是最好，但由於這個工作坊執行時Unity ECS尚未支援物理，因此先這樣用，未來等支援再修改
                    //GM.GetComponent<ColliderPool>().AddCollider(posTemp);
                    //AddCollider(posTemp);

                    //基於BlockArchetype結構產生一個Entity
                    Entity entities = ECS_Generator.GM.manager.CreateEntity(ECS_Generator.BlockArchetype);

                    //把新的Entity指定目前座標後指定要畫的網格和材質，並新增一個BlockTag標籤
                    ECS_Generator.GM.manager.SetComponentData(entities, new Position { Value = new Vector3(xBlock * meshSize.x, yBlock, zBlock * meshSize.z) + offset });
                    ECS_Generator.GM.manager.AddComponentData(entities, new BlockTag { });

                    ECS_Generator.GM.manager.AddSharedComponentData(entities, new MeshInstanceRenderer
                    {
                        mesh = meshTemp,
                        material = maTemp
                    });
                }
            }
        }

    }
}