using UnityEngine;
using System.Collections;

//Usage : 

public class PolygonMesh : MonoBehaviour
{
    public float scale = 1f;
    public int edge_num = 3;     //多邊形幾邊
    public int mag_level;    //有幾個等級?
    public float[] mag_array;
    public float moveRay;
    //reference
    public GameObject line_prefab;
    public Material mat;
    public GameObject abilitymesh;

    [Header("能力數值")]
    public float Hidden;
    public float HP;
    public float Detect;
    public float Flexibility;
    public float Speed;
    public float power;

    //internal variables
    GameObject line;
    GameObject meshobject;
    LineRenderer lines;
    Mesh mesh;

    GameObject[] meshobjects;
    Mesh[] meshes;

    protected Vector3[] vertice_positions;


    void Awake()
    {
        scale *= transform.parent.localScale.x;
        edge_num = 6;
        mag_array = new float[10];
        mag_array[0] = Hidden;
        mag_array[1] = HP;
        mag_array[2] = Detect;
        mag_array[3] = Flexibility;
        mag_array[4] = Speed;
        mag_array[5] = power;
        mag_array[6] = mag_array[0];
        mag_level = 100;
        setLineEdge(6, mag_array);
        //	setPolyMesh(); // 這是有問題的第一種作法
        setTriMeshes();
        gameObject.SetActive(false);
    }

    public void setTriMeshes()
    {
        meshobjects = new GameObject[edge_num];
        meshes = new Mesh[edge_num];

        for (int i = 0; i < edge_num; i++)
        {
            meshes[i] = new Mesh();

            //抓兩點,再跟原點 三點形成mesh
            Vector3[] Vertices = new Vector3[3];
            Vertices[0] = Vector3.zero;
            if (i == edge_num - 1)
            { //last mesh 
                Vertices[1] = vertice_positions[i];
                Vertices[2] = vertice_positions[0];
            }
            else
            {
                Vertices[1] = vertice_positions[i];
                Vertices[2] = vertice_positions[i + 1];
            }
            //UV 就直接assign
            Vector2[] UVs = new Vector2[3];
            UVs[0] = new Vector2(0, 0);
            UVs[1] = new Vector2(1, 1);
            UVs[2] = new Vector2(0, 0);

            //Tri 也直接assign
            int[] tris = new int[3] { 2, 1, 0 }; //順時針
                                                 //int[] tris = new int[3] {1,2,0}; //逆時針

            meshes[i].vertices = Vertices;
            meshes[i].uv = UVs;
            meshes[i].triangles = tris;


            meshobjects[i] = (GameObject)Instantiate(abilitymesh, Vector3.zero, Quaternion.identity);
            meshobjects[i].GetComponent<MeshFilter>().mesh = meshes[i];
            meshobjects[i].transform.parent = transform;
            meshobjects[i].transform.localPosition = Vector3.zero;
        }
    }

    public void setLineEdge(int poly_num, float[] mag_array)
    {
        edge_num = poly_num;
        int node_cnt = edge_num + 1;

        //gen position array
        vertice_positions = new Vector3[node_cnt];
        Vector3 unit_vector = Vector3.up;

        for (int i = 0; i < node_cnt; i++)
        {
            if (i == 0)
                unit_vector = Vector3.up * scale; //第一個vector 為 up , 之後照著每邊的內角以z軸旋轉	
            else
                //以下兩者都可將Vector旋轉
                unit_vector = Quaternion.AngleAxis((360f / edge_num), Vector3.forward) * unit_vector;
            //unit_vector = Quaternion.Euler(0,0,360f/edge_num) * unit_vector;


            vertice_positions[i] = unit_vector; //起始無旋轉角 + 每一區塊的轉角
                                                //            Debug.Log(mag_array[i] / (float)mag_level);
            vertice_positions[i] *= mag_array[i] / (float)mag_level; //根據mag_level , scale down vector


            //Debug.Log("vertice_positions["+i+"] = " + vertice_positions[i]); 
            if (i > 0)
            {
                line = (GameObject)Instantiate(line_prefab, Vector3.zero, Quaternion.identity);
                line.transform.parent = transform;
                line.transform.localPosition = Vector3.zero;
                line.transform.localScale = new Vector3(1, 1, 1);
                line.GetComponent<LineRenderer>().SetPosition(0, vertice_positions[i - 1]);
                line.GetComponent<LineRenderer>().SetPosition(1, vertice_positions[i]);
                line.GetComponent<LineRenderer>().startWidth = 0.05f;
                line.GetComponent<LineRenderer>().endWidth = 0.05f;
            }
        }
    }



    public void setPolyMesh()
    {
        mesh = new Mesh();

        Vector3[] Vertices = new Vector3[edge_num];
        for (int i = 0; i < edge_num; i++)
        {
            Vertices[i] = vertice_positions[i];
        }


        //最單純的UV展開
        Vector2[] UVs = new Vector2[edge_num];
        for (int x = 0; x < edge_num; x++)
        {
            if ((x % 2) == 0)
            {
                UVs[x] = new Vector2(0, 0);
            }
            else
            {
                UVs[x] = new Vector2(1, 1);
            }
        }


        //Note : tris 的rule是你要嘛對所有三角node做順時針描述,不然就是逆時針描述,只要違反,就不見!
        //Note2 : 凹邊型無法處理 
        //int[] tris = new int[12] {0,5,4,0,4,3,0,3,2,0,2,1};
        //int[] tris = new int[12] {0,1,2,0,2,3,0,3,4,0,4,5};
        //int[] tris = new int[12] {0,4,3,0,5,4,0,2,1,0,3,2};


        int[] tris = new int[3 * (edge_num - 2)];    //3 verts per triangle * num triangles
        int C1, C2, C3;

        bool clockwise = true; //決定順時針或逆時針描述 (正面 or 反面)

        if (clockwise == true)
        {
            C1 = 0;
            C2 = edge_num - 1;
            C3 = edge_num - 2;
            for (int i = 0; i < tris.Length; i += 3)
            {
                tris[i] = C1;
                tris[i + 1] = C2;
                tris[i + 2] = C3;

                C2--;
                C3--;
            }
        }
        else
        {
            C1 = 0;
            C2 = 1;
            C3 = 2;
            for (int i = 0; i < tris.Length; i += 3)
            {
                tris[i] = C1;
                tris[i + 1] = C2;
                tris[i + 2] = C3;

                C2++;
                C3++;
            }
        }
        mesh.vertices = Vertices;
        mesh.uv = UVs;
        mesh.triangles = tris;

        meshobject = (GameObject)Instantiate(abilitymesh, Vector3.zero, Quaternion.identity);
        meshobject.GetComponent<MeshFilter>().mesh = mesh;
        meshobject.transform.parent = transform;
        //}
    }
}