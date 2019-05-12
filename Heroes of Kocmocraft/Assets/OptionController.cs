using UnityEngine;

public class OptionController : MonoBehaviour
{
    [Header("Skin")]
    public GameObject[] skin;
    private int countSkin;
    private int nowSkin = 0;

    private void Reset()
    {
        countSkin = transform.childCount;
        skin = new GameObject[countSkin];
        for (int i = 0; i < countSkin; i++)
        {
            skin[i] = transform.GetChild(i).gameObject;
            skin[i].SetActive(false);
        }
        skin[0].SetActive(true);
    }

    public int ChangeSkin()
    {
        nowSkin = (int)Mathf.Repeat(++nowSkin, skin.Length);
        for (int i = 0; i < skin.Length; i++)
        {
            skin[i].SetActive(false);
        }
        skin[nowSkin].SetActive(true);
        return nowSkin;
    }
}