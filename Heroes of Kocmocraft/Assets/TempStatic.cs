using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempStatic : MonoBehaviour
{
    public static TempStatic instance;

    public Transform[] arrayOne = new Transform[50];
    public Transform[] arrayTwo = new Transform[50];
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

    }

    public void AddSearchList(int portNumber,Transform kocmocraft)
    {
        for (int i = 0; i < 2; i++)
        {
            if (i == portNumber % 2)
                arrayOne[portNumber / 2] = kocmocraft;
            else
                arrayTwo[portNumber / 2] = kocmocraft;
        }
    }
    public void AddSearchListCA(int fac,int num, Transform kocmocraft)
    {
        for (int i = 0; i < 2; i++)
        {
            if (i == fac)
                arrayOne[num] = kocmocraft;
            else
                arrayTwo[num] = kocmocraft;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
