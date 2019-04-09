using UnityEngine;
using UnityEngine.UI;

public partial class WakakaController : MonoBehaviour
{
    [Header("Custom Setting")]
    public Text textRightFire;
    public Text textRightThumb, textLeftFire, textLeftThumb;
    private int keycodeRightFire, keycodeRightThumb, keycodeLeftFire, keycodeLeftThumb;
    private bool repeatRightFire, repeatRightThumb, repeatLeftFire, repeatLeftThumb;

    public bool RepeatRightFire
    {
        set { repeatRightFire = value; }
        get { return repeatRightFire; }
    }
    public bool RepeatRightThumb
    {
        set { repeatRightThumb = value; }
        get { return repeatRightThumb; }
    }
    public bool RepeatLeftFire
    {
        set { repeatLeftFire = value; }
        get { return repeatLeftFire; }
    }
    public bool RepeatLeftThumb
    {
        set { repeatLeftThumb = value; }
        get { return repeatLeftThumb; }
    }

    void LoadingCustomKeycode()
    {
        keycodeRightFire = UnityKeyCode2KeycodeValue(PlayerPrefs.GetInt("saveRightFire"));
        textRightFire.text = "<color=yellow>[" + keycodeRightFire + "]</color>   " + ((KeyCode)PlayerPrefs.GetInt("saveRightFire")).ToString();

        keycodeRightThumb = UnityKeyCode2KeycodeValue(PlayerPrefs.GetInt("saveRightThumb"));
        textRightThumb.text = "<color=yellow>[" + keycodeRightThumb + "]</color>   " + ((KeyCode)PlayerPrefs.GetInt("saveRightThumb")).ToString();

        keycodeLeftFire = UnityKeyCode2KeycodeValue(PlayerPrefs.GetInt("saveLeftFire"));
        textLeftFire.text = "<color=yellow>[" + keycodeLeftFire + "]</color>   " + ((KeyCode)PlayerPrefs.GetInt("saveLeftFire")).ToString();

        keycodeLeftThumb = UnityKeyCode2KeycodeValue(PlayerPrefs.GetInt("saveLeftThumb"));
        textLeftThumb.text = "<color=yellow>[" + keycodeLeftThumb + "]</color>   " + ((KeyCode)PlayerPrefs.GetInt("saveLeftThumb")).ToString();

    }
    int UnityKeyCode2KeycodeValue(int unityCode)
    {
        if (unityCode >= 256 && unityCode <= 265)    // Unity Keypad的KeyCode轉換鍵值
            return unityCode - 160;
        else if (unityCode == 0)
            return 48;
        else
            return unityCode;
    }
    public void RightFireKeycodeSetting(string key)
    {
        int[] code = KeycodeSetting(key);
        keycodeRightFire = code[0];
        textRightFire.text = "<color=yellow>[" + keycodeRightFire + "]</color>   " + ((KeyCode)code[1]).ToString();
        PlayerPrefs.SetInt("saveRightFire", keycodeRightFire);
    }
    public void RightThumbKeycodeSetting(string key)
    {
        int[] code = KeycodeSetting(key);
        keycodeRightThumb = code[0];
        textRightThumb.text = "<color=yellow>[" + keycodeRightThumb + "]</color>   " + ((KeyCode)code[1]).ToString();
        PlayerPrefs.SetInt("saveRightThumb", keycodeRightThumb);
    }
    public void LeftFireKeycodeSetting(string key)
    {
        int[] code = KeycodeSetting(key);
        keycodeLeftFire = code[0];
        textLeftFire.text = "<color=yellow>[" + keycodeLeftFire + "]</color>   " + ((KeyCode)code[1]).ToString();
        PlayerPrefs.SetInt("saveLeftFire", keycodeLeftFire);
    }
    public void LeftThumbKeycodeSetting(string key)
    {
        int[] code = KeycodeSetting(key);
        keycodeLeftThumb = code[0];
        textLeftThumb.text = "<color=yellow>[" + keycodeLeftThumb + "]</color>   " + ((KeyCode)code[1]).ToString();
        PlayerPrefs.SetInt("saveLeftThumb", keycodeLeftThumb);
    }
    int[] KeycodeSetting(string key)
    {
        if (key == "")
            return new int[2] { 48, 48 };
        else
        {
            int valueKeycode;
            string keyName;

            if (int.TryParse(key, out valueKeycode)) // 數字
            {
                if (valueKeycode >= 256 && valueKeycode <= 265)    // Unity Keypad的KeyCode轉換鍵值
                    valueKeycode -= 160;

                // 一般數字 - 鍵值
                if (valueKeycode >= 48 && valueKeycode <= 57)
                    return new int[2] { valueKeycode, valueKeycode };
                // 數字鍵盤 - 鍵值
                else if (valueKeycode >= 96 && valueKeycode <= 105)
                    return new int[2] { valueKeycode, valueKeycode + 160 };
                // 其他按鍵
                else
                {
                    keyName = ((KeyCode)valueKeycode).ToString();
                    int valid;
                    if (!int.TryParse(keyName, out valid)) // 如果KeyCode存在
                        return new int[2] { valueKeycode, valueKeycode };
                    else
                        return new int[2] { 48, 48 };
                }
            }
            else
            {
                if (key[0] >= 97 && key[0] <= 122) //小寫字母先轉換為大寫字母
                    valueKeycode = key[0] - 32;
                else
                    valueKeycode = key[0];
                return new int[2] { valueKeycode, valueKeycode + 32 };
            }
        }
    }
}
