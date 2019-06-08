using UnityEngine;

[CreateAssetMenu(fileName = "Engine Data", menuName = "KocmocA Data/Create Engine Data")]
[System.Serializable]
public class Engine : ScriptableObject
{
    public EngineType type;
    public string typeName;
    public string typeEN;
    public string typeCN;
    public Sprite icon;
    public int power;
    [Header("Engine Sound")]
    public AudioClip sound;
    public float maxVolume, minVolume, maxPitch, minPitch;
}
public enum EngineType
{
    RocketEngine, // 火箭发动机
    PulsejetEngine, // 脉冲发动机
    TurbojetEngine, // 涡轮喷射发动机
    TurbofanEngine, // 涡轮扇发动机
    IonEngine, // 离子发动机
    TurbopropEngine, // 涡轮螺旋桨发动机
    TurboshaftEngine, // 涡轮轴发动机
    KocmoFloater, //  宇航漂浮器
    ReverseThruster, // 反向推进器
    MechanicalWing, // 机械翼
}