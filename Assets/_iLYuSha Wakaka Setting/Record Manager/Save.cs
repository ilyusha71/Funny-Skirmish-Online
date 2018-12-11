using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class Save : MonoBehaviour
{
    private string folderPath;

    void Awake ()
    {
        folderPath = "C:/Screenshot - " + SceneManager.GetActiveScene().name + "/";
    }

    #region Save
    public IEnumerator Screenshot()
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        //在擷取畫面之前請等到所有的Camera都Render完
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(Camera.main.pixelWidth, Camera.main.pixelHeight, TextureFormat.RGB24, false);
        //擷取作畫範圍 parm: recordWidth = 1600 (e.g. 1920 x 1080)
        texture.ReadPixels(new Rect(0, 0, Camera.main.pixelWidth, Camera.main.pixelHeight), 0, 0, false);
        texture.Apply();

        /* Save */
        SaveTextureToFile(texture, "Screenshot " + 
            DateTime.Now.Year + "." +
            DateTime.Now.Month + "." +
            DateTime.Now.Day + " - " +
            DateTime.Now.Hour + "." +
            DateTime.Now.Minute + "." +
            DateTime.Now.Second);
    }
    public IEnumerator Screenshot(GameObject obj)
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        obj.SetActive(true);
        yield return new WaitForEndOfFrame();
        obj.SetActive(false);
        //在擷取畫面之前請等到所有的Camera都Render完
        Texture2D texture = new Texture2D(Camera.main.pixelWidth, Camera.main.pixelHeight, TextureFormat.RGB24, false);
        //擷取作畫範圍 parm: recordWidth = 1600 (e.g. 1920 x 1080)
        texture.ReadPixels(new Rect(0, 0, Camera.main.pixelWidth, Camera.main.pixelHeight), 0, 0, false);
        texture.Apply();

        /* Save */
        SaveTextureToFile(texture, "Screenshot " +
            DateTime.Now.Year + "." +
            DateTime.Now.Month + "." +
            DateTime.Now.Day + " - " +
            DateTime.Now.Hour + "." +
            DateTime.Now.Minute + "." +
            DateTime.Now.Second);
    }

    void SaveTextureToFile(Texture2D texture, string fileName)
    {
        byte[] bytes = texture.EncodeToPNG();
        string filePath = folderPath + fileName + ".png";
        using (FileStream fs = File.Open(filePath, FileMode.Create))
        {
            BinaryWriter binary = new BinaryWriter(fs);
            binary.Write(bytes);
        }
    }
    #endregion
}
