using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Kocmoca
{
    public enum HangarState
    {
        Portal = 0,
        Ready,
        Moving,
        Open,
        Hide,
    }
    public enum TweenerState
    {
        Ready,
        Moving,
        Open,
        Hide,
    }
    public struct SciFiBar
    {
        public Image[] bar;
        private int Denominator;
        private int Minimum;

        public void Initialize(Image[] shell, int min, int max)
        {
            bar = new Image[7];
            for (int i = 0; i < 7; i++)
            {
                bar[i] = shell[i + 1];
            }
            Denominator = max - min;
            Minimum = min;
        }

        public void SetBar(int value)
        {
            int Numerator = value - Minimum;
            float level = Numerator / (float)Denominator;
            for (int i = 0; i < 7; i++)
            {
                bar[i].enabled = true;
            }
            if (level < 0.8571428) bar[6].enabled = false;
            if (level < 0.7142857f) bar[5].enabled = false;
            if (level < 0.5714285f) bar[4].enabled = false;
            if (level < 0.4285714f) bar[3].enabled = false;
            if (level < 0.2857142f) bar[2].enabled = false;
            if (level < 0.1428571f) bar[1].enabled = false;
            if (level < 0.0100001f) bar[0].enabled = false;
        }
    }
    public class HangarRanger : MonoBehaviour
    {
        private readonly int hangarMaxCount = 24;
        private readonly int hangarHalfCount = 12;
        [Header("External Scripts")]
        public PortalController Portal;
        [Header("UI")]
        public CanvasGroup hangarCanvas;
        private AudioSource SFX;
        [Header("Hangar Rail & Camera")]
        public Transform hangarRailMain;
        public Transform hangarRailY;
        public Transform hangarRailX;
        public Transform hangarCamera;
        public Transform[] hangarCenter;
        public Camera cameraTop;
        public Camera cameraSide;
        public Camera cameraFront;
        private BoxCollider[] prototype;
        private HangarState hangarState = HangarState.Portal;
        private int hangarIndex;
        private int hangarMax = 18;
        [Header("Billboard")]
        public Transform billboard;
        private Vector3 billboardPos = new Vector3(-12.972f, 0, 9.289f);
        private Vector3 billboardHide = new Vector3(0, 1000, 0);
        [Header("Panel")]
        public Transform blockInfo;
        public Transform blockHangar;
        public Transform blockKocmocraft;
        public Transform blockData;
        public Button btnOpen;
        public Button btnHide;
        public AudioClip sfxOpen;
        public AudioClip sfxHide;
        private TweenerState panelState = TweenerState.Hide;
        private readonly WaitForSeconds delay = new WaitForSeconds(0.137f);
        [Header("Info")]
        public Image imageFrame;
        public Image imageButton;
        public RectTransform scaleWingspan;
        public RectTransform scaleLength;
        public RectTransform scaleHeight;
        public TextMeshProUGUI textWingspan;
        public TextMeshProUGUI textLength;
        public TextMeshProUGUI textHeight;
        public TextMeshProUGUI textInfo;
        [Header("Hangar Data")]
        public TextMeshProUGUI textOKB;
        public TextMeshProUGUI textKocmocraft;
        public TextMeshProUGUI textCode;
        public TextMeshProUGUI textDubi;
        public TextMeshProUGUI textEngine;
        [Header("Kocmocraft Data")]
        public TextMeshProUGUI textMaxHull;
        public TextMeshProUGUI textMaxShield;
        public TextMeshProUGUI textMaxEnergy;
        public TextMeshProUGUI textCruiseSpeed;
        public TextMeshProUGUI textAfterburneSpeed;
        private SciFiBar barHull;
        private SciFiBar barShield;
        private SciFiBar barEnergy;
        private SciFiBar barCruise;
        private SciFiBar barAfterburne;
        [Header("Weapon Data")]
        public TextMeshProUGUI textTurretCount;
        public TextMeshProUGUI textFireRPS;
        public TextMeshProUGUI textDamage;
        private SciFiBar barDamage;
        public TextMeshProUGUI textMaxRange;
        private SciFiBar barMaxRange;

        void Awake()
        {
            Portal.OnShutterPressedUp += EnterHangar;
            SFX = GetComponent<AudioSource>();
            prototype = new BoxCollider[hangarMaxCount];
            for (int i = 0; i < hangarMaxCount; i++) { prototype[i] = hangarCenter[i].GetComponentsInChildren<BoxCollider>()[1]; }
            hangarIndex = PlayerPrefs.GetInt(LobbyInfomation.PREFS_TYPE);
            billboard.localPosition = billboardHide;
            blockInfo.localScale = Vector3.zero;
            blockHangar.localPosition = new Vector3(blockHangar.localPosition.x, -128,0);
            blockKocmocraft.localPosition = new Vector3(blockKocmocraft.localPosition.x, -128, 0);
            blockData.localPosition = new Vector3(blockData.localPosition.x, -128, 0);
            btnOpen.onClick.AddListener(() => OpenPanel());
            btnHide.onClick.AddListener(() => HidePanel());
            barHull.Initialize(textMaxHull.transform.parent.GetComponentsInChildren<Image>(), 4000, 25000);
            barShield.Initialize(textMaxShield.transform.parent.GetComponentsInChildren<Image>(), 3000, 24000);
            barEnergy.Initialize(textMaxEnergy.transform.parent.GetComponentsInChildren<Image>(), 500, 3700);
            barCruise.Initialize(textCruiseSpeed.transform.parent.GetComponentsInChildren<Image>(), 20, 60);
            barAfterburne.Initialize(textAfterburneSpeed.transform.parent.GetComponentsInChildren<Image>(), 70, 133);
            barDamage.Initialize(textDamage.transform.parent.GetComponentsInChildren<Image>(), 1092, 2842);
            barMaxRange.Initialize(textMaxRange.transform.parent.GetComponentsInChildren<Image>(), 955, 1270);
        }

        private void EnterHangar()
        {
            Controller.controlMode = ControlMode.General;
            hangarState = HangarState.Moving;
            hangarRailY.DOLocalRotate(hangarIndex < hangarHalfCount ? new Vector3(0, 153, 0) : new Vector3(0, -153, 0), 3.37f);
            hangarRailX.DOLocalRotate(new Vector3(71, 0, 0), 3.37f);
            hangarCamera.DOLocalMove(new Vector3(0, 0, -13.7f), 3.37f);
            hangarRailMain.DORotateQuaternion(prototype[hangarIndex].transform.rotation, 3.37f);
            hangarRailMain.DOMove(prototype[hangarIndex].transform.position, 3.37f).OnComplete(() =>
            {
                hangarState = HangarState.Ready;
                billboard.localPosition = billboardPos;
                LoadHangarData();
            });
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                hangarState = HangarState.Portal;
                hangarCanvas.alpha = 0;
                Portal.Ending();
                Invoke("BackLobby", 2.0f);
            }

            if (hangarState == HangarState.Portal) return;

            if (Input.GetKeyDown(Controller.KEY_NextHangar))
            {
                hangarIndex = (int)Mathf.Repeat(++hangarIndex, hangarCenter.Length);
                MoveHangarRail();
            }
            else if (Input.GetKeyDown(Controller.KEY_PreviousHangar))
            {
                hangarIndex = (int)Mathf.Repeat(--hangarIndex, hangarCenter.Length);
                MoveHangarRail();
            }

            billboard.LookAt(hangarCamera);
            billboard.eulerAngles = new Vector3(0, billboard.eulerAngles.y, 0);

            if (hangarState == HangarState.Ready)
            {
                hangarCanvas.alpha = 1.0f;
                if (Input.GetKeyDown(Controller.KEYBOARD_Panel) || Input.GetKeyDown(Controller.XBOX360_Panel))
                {
                    if (panelState == TweenerState.Hide)
                        OpenPanel();
                    else if (panelState == TweenerState.Open)
                        HidePanel();
                    else
                        return;
                }
                   
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    hangarCanvas.alpha = 0;
                    hangarRailY.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * 2, 0);
                    hangarRailX.rotation *= Quaternion.Euler(-Input.GetAxis("Mouse Y") * 2, 0, 0);
                }
                hangarRailX.eulerAngles = new Vector3(Mathf.Clamp(hangarRailX.rotation.eulerAngles.x, 60, 120), hangarRailX.rotation.eulerAngles.y, hangarRailX.rotation.eulerAngles.z);
                hangarCamera.localPosition = Vector3.Lerp(hangarCamera.localPosition, hangarCamera.localPosition + new Vector3(0, 0, 10 * Input.GetAxis("Mouse ScrollWheel")), 0.5f);
                hangarCamera.localPosition = new Vector3(hangarCamera.localPosition.x, hangarCamera.localPosition.y, Mathf.Clamp(hangarCamera.localPosition.z, -20, -5));
            }
        }

        void MoveHangarRail()
        {
            billboard.localPosition = billboardHide;
            hangarCanvas.alpha = 0.0f;
            hangarRailMain.DOKill();
            hangarState = HangarState.Moving;
            hangarRailMain.DORotateQuaternion(prototype[hangarIndex].transform.rotation, 0.73f);
            hangarRailMain.DOMove(prototype[hangarIndex].transform.position, 0.73f).OnComplete(() =>
            {
                hangarState = HangarState.Ready;
                if (hangarIndex < 20)
                    PlayerPrefs.SetInt(LobbyInfomation.PREFS_TYPE, hangarIndex);
                billboard.localPosition = billboardPos;
                LoadHangarData();
            });
        }

        void LoadHangarData()
        {
            imageFrame.color = HangarData.FrameColor[hangarIndex];
            imageButton.color = HangarData.ButtonColor[hangarIndex];

            if (hangarIndex < hangarMax)
            {
                float wingspan = prototype[hangarIndex].size.x;
                float length = prototype[hangarIndex].size.z;
                float height = prototype[hangarIndex].size.y;
                float max = Mathf.Max(wingspan, length);
                max = Mathf.Max(max, height);
                float maxSize = max * 0.5f;
                cameraTop.orthographicSize = maxSize;
                cameraSide.orthographicSize = maxSize;
                cameraFront.orthographicSize = maxSize;
                scaleWingspan.localScale = new Vector3(wingspan / max,1,1);
                scaleLength.localScale = new Vector3(length / max, 1, 1);
                scaleHeight.localScale = new Vector3(height / max, 1, 1);
                textWingspan.text = wingspan + " m";
                textLength.text = length + " m";
                textHeight.text = height + " m";
            }
            
            textInfo.color = HangarData.TextColor[hangarIndex];
            textInfo.text = HangarData.Info[hangarIndex];

            textOKB.text = "" + HangarData.OKB[hangarIndex];
            textKocmocraft.text = "" + HangarData.Kocmocraft[hangarIndex];
            textCode.text = "" + HangarData.Code[hangarIndex];
            textDubi.text = "" + HangarData.Dubi[hangarIndex];
            textEngine.text = "" + HangarData.Engine[hangarIndex];

            textMaxHull.text = "" + KocmocraftData.MaxHull[hangarIndex];
            barHull.SetBar(KocmocraftData.MaxHull[hangarIndex]);
            textMaxShield.text = "" + KocmocraftData.MaxShieldl[hangarIndex];
            barShield.SetBar(KocmocraftData.MaxShieldl[hangarIndex]);
            textMaxEnergy.text = "" + KocmocraftData.MaxEnergy[hangarIndex];
            barEnergy.SetBar(KocmocraftData.MaxEnergy[hangarIndex]);
            textCruiseSpeed.text = (KocmocraftData.CruiseSpeed[hangarIndex] * 1.9438445f).ToString("0.00") + " knot";
            barCruise.SetBar(KocmocraftData.CruiseSpeed[hangarIndex]);
            textAfterburneSpeed.text = (KocmocraftData.AfterburnerSpeed[hangarIndex] * 1.9438445f).ToString("0.00") + " knot";
            barAfterburne.SetBar(KocmocraftData.AfterburnerSpeed[hangarIndex]);

            if (hangarIndex < 20)
            {
                WeaponData.GetWeaponData(hangarIndex);
                textTurretCount.text = WeaponData.TurretCount[hangarIndex] + "x 突击激光炮";
                textFireRPS.text = KocmoLaserCannon.fireRoundPerSecond + " rps";
                textDamage.text = string.Format("{0} ~ {1} dmg", WeaponData.MinDamage, WeaponData.MaxDamage);
                barDamage.SetBar(WeaponData.MaxDamage);
                textMaxRange.text = WeaponData.MaxRange + " m";
                barMaxRange.SetBar(WeaponData.MaxRange);
            }
            else
            {
                textTurretCount.text = "---";
                textFireRPS.text = "---";
                textDamage.text = "---";
                barDamage.SetBar(0);
                textMaxRange.text = "--- m";
                barMaxRange.SetBar(0);
            }

        }

        public void OpenPanel()
        {
            blockInfo.DOKill();
            blockHangar.DOKill();
            blockKocmocraft.DOKill();
            blockData.DOKill();
            panelState = TweenerState.Moving;
            SFX.PlayOneShot(sfxOpen);
            blockInfo.DOScale(Vector3.one,0.37f).OnComplete(() => 
            {
                StartCoroutine(Animation());
            });
        }

        public void HidePanel()
        {
            blockInfo.DOKill();
            blockHangar.DOKill();
            blockKocmocraft.DOKill();
            blockData.DOKill();
            panelState = TweenerState.Moving;
            SFX.PlayOneShot(sfxHide);
            blockInfo.DOScale(Vector3.zero, 0.37f);
            blockHangar.DOLocalMoveY(-128, 0.37f);
            blockKocmocraft.DOLocalMoveY(-128, 0.37f);
            blockData.DOLocalMoveY(-128, 0.37f).OnComplete(() => { panelState = TweenerState.Hide; });
        }

        IEnumerator Animation()
        {
            blockKocmocraft.DOLocalMoveY(128, 1.0f).SetEase(Ease.OutElastic);
            yield return delay;
            blockHangar.DOLocalMoveY(128, 1.0f).SetEase(Ease.OutElastic);
            yield return delay;
            blockData.DOLocalMoveY(128, 1.0f).SetEase(Ease.OutElastic).OnComplete(() => { panelState = TweenerState.Open; });
        }

        void BackLobby()
        {
            SceneManager.LoadScene(LobbyInfomation.SCENE_LOBBY);
        }
    }
}