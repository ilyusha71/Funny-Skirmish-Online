using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kocmoca
{

    //[ExecuteInEditMode]
    public partial class WakakaBase : MonoBehaviour
    {
        [Header("Scene - kocmocraft Parameter")]
        public Prototype[] prototype;
        public CinemachineFreeLook[] cmFreeLook;

        //public BoxCollider[] kocmocraftSize;

        //public ViewData[] viewData;
        [Header("Scene - Hangar")]
        public Transform hangar;
        public int hangarCount;
        public Transform[] hangars;
        [Header("Scene - Apron")]
        public Transform westSide;
        public Transform EastSide;
        public Transform[] apron;
        public GameObject[] signboards;
        public GameObject[] stickers;
        [Header("Resources - Signboard")]
        public GameObject Signboard;
        public Material[] OKB;
        [Header("Resources - Sticker")]
        public GameObject Sticker;
        public Texture2D StickerHangarNumber;
        public Texture2D StickerKocmocraftName;
        private Sprite[] spritesHangarNumber;
        private Sprite[] spritesKocmocraftName;


        public void Create()
        {
            prototype = hangar.GetComponentsInChildren<Prototype>();
            cmFreeLook = hangar.GetComponentsInChildren<CinemachineFreeLook>();
            hangarCount = prototype.Length;
            hangars = new Transform[hangarCount];

            for (int i = 0; i < hangarCount; i++)
            {
                prototype[i].Initialize();
                cmFreeLook[i].m_Orbits[2].m_Height = -prototype[i].height * 0.3f;
                cmFreeLook[i].m_Orbits[0].m_Radius = prototype[i].near;
                cmFreeLook[i].m_Orbits[1].m_Radius = 11;
                cmFreeLook[i].m_Orbits[2].m_Radius = prototype[i].near;
                hangars[i] = prototype[i].transform.parent.transform;
                hangars[i].localPosition = new Vector3(630 - (i % 12 / 3) * 360 - i % 3 * 90, hangars[i].GetComponentInChildren<BoxCollider>().size.y * 0.5f + 2, 0);
            }



            cutSprites(StickerHangarNumber, out spritesHangarNumber);
            cutSprites(StickerKocmocraftName, out spritesKocmocraftName);

            int count = signboards.Length;
            for (int i = 0; i < count; i++)
            {
                DestroyImmediate(signboards[i]);
                DestroyImmediate(stickers[i]);
            }

            int countApron = apron.Length;
            signboards = new GameObject[countApron];
            stickers = new GameObject[countApron];
            for (int i = 0; i < countApron; i++)
            {
                //int baseM = i / 12 == 0 ? -630 + (i / 3) * 360 + i % 3 * 90 : 630 - (i / 3) * 360 - i % 3 * 90;
                apron[i].SetParent(i < 12 ? westSide : EastSide);
                apron[i].localPosition = new Vector3(-630 + (i % 12 / 3) * 360 + i % 3 * 90, 0, 0);

                signboards[i] = Instantiate(Signboard, apron[i]);
                signboards[i].GetComponentsInChildren<MeshRenderer>()[0].material = OKB[i];
                signboards[i].GetComponentsInChildren<MeshRenderer>()[1].material = OKB[i];
                signboards[i].GetComponentsInChildren<MeshRenderer>()[2].material = OKB[i];

                stickers[i] = Instantiate(Sticker, apron[i]);
                stickers[i].GetComponentsInChildren<SpriteRenderer>()[0].sprite = spritesHangarNumber[i];
                stickers[i].GetComponentsInChildren<SpriteRenderer>()[1].sprite = spritesHangarNumber[i];
                stickers[i].GetComponentsInChildren<SpriteRenderer>()[2].sprite = spritesKocmocraftName[i];

            }
        }


        private void cutSprites(Texture2D spriteSheet, out Sprite[] _sprites)
        {
            _sprites = new Sprite[0];
            if (!IsAtlas(spriteSheet))
            {
                Debug.LogWarning("Unable to proceed, the source texture is not a sprite atlas.");
                return;
            }
            //Proceed to read all sprites from CopyFrom texture and reassign to a TextureImporter for the end result
            UnityEngine.Object[] _objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(spriteSheet));

            if (_objects != null && _objects.Length > 0)
                _sprites = new Sprite[_objects.Length];

            for (int i = 0; i < _objects.Length; i++)
            {
                _sprites[i] = _objects[i] as Sprite;
            }
        }

        //Check that the texture is an actual atlas and not a normal texture
        private bool IsAtlas(Texture2D tex)
        {
            string _path = AssetDatabase.GetAssetPath(tex);
            TextureImporter _importer = AssetImporter.GetAtPath(_path) as TextureImporter;

            return _importer.textureType == TextureImporterType.Sprite && _importer.spriteImportMode == SpriteImportMode.Multiple;
        }
    }

#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(WakakaBase))]
    public class ApronCreaterEditor : Editor
    {


        public override void OnInspectorGUI()
        {
            
            //Sticker = (GameObject)EditorGUILayout.ObjectField("Sticker", Sticker, typeof(GameObject), true);
            //WallStickerHangarNumber = (Texture2D)EditorGUILayout.ObjectField("Wall Sticker Hangar Number", WallStickerHangarNumber, typeof(Texture2D), true);
            //GroundStickerHangarNumber = (Texture2D)EditorGUILayout.ObjectField("Ground Sticker Hangar Number", GroundStickerHangarNumber, typeof(Texture2D), true);
            //GroundStickerKocmocraftName = (Texture2D)EditorGUILayout.ObjectField("Ground Sticker Kocmocraft Name", GroundStickerKocmocraftName, typeof(Texture2D), true);

            var scripts = targets.OfType<WakakaBase>();
            if (GUILayout.Button("Create"))
            {
                foreach (var script in scripts)
                    script.Create();
            }
            DrawDefaultInspector();
        }
    }
#endif
}