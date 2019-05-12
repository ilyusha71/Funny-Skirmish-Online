using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CubeMesh : MonoBehaviour
{
    private const string MESH_PATH = "Assets/_Dev/Save/";

    [MenuItem("Examples/CubeMesh")]
    static void MakeCube()
    {
        GameObject obj = Selection.activeGameObject;
        if (obj.GetComponent<MeshRenderer>() == null)
        {
            obj.AddComponent<MeshRenderer>();
        }
        if (obj.GetComponent<MeshFilter>() == null)
        {
            obj.AddComponent<MeshFilter>();
        }
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        if (meshFilter != null)
            mesh = Instantiate(meshFilter.sharedMesh) as Mesh;

        if (mesh == null || mesh.uv.Length != 24)
        {
            Debug.Log("Script needs to be attached to built-in cube");
            return;
        }

        //合并Mesh
        meshFilter.sharedMesh = mesh;
        //合并第二套UV
        var UVs = mesh.uv;

        // 标准六面
        //// Front
        //UVs[0] = new Vector2(0.0f, 0.0f);
        //UVs[1] = new Vector2(0.333f, 0.0f);
        //UVs[2] = new Vector2(0.0f, 0.333f);
        //UVs[3] = new Vector2(0.333f, 0.333f);
        //// Top
        //UVs[4] = new Vector2(0.334f, 0.333f);
        //UVs[5] = new Vector2(0.666f, 0.333f);
        //UVs[8] = new Vector2(0.334f, 0.0f);
        //UVs[9] = new Vector2(0.666f, 0.0f);
        //// Back
        //UVs[6] = new Vector2(1.0f, 0.0f);
        //UVs[7] = new Vector2(0.667f, 0.0f);
        //UVs[10] = new Vector2(1.0f, 0.333f);
        //UVs[11] = new Vector2(0.667f, 0.333f);
        //// Bottom
        //UVs[12] = new Vector2(0.0f, 0.334f);
        //UVs[13] = new Vector2(0.0f, 0.666f);
        //UVs[14] = new Vector2(0.333f, 0.666f);
        //UVs[15] = new Vector2(0.333f, 0.334f);
        //// Left
        //UVs[16] = new Vector2(0.334f, 0.334f);
        //UVs[17] = new Vector2(0.334f, 0.666f);
        //UVs[18] = new Vector2(0.666f, 0.666f);
        //UVs[19] = new Vector2(0.666f, 0.334f);
        //// Right        
        //UVs[20] = new Vector2(0.667f, 0.334f);
        //UVs[21] = new Vector2(0.667f, 0.666f);
        //UVs[22] = new Vector2(1.0f, 0.666f);
        //UVs[23] = new Vector2(1.0f, 0.334f);

        // 人形四面（上下对称、左右对称）
        // Front
        UVs[0] = new Vector2(0.0f, 0.5f); // 左下
        UVs[1] = new Vector2(0.5f, 0.5f); // 右下
        UVs[2] = new Vector2(0.0f, 1.0f); // 左上
        UVs[3] = new Vector2(0.5f, 1.0f); // 右上
        // Top
        UVs[4] = new Vector2(0.5f, 0.5f); // 左上
        UVs[5] = new Vector2(1.0f, 0.5f); // 右上
        UVs[8] = new Vector2(0.5f, 0.0f); // 左下
        UVs[9] = new Vector2(1.0f, 0.0f); // 右下
        // Back
        UVs[6] = new Vector2(0.5f, 0.0f); // 右下
        UVs[7] = new Vector2(0.0f, 0.0f); // 左下
        UVs[10] = new Vector2(0.5f, 0.5f); // 右上
        UVs[11] = new Vector2(0.0f, 0.5f); // 左上
        // Bottom
        UVs[12] = new Vector2(0.5f, 0.0f); // 左下
        UVs[13] = new Vector2(0.5f, 0.5f); // 左上
        UVs[14] = new Vector2(1.0f, 0.5f); // 右上
        UVs[15] = new Vector2(1.0f, 0.0f); // 右下
        // Left
        UVs[16] = new Vector2(0.5f, 0.5f); // 左下
        UVs[17] = new Vector2(0.5f, 1.0f); // 左上
        UVs[18] = new Vector2(1.0f, 1.0f); // 右上
        UVs[19] = new Vector2(1.0f, 0.5f); // 右下
        // Right        
        UVs[20] = new Vector2(0.5f, 0.5f); // 左下
        UVs[21] = new Vector2(0.5f, 1.0f); // 左上
        UVs[22] = new Vector2(1.0f, 1.0f); // 右上
        UVs[23] = new Vector2(1.0f, 0.5f); // 右下

        mesh.uv = UVs;

        string tempPath = MESH_PATH + obj.name + "_Mesh.asset";
        AssetDatabase.CreateAsset(mesh, tempPath);
    }
}
