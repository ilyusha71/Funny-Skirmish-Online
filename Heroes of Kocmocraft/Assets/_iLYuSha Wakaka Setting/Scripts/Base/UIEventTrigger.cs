using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource audioSource;
    public CanvasGroup target;

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.Play();
        target.alpha = 1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        target.alpha = 0;
    }
}
