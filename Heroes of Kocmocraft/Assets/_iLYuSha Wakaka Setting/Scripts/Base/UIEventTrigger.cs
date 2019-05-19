using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public AudioSource audioSource;
    public TextMeshProUGUI tootips;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        tootips = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.Play();
        tootips.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tootips.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
