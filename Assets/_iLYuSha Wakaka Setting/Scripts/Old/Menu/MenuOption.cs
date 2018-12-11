using UnityEngine;
using UnityEngine.UI;

public abstract class MenuOption : MonoBehaviour
{
    internal Toggle[] toggleOption;
    internal int countOption;
    internal int nowOption;
    internal abstract void NextOption();
    internal abstract void PreviousOption();
    internal abstract void SwitchOption();
}
