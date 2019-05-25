using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource audioSource;
    public CanvasGroup target;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.Play();
        target.alpha = 1;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        target.alpha = 0;
    }
}
