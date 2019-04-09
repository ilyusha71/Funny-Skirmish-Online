using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempChange : MonoBehaviour
{
    int index = -99;
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            index = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            index = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            index = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            index = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            index = 4;
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            index = 5;
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            index = 6;
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            index = 7;

        if (index != -99)
        {
          //  PlayerPrefs.SetInt(GeneralInfo.saveAircraft, index);
        //    SceneManager.LoadScene("TerrainTutorial");
        }

    }
}
