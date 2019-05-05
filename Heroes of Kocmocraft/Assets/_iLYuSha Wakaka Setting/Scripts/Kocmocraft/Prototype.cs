using UnityEngine;
using Cinemachine;

namespace Kocmoca
{
    public class Prototype : MonoBehaviour
    {
        [Header("Skin")]
        public GameObject[] skin; // skin[0] 留给 Blueprint
        [SerializeField]
        private int countSkin;
        private int nowSkin = 1;
        private int lastSkin = 1;

        [Header("Size View")]
        public CinemachineFreeLook cmFreeLook;
        public float wingspan, length,height;
        public Vector3 wingspanScale, lengthScale, heightScale;
        public float orthoSize; // 用于三视图摄影机正交尺寸
        public float near, far;

        public void Create()
        {
            Transform[] objects = GetComponentsInChildren<Transform>();
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].gameObject.layer = 9;
            }

            CheckSkin();
            InitializeCinemachineFreeLook();
        }

        #region Skin
        void CheckSkin()
        {
            countSkin = transform.childCount - 1; // 扣除 Free Look Camera
            skin = new GameObject[countSkin];
            for (int i = 0; i < countSkin; i++)
            {
                skin[i] = transform.GetChild(i).gameObject;
            }
            countSkin--; // 預設第一項為 Blueprint
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
        public void RandomSkin()
        {
            nowSkin = Random.Range(1, countSkin);
            for (int i = 0; i < skin.Length; i++)
            {
                skin[i].SetActive(false);
            }
            skin[nowSkin].SetActive(true);
        }
        #endregion

        #region Free Look
        void InitializeCinemachineFreeLook()
        {
            Vector3 size = GetComponent<BoxCollider>().size;
            wingspan = size.x;
            length = size.z;
            height = size.y;
            float max = wingspan > length ? (wingspan > height ? wingspan : height) : (length > height ? length : height);
            wingspanScale = new Vector3(wingspan / max, 1, 1);
            lengthScale = new Vector3(length / max, 1, 1);
            heightScale = new Vector3(height / max, 1, 1);
            orthoSize = max * 0.5f;
            near = orthoSize + 2.7f;
            far = orthoSize + 19.3f;
            cmFreeLook = GetComponentInChildren<CinemachineFreeLook>();
            cmFreeLook.Follow = transform;
            cmFreeLook.LookAt = transform;
            cmFreeLook.m_Lens.FieldOfView = 60;
            cmFreeLook.m_BindingMode = CinemachineTransposer.BindingMode.LockToTarget;
            cmFreeLook.m_Orbits[0].m_Height = orthoSize; //sizeView.Height * 0.5f + 5;
            cmFreeLook.m_Orbits[1].m_Height = 0;
            cmFreeLook.m_Orbits[2].m_Height = -orthoSize; //-sizeView.Height;
            cmFreeLook.m_Orbits[0].m_Radius = far; //sizeView.NearView + 2;
            cmFreeLook.m_Orbits[1].m_Radius = far; //sizeView.HalfSize + 15;
            cmFreeLook.m_Orbits[2].m_Radius = far; //sizeView.NearView + 1;
            cmFreeLook.m_Heading.m_Bias = Random.Range(-180, 180);
            cmFreeLook.m_YAxis.Value = 1.0f;
            cmFreeLook.m_XAxis.m_InputAxisName = string.Empty;
            cmFreeLook.m_YAxis.m_InputAxisName = string.Empty;
            cmFreeLook.m_XAxis.m_InvertInput = false;
            cmFreeLook.m_YAxis.m_InvertInput = true;
            cmFreeLook.enabled = false;
        }
        #endregion

        public void Initialize()
        {


            //sizeView = new SizeView(GetComponent<BoxCollider>().size);
            //cmFreeLook = GetComponentInChildren<CinemachineFreeLook>();
            //cmFreeLook.Follow = transform;
            //cmFreeLook.LookAt = transform;
            //cmFreeLook.m_Lens.FieldOfView = 60;
            //cmFreeLook.m_BindingMode = CinemachineTransposer.BindingMode.LockToTarget;
            //cmFreeLook.m_Orbits[0].m_Height = sizeView.Height * 0.5f + 5;
            //cmFreeLook.m_Orbits[1].m_Height = 0;
            //cmFreeLook.m_Orbits[2].m_Height = -sizeView.Height;
            //cmFreeLook.m_Orbits[0].m_Radius = sizeView.NearView + 2;
            //cmFreeLook.m_Orbits[1].m_Radius = sizeView.HalfSize + 15;
            //cmFreeLook.m_Orbits[2].m_Radius = sizeView.NearView + 1;
            //cmFreeLook.m_Heading.m_Bias = Random.Range(-180, 180);
            //cmFreeLook.m_YAxis.Value = 1.0f;
            //cmFreeLook.m_XAxis.m_InputAxisName = string.Empty;
            //cmFreeLook.m_YAxis.m_InputAxisName = string.Empty;
            //cmFreeLook.m_XAxis.m_InvertInput = false;
            //cmFreeLook.m_YAxis.m_InvertInput = true;
            //cmFreeLook.enabled = false;
        }

    }
}