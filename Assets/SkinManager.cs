using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public GameObject[] skin;
    [SerializeField]private int index;

    public void InitializeSkin(int order)
    {
        int count = transform.childCount;
        skin = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            skin[i] = transform.GetChild(i).gameObject;
        }

        index = order;
        for (int i = 0; i < skin.Length; i++)
        {
            skin[i].SetActive(false);
        }
        skin[index].SetActive(true);
    }

    public int ChangeSkin()
    {
        index = (int)Mathf.Repeat(++index, skin.Length);
        for (int i = 0; i < skin.Length; i++)
        {
            skin[i].SetActive(false);
        }
        skin[index].SetActive(true);
        Debug.Log(index);
        return index;
    }
}
