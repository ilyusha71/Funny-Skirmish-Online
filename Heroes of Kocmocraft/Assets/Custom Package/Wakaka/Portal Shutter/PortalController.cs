/***************************************************************************
 * Portal Controller
 * 傳送門
 * Last Updated: 2018/12/27
 * Description:
 * 1. Shutter Controller -> Portal Controller
 * 2. Opening開場，共計2.74秒
 *      a. Loading Bar 1.00 秒
 *      c. 快門開 0.37 秒
 *      d. 等待 0.37 秒
 *      e. 離開傳送門 1.00 秒
 * 2. Ending，共計1.74秒
 *      a. 返回傳送門 1.0 秒
 *      b. 快門關 0.37 秒
 *      c. 快門開 0.37 秒
 ***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MK.Glow;

public class PortalController : MonoBehaviour
{
    public delegate void EventHandler();
    public event EventHandler OnShutterPressedUp;
    private AudioSource sfx;
    public Transform viewPlane;
    public Image bar;
    [Header("Leaves")]
    public CanvasGroup leaves;
    public Image[] blades;
    private int countBlade;
    public AudioClip sfxOpen;
    public AudioClip sfxClose;
    private float openPosition;
    private float closePosition;
    [Header("Animation")]
    public Animation Portal;
    public Animation PrimaryRing;
    public Animation SecondaryRing;
    private string clipName;
    // AddOn
    //private MKGlowFree mkVFX;
    private float glowIntensity;
    void Awake()
    {
        sfx = GetComponent<AudioSource>();
        leaves.alpha = 0.77f;
        countBlade = blades.Length;
        closePosition  = blades[0].transform.localPosition.x;
        openPosition = closePosition - 300;
        clipName = Portal.clip.name;
        Portal[clipName].speed = 0.7f;
        PrimaryRing[clipName].speed = 0.1f;
        SecondaryRing[clipName].speed = -0.3f;
        //mkVFX = transform.parent.GetComponent<MKGlowFree>();
        //glowIntensity = mkVFX.GlowIntensityInner;
        //mkVFX.GlowIntensityInner = 0;
    }

    private void Start()
    {
        viewPlane.gameObject.SetActive(true);
        bar.fillAmount = 1.0f;
        bar.DOFillAmount(0, 1.0f).OnComplete(Opening);
    }

    public void Opening()
    {
        sfx.PlayOneShot(sfxOpen);
        for (int i = 0; i < countBlade; i++)
        {
            blades[i].transform.DOLocalMoveX(openPosition, 0.37f);
        }
        leaves.DOFade(0.5f,0.37f).OnComplete(() =>
        {
            Portal.gameObject.SetActive(false);
            PrimaryRing.Stop();
            SecondaryRing.Stop();
            Invoke("EnterScene", 0.37f);
        });
    }

    void EnterScene()
    {
        viewPlane.DOLocalMoveZ(-1000, 1.0f).OnComplete(()=> 
        {
            bar.enabled = false;
            Portal.transform.localRotation = Quaternion.identity;
            PrimaryRing.transform.localRotation = Quaternion.identity;
            SecondaryRing.transform.localRotation = Quaternion.identity;
            OnShutterPressedUp();
            //mkVFX.GlowIntensityInner = glowIntensity;
        });
    }

    public void Ending()
    {
        //mkVFX.GlowIntensityInner = 0;
        viewPlane.DOLocalMoveZ(0, 1.0f).OnComplete(() =>
        {
            sfx.PlayOneShot(sfxClose);
            for (int i = 0; i < countBlade; i++)
            {
                int index = i;
                blades[index].transform.DOLocalMoveX(closePosition, 0.37f);
            }
            leaves.DOFade(0.77f, 0.37f).OnComplete(() =>
            {
                Portal.gameObject.SetActive(true);
                Portal.Stop();
            });
        });
    }
}
