using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Minecraft
{
    public class GameSettings : MonoBehaviour
    {
        public static GameSettings GM;
        public static Texture2D Heightmap;

        public static EntityArchetype BlockArchetype;

        //設定世界區塊數量 每加1 = 10x10x15 = 1500個方塊
        [Header("World = ChunkBase x ChunkBase")]
        public int ChunkBase = 1;

        //基礎的模型一般方塊，地表方塊和地面植物
        [Header("Mesh Info")]
        public Mesh blockMesh;
        public Mesh surfaceMesh;
        public Mesh tallGrassMesh;
        public MeshFilter filter;
        public Mesh oceanMesh;
        public Material oceanMaterial;

        //一般方塊可能會用到的材質貼圖 - 自然類貼圖由上到下為(石頭、木頭、樹葉、表面、鵝軟石、泥土、草、花、雲)
        [Header("Nature Block Type")]
        public Material stoneMaterial;
        public Material woodMaterial;
        public Material leavesMaterial;
        public Material surfaceMaterial;
        public Material cobbleMaterial;
        public Material dirtMaterial;
        public Material tallGrassMaterial;
        public Material roseMaterial;
        public Material CloudMaterial;

        //非自然類方塊材質(玻璃、磚塊、木板、炸藥)
        [Header("Other Block Type")]
        public Material glassMaterial;
        public Material brickMaterial;
        public Material plankMaterial;
        public Material tntMaterial;

        //當沒有找到材質，使用這個粉紅材質
        [Header("")]
        public Material pinkMaterial;

        //是否要產生碰撞結構
        [Header("Collision Settings")]
        //public float playerCollisionRadius;
        public bool createCollider;

        int ranDice;
        Material maTemp;
        Mesh meshTemp;

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

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        void Start()
        {
            //同上，呼叫一個EntityManager
            manager = World.Active.GetOrCreateManager<EntityManager>();
            //執行方塊產生函式
            //ChunkGenerator(ChunkBase);
            Test2(new float3(5.0f, 1.0f, 5.0f), new float3(10000.0f, 0.5f, 10000.0f), new Vector3(0, -2.1f, 0), true);

        }
        Vector3[] baseVertices;
        void Test2(float3 scale, float3 size, Vector3 offset, bool alignCenter)
        {
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
                        AddCollider(posTemp);

                        //基於BlockArchetype結構產生一個Entity
                        Entity entities = manager.CreateEntity(BlockArchetype);

                        //把新的Entity指定目前座標後指定要畫的網格和材質，並新增一個BlockTag標籤
                        manager.SetComponentData(entities, new Position { Value = new Vector3(xBlock * meshSize.x, yBlock, zBlock * meshSize.z) + offset });
                        manager.AddComponentData(entities, new BlockTag { });

                        manager.AddSharedComponentData(entities, new MeshInstanceRenderer
                        {
                            mesh = meshTemp,
                            material = maTemp
                        });
                    }
                }
            }

        }


        void ChunkGenerator(int amount)
        {

            int totalamount = (amount * amount) * 1500;
            //int ordernumber = 0;
            int hightlevel;
            bool airChecker;


            //方塊排序從 X0,Y0,Z0 to X15,Y10,Z10 為基礎產生ChunkBase X ChunkBase塊
            for (int yBlock = 0; yBlock < 15; yBlock++)
            {
                for (int xBlock = 0; xBlock < 10 * amount; xBlock++)
                {
                    for (int zBlock = 0; zBlock < 10 * amount; zBlock++)
                    {
                        //呼叫PerlinNoiseGenerator函式所產生的2D高度圖，對應每一個點來定義目前位置的高度
                        hightlevel = (int)(Heightmap.GetPixel(xBlock, zBlock).r * 100) - yBlock;
                        airChecker = false;
                        Vector3 posTemp = new Vector3(xBlock, yBlock, zBlock);

                        //依照不同的高度來產生不同的方塊
                        switch (hightlevel)
                        {
                            //如果高度是0, 代表這是地表上，亂數隨機產生(沒方塊、草、花、樹木、雲)
                            case 0:
                                //random surface block
                                ranDice = UnityEngine.Random.Range(1, 201);
                                if (ranDice <= 20)
                                {
                                    //草
                                    PlantGenerator(xBlock, yBlock, zBlock, 1);

                                }
                                if (ranDice == 198)
                                {
                                    //雲
                                    CloudGenerator(xBlock, yBlock, zBlock);
                                }
                                if (ranDice == 200)
                                {
                                    //花
                                    PlantGenerator(xBlock, yBlock, zBlock, 2);
                                }
                                if (ranDice == 199)
                                {
                                    //樹
                                    TreeGenerator(xBlock, yBlock, zBlock);


                                }
                                airChecker = true;
                                break;
                            //如果高度1,代表這是地表層，塞入地表方塊
                            case 1:
                                meshTemp = surfaceMesh;
                                maTemp = surfaceMaterial;
                                break;
                            //如果高度為2,3,4塞入泥土方塊
                            case 2:
                            case 3:
                            case 4:
                                //Dirt
                                meshTemp = blockMesh;
                                maTemp = dirtMaterial;
                                break;
                            //如果高度為5,6塞入石頭方塊
                            case 5:
                            case 6:
                                //stone block
                                meshTemp = blockMesh;
                                maTemp = stoneMaterial;
                                break;
                            //如果高度是7,8塞入鵝軟石方塊
                            case 7:
                            case 8:
                                meshTemp = blockMesh;
                                maTemp = cobbleMaterial;
                                break;
                            //預設的情況下設麼都不要塞
                            default:
                                //airBlock or anything higher level < 0
                                airChecker = true;

                                break;

                        }

                        if (!airChecker)
                        {

                            if (!maTemp)
                                maTemp = pinkMaterial;

                            //產生方塊的同時，在同一個座標也產生一個碰撞器Box Collider
                            //這種做法對效能不是最好，但由於這個工作坊執行時Unity ECS尚未支援物理，因此先這樣用，未來等支援再修改
                            //GM.GetComponent<ColliderPool>().AddCollider(posTemp);
                            AddCollider(posTemp);

                            //基於BlockArchetype結構產生一個Entity
                            Entity entities = manager.CreateEntity(BlockArchetype);

                            //把新的Entity指定目前座標後指定要畫的網格和材質，並新增一個BlockTag標籤
                            manager.SetComponentData(entities, new Position { Value = new int3(xBlock, yBlock, zBlock) });
                            manager.AddComponentData(entities, new BlockTag { });

                            manager.AddSharedComponentData(entities, new MeshInstanceRenderer
                            {
                                mesh = meshTemp,
                                material = maTemp
                            });
                        }
                    }
                }
            }
        }
        void TreeGenerator(int xPos, int yPos, int zPos)
        {
            //樹木，把當前座標xpos,ypos,zpos作為樹根，往上長其他樹幹和樹葉
            for (int i = yPos; i < yPos + 7; i++)
            {
                //高度到頂要放樹葉
                if (i == yPos + 6)
                {
                    maTemp = leavesMaterial;
                }
                else
                {
                    maTemp = woodMaterial;
                }

                if (!maTemp)
                    maTemp = pinkMaterial;

                //幫樹葉加上碰撞結構，這裏也是暫時加上碰撞器，直到ECS支援物理後可以改進
                Vector3 posTemp = new Vector3(xPos, i, zPos);
                //GM.GetComponent<ColliderPool>().AddCollider(posTemp);
                AddCollider(posTemp);

                //產生Entity，設定模型材質
                Entity entities = manager.CreateEntity(BlockArchetype);
                manager.SetComponentData(entities, new Position { Value = new int3(xPos, i, zPos) });
                manager.AddComponentData(entities, new BlockTag { });
                manager.AddSharedComponentData(entities, new MeshInstanceRenderer
                {
                    mesh = blockMesh,
                    material = maTemp
                });

                //如果高度在3-6之間要額外種樹葉
                if (i >= yPos + 3 && i <= yPos + 6)
                {
                    for (int j = xPos - 1; j <= xPos + 1; j++)
                    {
                        for (int k = zPos - 1; k <= zPos + 1; k++)
                        {
                            if (k != zPos || j != xPos)
                            {
                                //加上碰撞器
                                posTemp = new Vector3(j, i, k);
                                //GM.GetComponent<ColliderPool>().AddCollider(posTemp);
                                AddCollider(posTemp);

                                //產生Entity，設定模型材質
                                entities = manager.CreateEntity(BlockArchetype);
                                manager.SetComponentData(entities, new Position { Value = new int3(j, i, k) });
                                manager.AddComponentData(entities, new BlockTag { });
                                //manager.AddComponentData(entities, new HasCollider { ColliderState = false });
                                manager.AddSharedComponentData(entities, new MeshInstanceRenderer
                                {
                                    mesh = blockMesh,
                                    material = leavesMaterial
                                });
                            }
                        }
                    }
                }
            }
        }

        void PlantGenerator(int xPos, int yPos, int zPos, int plantType)
        {

            //當亂數產生草，在該座標種上花或草
            if (plantType == 1)
            {
                maTemp = tallGrassMaterial;
            }
            else
            {
                maTemp = roseMaterial;
            }

            if (!maTemp)
                maTemp = pinkMaterial;

            Quaternion rotation = Quaternion.Euler(0, 45, 0);

            //同上建立Entity,指定模型材質，不一樣的地方是由於模型是一個十字型，先把它轉45度，然後指定一個SurfacePlantTag
            //這種方塊沒有碰撞人可以穿過，所以不用加上碰撞器
            Entity entities = manager.CreateEntity(BlockArchetype);
            manager.SetComponentData(entities, new Position { Value = new int3(xPos, yPos, zPos) });
            manager.AddComponentData(entities, new Rotation { Value = rotation });
            manager.AddComponentData(entities, new SurfacePlantTag { });
            manager.AddSharedComponentData(entities, new MeshInstanceRenderer
            {
                mesh = tallGrassMesh,
                material = maTemp
            });
        }

        void CloudGenerator(int xPos, int yPos, int zPos)
        {

            meshTemp = blockMesh;
            maTemp = CloudMaterial;

            if (!maTemp)
                maTemp = pinkMaterial;

            ranDice = UnityEngine.Random.Range(4, 7);

            //從該座標往上Y加15,隨機產生4-7大小的雲塊，產生的Entity指定模型材質
            for (int i = 0; i < ranDice; i++)
            {
                for (int j = 0; j < ranDice; j++)
                {
                    Entity entities = manager.CreateEntity(BlockArchetype);
                    manager.SetComponentData(entities, new Position { Value = new int3(xPos + i, yPos + 15, zPos + j) });
                    manager.AddSharedComponentData(entities, new MeshInstanceRenderer
                    {
                        mesh = meshTemp,
                        material = maTemp
                    });
                }
            }
        }

        void AddCollider(Vector3 posTemp)
        {
            if (createCollider)
                GM.GetComponent<ColliderPool>().AddCollider(posTemp);
        }
    }
}
