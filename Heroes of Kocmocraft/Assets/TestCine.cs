using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCine : MonoBehaviour
{
    public Cinemachine.CinemachineFreeLook[] vcam;
    int index;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            index = (int)Mathf.Repeat(++index, vcam.Length);
            for (int i = 0; i < vcam.Length; i++)
            {
                vcam[i].enabled = false;
            }
            vcam[index].enabled = true;

        }
    }
}
