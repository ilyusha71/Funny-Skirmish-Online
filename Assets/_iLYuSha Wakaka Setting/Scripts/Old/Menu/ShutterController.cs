using UnityEngine;
using UnityEngine.UI;

public class ShutterController : MonoBehaviour
{
    public delegate void EventHandler();
    public event EventHandler OnShutterPressedUp;
    public Transform shellOuter;
    public Transform shellInner;
    public Transform portalCore;
    private AudioSource sound;
    private Image[] leaves;
    private int countLeaves;
    private Vector3 iniShutterPos;
    private Vector3 minShutterPos;
    private float spdPortal = 120.0f;
    private float spdShell = 30.0f;
    private float speedShutter = 10.0f;
    private bool shot;

    void Awake()
    {
        sound = GetComponent<AudioSource>();
        leaves = GetComponentsInChildren<Image>();
        countLeaves = leaves.Length;
        iniShutterPos = leaves[0].transform.localPosition;
        minShutterPos = iniShutterPos + new Vector3(leaves[0].GetComponent<RectTransform>().rect.width * 2, 0, 0);
    }
    void Update()
    {
        if (shot)
        {
            spdShell = 180;
            for (int i = 0; i < countLeaves; i++)
            {
                leaves[i].transform.localPosition = Vector3.Lerp(leaves[i].transform.localPosition, minShutterPos, Time.deltaTime * speedShutter);
            }
            if (Vector3.Distance(leaves[0].transform.localPosition, minShutterPos) < 1)
            {
                shot = false;
                OnShutterPressedUp();
            }
        }
        else
        {
            spdShell = 30;
            for (int i = 0; i < countLeaves; i++)
            {
                leaves[i].transform.localPosition = Vector3.Lerp(leaves[i].transform.localPosition, iniShutterPos, Time.deltaTime * speedShutter);
            }
        }
        shellOuter.transform.rotation *= Quaternion.Euler(0, 0, spdShell * Time.deltaTime);
        shellInner.transform.rotation *= Quaternion.Euler(0, 0, -spdShell * Time.deltaTime);
        portalCore.transform.rotation *= Quaternion.Euler(0, 0, spdPortal * Time.deltaTime);
    }
    public void Shot()
    {
        shot = true;
        sound.Play();
    }
}
