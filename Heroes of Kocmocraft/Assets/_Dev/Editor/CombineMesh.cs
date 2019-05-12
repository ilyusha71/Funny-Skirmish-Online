using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CombineMesh
{
    private const string MESH_PATH = "Assets/_Dev/Save/";

    [MenuItem("Examples/CombineMesh")]
    static void Combine()
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

        List<Material> material = new List<Material>();
        Matrix4x4 matrix = obj.transform.worldToLocalMatrix;
        MeshFilter[] filters = obj.GetComponentsInChildren<MeshFilter>();
        int filterLength = filters.Length;
        CombineInstance[] combine = new CombineInstance[filterLength];
        for (int i = 0; i < filterLength; i++)
        {
            MeshFilter filter = filters[i];
            MeshRenderer render = filter.GetComponent<MeshRenderer>();
            if (render == null)
            {
                continue;
            }
            if (render.sharedMaterial != null && !material.Contains(render.sharedMaterial))
            {
                material.Add(render.sharedMaterial);
            }
            combine[i].mesh = filter.sharedMesh;
            //对坐标系施加变换的方法是 当前对象和子对象在世界空间中的矩阵 左乘 当前对象从世界空间转换为本地空间的变换矩阵
            //得到当前对象和子对象在本地空间的矩阵。
            combine[i].transform = matrix * filter.transform.localToWorldMatrix;
            render.enabled = false;
        }

        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mesh.name = "Combine";
        //合并Mesh
        mesh.CombineMeshes(combine);
        meshFilter.sharedMesh = mesh;
        //合并第二套UV
        Unwrapping.GenerateSecondaryUVSet(meshFilter.sharedMesh);
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        renderer.sharedMaterials = material.ToArray();
        renderer.enabled = true;

        MeshCollider collider = new MeshCollider();
        if (collider != null)
        {
            collider.sharedMesh = mesh;
        }
        string tempPath = MESH_PATH + obj.name + "_mesh.asset";
        AssetDatabase.CreateAsset(meshFilter.sharedMesh, tempPath);
        //PrefabUtility.DisconnectPrefabInstance(obj);
    }

}