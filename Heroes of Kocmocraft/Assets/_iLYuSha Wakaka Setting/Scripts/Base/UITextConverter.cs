using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UITextConverter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textTitle;
    public string en;
    public string cn;

    void Reset()
    {
        textTitle = GetComponent<TextMeshProUGUI>();
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        textTitle.text = cn;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        textTitle.text = en;
    }
}