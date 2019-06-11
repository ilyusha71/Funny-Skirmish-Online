using Cinemachine;
using UnityEngine;

namespace Kocmoca
{
    public class Prototype : MonoBehaviour
    {
        [Header ("Skin")]
        public GameObject[] skin; // skin[0] 留给 Blueprint
        [SerializeField]
        private int countSkin;
        private int nowSkin = 1;
        private int lastSkin = 1;
        [Header ("FreeLook")]
        public CinemachineFreeLook cmFreeLook;

        void Reset ()
        {
            Transform[] objects = GetComponentsInChildren<Transform> ();
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].gameObject.layer = 9;
            }

            CheckSkin ();
            Debug.Log ("<color=lime>" + name + " data has been preset.</color>");
        }
        #region Skin
        void CheckSkin ()
        {
            countSkin = transform.childCount - 1; // 扣除 Free Look Camera
            skin = new GameObject[countSkin];
            for (int i = 0; i < countSkin; i++)
            {
                skin[i] = transform.GetChild (i).gameObject;
            }
            countSkin--; // 預設第一項為 Blueprint
        }
        public void LoadSkin (int order)
        {
            nowSkin = order == 0 ? 1 : order; // skin[1] 为经典造型
            for (int i = 0; i < skin.Length; i++)
            {
                skin[i].SetActive (false);
            }
            skin[nowSkin].SetActive (true);
        }
        public int ChangeSkin ()
        {
            nowSkin = (int) Mathf.Repeat (++nowSkin, skin.Length) == 0 ? 1 : nowSkin;
            for (int i = 0; i < skin.Length; i++)
            {
                skin[i].SetActive (false);
            }
            skin[nowSkin].SetActive (true);
            return nowSkin;
        }
        public void SwitchWireframe ()
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
                skin[i].SetActive (false);
            }
            skin[nowSkin].SetActive (true);
        }
        public int GetRandomSkinIndex ()
        {
            return Random.Range (1, countSkin);
        }
        public void RandomSkin ()
        {
            nowSkin = Random.Range (1, countSkin);
            for (int i = 0; i < skin.Length; i++)
            {
                skin[i].SetActive (false);
            }
            skin[nowSkin].SetActive (true);
        }
        #endregion

        #region Free Look
        public void CreatePrototypeDatabase ()
        {
            Vector3 size = GetComponent<BoxCollider> ().size;
            float wingspan = size.x;
            float length = size.z;
            float height = size.y;
            float max = wingspan > length ? (wingspan > height ? wingspan : height) : (length > height ? length : height);
            float wingspanScale = wingspan / max;
            float lengthScale = length / max;
            float heightScale = height / max;
            float orthoSize = max * 0.5f;
            float near = orthoSize + 2.7f;
            float far = orthoSize + 19.3f;
            cmFreeLook = GetComponentInChildren<CinemachineFreeLook> ();
            cmFreeLook.enabled = true;
            cmFreeLook.Follow = transform;
            cmFreeLook.LookAt = transform;
            cmFreeLook.m_Lens.FieldOfView = 60;
            cmFreeLook.m_BindingMode = CinemachineTransposer.BindingMode.LockToTarget;
            cmFreeLook.m_Orbits[0].m_Height = orthoSize + 3; //sizeView.Height * 0.5f + 5;
            cmFreeLook.m_Orbits[1].m_Height = 0;
            cmFreeLook.m_Orbits[2].m_Height = -orthoSize - 3; //-sizeView.Height;
            cmFreeLook.m_Orbits[0].m_Radius = 11; //sizeView.NearView + 2;
            cmFreeLook.m_Orbits[1].m_Radius = 11; //sizeView.HalfSize + 15;
            cmFreeLook.m_Orbits[2].m_Radius = 11; //sizeView.NearView + 1;
            cmFreeLook.m_Heading.m_Bias = Random.Range (-180, 180);
            cmFreeLook.m_YAxis.Value = 1.0f;
            cmFreeLook.m_XAxis.m_InputAxisName = string.Empty;
            cmFreeLook.m_YAxis.m_InputAxisName = string.Empty;
            cmFreeLook.m_XAxis.m_InvertInput = false;
            cmFreeLook.m_YAxis.m_InvertInput = true;
            cmFreeLook.enabled = false;

            // Saving
#if UNITY_EDITOR
            int type = int.Parse (name.Split (new char[2] { '(', ')' }) [1]);
            KocmocraftDatabase index = UnityEditor.AssetDatabase.LoadAssetAtPath<KocmocraftDatabase> ("Assets/_iLYuSha Wakaka Setting/ScriptableObject/Kocmocraft Database.asset");
            KocmocraftModule module = index.kocmocraft[type];
            module.type = (Type) type;
            module.design.size.wingspan = wingspan;
            module.design.size.length = length;
            module.design.size.height = height;
            module.design.size.wingspanScale = wingspanScale;
            module.design.size.lengthScale = lengthScale;
            module.design.size.heightScale = heightScale;
            module.design.size.weight = Mathf.RoundToInt (30 * (wingspan * length + length * height + height * wingspan) + wingspan * length * height);
            module.design.view.orthoSize = orthoSize;
            module.design.view.near = near;
            module.design.view.far = far;
            module.name = string.Format ("{0:00}", type);
            module.Calculate ();
            UnityEditor.AssetDatabase.SaveAssets ();
#endif
        }
        #endregion
    }
}