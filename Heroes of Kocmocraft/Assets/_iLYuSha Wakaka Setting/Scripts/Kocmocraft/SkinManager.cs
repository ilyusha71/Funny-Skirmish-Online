using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Kocmoca
{
    public class SkinManager : MonoBehaviour
    {
        public ViewData viewData;
        public CinemachineFreeLook viewCamera;
        public GameObject[] skin; // skin[0] 留给 Blueprint
        private int countSkin;
        private int nowSkin = 1;
        private int lastSkin = 1;

        private void Awake()
        {
            Vector3 size = GetComponent<BoxCollider>().size;
            viewData = new ViewData(size);
            viewCamera = GetComponentInChildren<CinemachineFreeLook>();
            viewCamera.Follow = transform;
            viewCamera.LookAt = transform;
            viewCamera.m_Lens.FieldOfView = 60;
            viewCamera.m_BindingMode = CinemachineTransposer.BindingMode.LockToTarget;
            viewCamera.m_Orbits[0].m_Height = viewData.Height * 0.5f + 10;
            viewCamera.m_Orbits[1].m_Height = 0;
            viewCamera.m_Orbits[2].m_Height = -viewData.Height * 0.3f;
            viewCamera.m_Orbits[0].m_Radius = viewData.HalfSize + 15;
            viewCamera.m_Orbits[1].m_Radius = viewData.HalfSize + 15;
            viewCamera.m_Orbits[2].m_Radius = viewData.HalfSize + 15;
            viewCamera.m_Heading.m_Bias = Random.Range(-180, 180);
            viewCamera.m_YAxis.Value = 1.0f;
            viewCamera.m_XAxis.m_InputAxisName = string.Empty;
            viewCamera.m_YAxis.m_InputAxisName = string.Empty;
            viewCamera.m_XAxis.m_InvertInput = false;
            viewCamera.m_YAxis.m_InvertInput = true;
            viewCamera.enabled = false;

            countSkin = transform.childCount-1; // 扣除viewCamera
            skin = new GameObject[countSkin];
            for (int i = 0; i < countSkin; i++)
            {
                skin[i] = transform.GetChild(i+1).gameObject;
            }
            countSkin--;
        }

        public void LoadSkin(int order)
        {
            nowSkin = order == 0 ? 1 : order; // skin[1] 为经典造型
            for (int i = 0; i < skin.Length; i++)
            {
                skin[i].SetActive(false);
            }
            skin[nowSkin].SetActive(true);
        }

        public void RandomSkin()
        {
            nowSkin = Random.Range(1, countSkin);
            for (int i = 0; i < skin.Length; i++)
            {
                skin[i].SetActive(false);
            }
            skin[nowSkin].SetActive(true);
        }

        public int ChangeSkin()
        {
            nowSkin = (int)Mathf.Repeat(++nowSkin, skin.Length) == 0 ? 1 : nowSkin;
            for (int i = 0; i < skin.Length; i++)
            {
                skin[i].SetActive(false);
            }
            skin[nowSkin].SetActive(true);
            return nowSkin;
        }

        public void SwitchWireframe()
        {
            if (nowSkin == 0)
                nowSkin = lastSkin;
            else
            {
                lastSkin = nowSkin;
                nowSkin = 0;
            }

            for (int i = 0; i < skin.Length; i++)
            {
                skin[i].SetActive(false);
            }
            skin[nowSkin].SetActive(true);
        }
    }

}