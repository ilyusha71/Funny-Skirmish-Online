using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class MeshCalculator : MonoBehaviour
{
    private MeshFilter mf = null;
    private Vector3 scale;
    private float sum;
    private string sum1;

    // Use this for initialization
    void Start()
    {
        this.mf = this.GetComponent<MeshFilter>();
        //有损缩放
        this.scale = this.transform.lossyScale;
        this.CalculateSumArea();
        this.CalculateSumVolume();
    }
    private void CalculateSumVolume()
    {
        Vector3[] arrVertices = this.mf.mesh.vertices;
        int[] arrTriangles = this.mf.mesh.triangles;
        float sum = 0.0f;
        for (int i = 0; i < this.mf.mesh.subMeshCount; i++)
        {
            int[] arrIndices = this.mf.mesh.GetTriangles(i);
            for (int j = 0; j < arrIndices.Length; j += 3)
                sum += this.CalculateVolume(arrVertices[arrIndices[j]]
                                        , arrVertices[arrIndices[j + 1]]
                                        , arrVertices[arrIndices[j + 2]]);
        }


        Debug.Log("Volume= " + Mathf.Abs(sum));
    }
    private void CalculateSumArea()
    {
        Vector3[] arrVertices = this.mf.mesh.vertices;
        int[] arrTriangles = this.mf.mesh.triangles;
        float sum1 = 0.0f;
        for (int i = 0; i < this.mf.mesh.subMeshCount; i++)
        {
            int[] arrIndices = this.mf.mesh.GetTriangles(i);
            for (int j = 0; j < arrIndices.Length; j += 3)
                sum1 += this.CalculateArea(arrVertices[arrIndices[j]]
                                        , arrVertices[arrIndices[j + 1]]
                                        , arrVertices[arrIndices[j + 2]]);
        }

        Debug.Log("Area = " + sum1);
    }
    private float CalculateVolume(Vector3 pt0, Vector3 pt1, Vector3 pt2)
    {
        pt0 = new Vector3(pt0.x * this.scale.x, pt0.y * this.scale.y, pt0.z * this.scale.z);
        pt1 = new Vector3(pt1.x * this.scale.x, pt1.y * this.scale.y, pt1.z * this.scale.z);
        pt2 = new Vector3(pt2.x * this.scale.x, pt2.y * this.scale.y, pt2.z * this.scale.z);
        float v321 = pt2.x * pt1.y * pt0.z;
        float v231 = pt1.x * pt2.y * pt0.z;
        float v312 = pt2.x * pt0.y * pt1.z;
        float v132 = pt0.x * pt2.y * pt1.z;
        float v213 = pt1.x * pt0.y * pt2.z;
        float v123 = pt0.x * pt1.y * pt2.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }
    private float CalculateArea(Vector3 pt0, Vector3 pt1, Vector3 pt2)
    {
        pt0 = new Vector3(pt0.x * this.scale.x, pt0.y * this.scale.y, pt0.z * this.scale.z);
        pt1 = new Vector3(pt1.x * this.scale.x, pt1.y * this.scale.y, pt1.z * this.scale.z);
        pt2 = new Vector3(pt2.x * this.scale.x, pt2.y * this.scale.y, pt2.z * this.scale.z);
        float a = (pt1 - pt0).magnitude;
        float b = (pt2 - pt1).magnitude;
        float c = (pt0 - pt2).magnitude;
        float p = (a + b + c) * 0.5f;
        return Mathf.Sqrt(p * (p - a) * (p - b) * (p - c));
    }
    /* private void OnGUI()
     {
         GUILayout.Label("体积：" + Mathf.Abs(sum));
         GUILayout.Label("面积：" +sum1);
     }*/
}