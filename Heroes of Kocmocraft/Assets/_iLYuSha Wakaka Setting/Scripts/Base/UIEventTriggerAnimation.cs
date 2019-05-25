
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTriggerAnimation : UIEventTrigger
{
    public Animation anim;
    public float period = 1.0f;
    private AnimationState state;

    private void Awake()
    {
        state = anim[anim.clip.name];
        state.speed = 0;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.Play();
        target.alpha = 1;
        state.speed = 1.0f / period;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        target.alpha = 0;
        state.speed = 0;
    }
}
