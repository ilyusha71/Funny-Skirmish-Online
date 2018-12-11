using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum HangarState
{
    Ready,
    Moving,
    Login,
    Lobby,
    Hangar,
    Show,
    newEvent,
}
public class HangarRanger : MonoBehaviour
{
    public Transform[] hangarCenter;
    private Transform hangarCamera;
    private Transform hangarRailX;
    private Transform hangarRailY;
    private int now = 0;
    public HangarState hangarState = HangarState.Ready;
    void Awake ()
    {      
        hangarCamera = transform;
        hangarRailX = hangarCamera.parent.parent;
        hangarRailY = hangarRailX.parent;

        hangarState = HangarState.Moving;
        hangarRailY.DOMove(hangarCenter[now].position, 0.73f).OnComplete(() => { hangarState = HangarState.Ready; });
        //hangarRailY.DORotateQuaternion(hangarCenter[now].rotation, 0.73f);
    }	

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.D))
            NextHanger();
        else if (Input.GetKeyDown(KeyCode.A))
            PreviousHanger();

        if (hangarState == HangarState.Ready)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                hangarRailY.rotation *= Quaternion.Euler(0,Input.GetAxis("Mouse X")*2,0);
                hangarRailX.rotation *= Quaternion.Euler(-Input.GetAxis("Mouse Y")*2, 0,0);
                hangarRailX.eulerAngles = new Vector3(Mathf.Clamp(hangarRailX.rotation.eulerAngles.x,60, 120), hangarRailX.rotation.eulerAngles.y, hangarRailX.rotation.eulerAngles.z);
            }
            hangarCamera.localPosition = Vector3.Lerp(hangarCamera.localPosition, hangarCamera.localPosition + new Vector3(0, 0, 10 * Input.GetAxis("Mouse ScrollWheel")), 0.5f);
            hangarCamera.localPosition = new Vector3(hangarCamera.localPosition.x, hangarCamera.localPosition.y, Mathf.Clamp(hangarCamera.localPosition.z, -20, -5));
        }
    }

    void NextHanger()
    {
        now = (int)Mathf.Repeat(++now,hangarCenter.Length);
        hangarRailY.DOKill();
        hangarState = HangarState.Moving;
        hangarRailY.DOMove(hangarCenter[now].position, 0.73f).OnComplete(()=>{ hangarState = HangarState.Ready; });
        //hangarRailY.DORotateQuaternion(hangarCenter[now].rotation, 0.73f);
    }

    void PreviousHanger()
    {
        now = (int)Mathf.Repeat(--now, hangarCenter.Length);
        hangarRailY.DOKill();
        hangarState = HangarState.Moving;
        hangarRailY.DOMove(hangarCenter[now].position, 0.73f).OnComplete(() => { hangarState = HangarState.Ready; });
        //hangarRailY.DORotateQuaternion(hangarCenter[now].rotation, 0.73f);
    }
}
