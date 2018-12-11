using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    /* Console */
    public Text msgBox;
    public Text keyword;
    private List<string> msgList = new List<string>();

    public void AddNewMsg(string msg)
    {
        if (string.IsNullOrEmpty(msg))
            return;
        byte[] byteArray = System.Text.Encoding.Unicode.GetBytes(msg);
        if (byteArray.Length < 1)
            Debug.LogWarning("QQ");
        // Arduino傳輸的錯誤，字元的第一個byte錯誤
        try
        {
            if (byteArray[0] == 0)
                return;
        }
        catch(System.Exception e)
        {
            Debug.LogWarning(e.ToString());
        }
        string str = System.Text.Encoding.Unicode.GetString(byteArray);
        msgBox.text = "";
        if (msgList.Count == 31)
            msgList.RemoveAt(0);
        msgList.Add(str);

        for (int i = 0; i < msgList.Count; i++)
        {
            if (i==0)
                msgBox.text += ("<color=yellow><i>" + i.ToString("00") + ": </i></color>" + msgList[i]);
            else
                msgBox.text += ("\n<color=yellow><i>" + i.ToString("00") + ": </i></color>" + msgList[i]);
        }
    }

    public void Keyword(string msg)
    {
        keyword.text = msg;
        AddNewMsg(msg);
    }
}
