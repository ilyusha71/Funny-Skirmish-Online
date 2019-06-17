using UnityEngine;
using System.Linq;
using Cinemachine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kocmoca
{
    public partial class WakakaBase : MonoBehaviour
    {
        [Header("Database")]
        public KocmocraftDatabase database;
        [Header("Scene - Airport")]
        public Transform airport;
        public Transform[] apron, hangar;
        public Transform apronView, hangarView;
        public int hangarCount;
        [Header("Scene - Hangar Parameter")]
        public Prototype[] prototype;
        public PilotManager[] pilot;
        public CinemachineFreeLook[] cmFreeLook;
        public CinemachineVirtualCamera[] cmCockpit;

        [Header("Resources - Signboard")]
        public GameObject Signboard;
        public Material[] OKB;
        public GameObject[] signboards;
        [Header("Resources - Sticker")]
        public GameObject Sticker;
        public Texture2D StickerHangarNumber;
        public Texture2D StickerKocmocraftName;
        private Sprite[] spritesHangarNumber;
        private Sprite[] spritesKocmocraftName;
        public GameObject[] stickers;

        public void Create()
        {
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
            hangarCount = hangar.Length;
            prototype = new Prototype[hangarCount];
            pilot = new PilotManager[hangarCount];
            cmFreeLook = new CinemachineFreeLook[hangarCount];
            cmCockpit = new CinemachineVirtualCamera[hangarCount];
            radioClips = new AudioClip[hangarCount];
            for (int i = 0; i < countApron; i++)
            {
                apron[i].localPosition = new Vector3(630 - (i % 12 / 3) * 360 - i % 3 * 90, 0, 0);
                signboards[i] = Instantiate(Signboard, apron[i]);
                signboards[i].GetComponentsInChildren<MeshRenderer>()[0].material = OKB[i];
                signboards[i].GetComponentsInChildren<MeshRenderer>()[1].material = OKB[i];
                signboards[i].GetComponentsInChildren<MeshRenderer>()[2].material = OKB[i];

                stickers[i] = Instantiate(Sticker, apron[i]);
                stickers[i].GetComponentsInChildren<SpriteRenderer>()[0].sprite = spritesHangarNumber[i];
                stickers[i].GetComponentsInChildren<SpriteRenderer>()[1].sprite = spritesHangarNumber[i];
                stickers[i].GetComponentsInChildren<SpriteRenderer>()[2].sprite = spritesKocmocraftName[i];

                if (i < hangarCount)
                {
                    hangar[i].localPosition = new Vector3(630 - (i % 12 / 3) * 360 - i % 3 * 90, hangar[i].GetComponentInChildren<BoxCollider>().size.y * 0.5f + 2, 0);
                    prototype[i] = hangar[i].GetComponentInChildren<Prototype>();
                    pilot[i] = hangar[i].GetComponentInChildren<PilotManager>();
                    cmFreeLook[i] = prototype[i].cmFreeLook;
                    cmFreeLook[i].enabled = true;
                    cmFreeLook[i].m_Orbits[0].m_Height = database.kocmocraft[i].design.view.orthoSize + 3;
                    cmFreeLook[i].m_Orbits[2].m_Height = -database.kocmocraft[i].design.view.orthoSize;
                    cmFreeLook[i].m_Orbits[0].m_Radius = database.kocmocraft[i].design.view.near;
                    cmFreeLook[i].m_Orbits[1].m_Radius = 11;
                    cmFreeLook[i].m_Orbits[2].m_Radius = database.kocmocraft[i].design.view.near;
                    cmFreeLook[i].enabled = false;

                    cmCockpit[i] = hangar[i].GetComponent<AvionicsSystem>().cockpitView;
                    radioClips[i] = database.kocmocraft[i].radio;
                }
            }

            // topCamera = hangarView.GetComponentsInChildren<Camera>()[0];
            // sideCamera = hangarView.GetComponentsInChildren<Camera>()[1];
            // frontCamera = hangarView.GetComponentsInChildren<Camera>()[2];

            // UI
            var tipsBlock = panel.GetChild(0).gameObject;
            togTabs = audioTabPress.GetComponentsInChildren<Toggle>();
            panelCounts = togTabs.Length;
            panels = new RectTransform[panelCounts];
            for (int i = 0; i < panelCounts; i++)
            {
                var et = togTabs[i].GetComponent<UIEventTrigger>();
                et.audioSource = et.GetComponent<AudioSource>();
                et.target = et.GetComponentInChildren<CanvasGroup>();
                panels[i] = panel.GetChild(i + 1).GetComponent<RectTransform>();
            }
            designPanel.Initialize(panels[0],hangarView);
            pilotPanel.Initialize(panels[1], tipsBlock);
            performancePanel.Initialize(panels[2], tipsBlock);
            powerSystemPanel.Initialize(panels[3], tipsBlock);
            turretPanel.Initialize(panels[5], tipsBlock);
            kocmomechPanel.Initialize(panels[7], tipsBlock);

            var buttons = audioButtonPress.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                var et = buttons[i].GetComponent<UIEventTrigger>();
                et.audioSource = et.GetComponent<AudioSource>();
                et.target = et.GetComponentInChildren<CanvasGroup>();
            }
            btnChangePainting = buttons[5];
            btnSwitchWireframe = buttons[4];
            btnChangePilot = buttons[3];
            btnSwitchCockpitView = buttons[2]; // Hotkey: C
            btnRadio = buttons[1];
            radioAudio = btnRadio.GetComponentsInChildren<AudioSource>()[1];
            btnOption = buttons[0]; // Hotkey: ESC
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