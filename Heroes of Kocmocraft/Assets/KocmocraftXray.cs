using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KocmocraftXray : MonoBehaviour
{
    public GameObject kocmocraft;
    public Material material;
    void Start()
    {
        MeshRenderer[] mesh = kocmocraft.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].sharedMaterial = material;
        }



    }
}
