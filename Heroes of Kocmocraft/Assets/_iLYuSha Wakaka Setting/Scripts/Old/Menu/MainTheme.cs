using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTheme : MonoBehaviour
{
    public static MainTheme instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);

        int sceneIndex = PlayerPrefs.GetInt("lastScene");
        if (sceneIndex >= 100)
            SceneManager.LoadScene(PlayerPrefs.GetInt("lastScene") - 100);
    }

    public void GoodBye()
    {
        DestroyImmediate(gameObject);
    }

}
