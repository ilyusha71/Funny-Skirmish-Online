using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Kocmoca
{
    public class AnimationTween : MonoBehaviour
    {
        public Transform main;
        public Transform online;
        public CanvasGroup top;
        public Transform icon;
        private Vector3 posIcon;

        public void Awake()
        {
            main.localScale = new Vector3(7, 7, 1);
            online.localScale = Vector3.zero;
            top.alpha = 0;
            posIcon = icon.localPosition;
            icon.localPosition += new Vector3(0, Screen.width, 0);
        }
        public IEnumerator Play()
        {
            main.DOScale(1, 0.37f);
            yield return new WaitForSeconds(0.37f);
            online.DOScale(1, 0.73f);
            yield return new WaitForSeconds(0.37f);
            top.DOFade(1, 1.0f);
            yield return new WaitForSeconds(0.37f);
            icon.DOLocalMove(posIcon, 1.5f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.37f);
            transform.parent.GetComponent<GalaxyLobbyPanel>().PlayerNameInput.gameObject.SetActive(true);
        }
    }
}
