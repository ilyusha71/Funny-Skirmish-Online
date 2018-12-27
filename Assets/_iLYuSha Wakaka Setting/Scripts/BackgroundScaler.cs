/***************************************************************************
 * Background Scaler
 * 適用於UGUI的螢幕自適應
 * Last Updated: 2018/12/27
 * Description:
 * UGUI的CanvasScaler提供matchWidthOrHeight的參數進行UI自適應的權重分配
 * 然而玩家的螢幕比例有可能比開發環境寬或窄，本腳本即為進階的自適應功能
 * 此腳本將自動修正matchWidthOrHeight為固定高度
 * 注意：各UI比例的調整不適用於背景圖
 ***************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public enum Scaler
{
    FixedWidth,
    FixedHeight,
}
public class BackgroundScaler : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    public Scaler scaler;
    private RectTransform background;
    float defaultRatio;
    float screenRatio;

    void Awake()
    {
        background = GetComponent<RectTransform>();
    }

    void Start()
    {
        canvasScaler.matchWidthOrHeight = 1;
        defaultRatio = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;
        screenRatio = Screen.width / (float)Screen.height;

        if (defaultRatio > screenRatio) // 1920 x 1080 => 1440 x 900 
        {
            // 1920 x 1080 => 1440 x 900 
            // 玩家的螢幕比開發環境窄，預設為固定高度，背景圖將被吃掉左右一部分
            // 玩家的螢幕比開發環境窄，固定寬度後，背景圖將被吃掉上下一部分
            if (scaler == Scaler.FixedWidth)
                background.localScale *= defaultRatio / screenRatio;
        }
        // 1920 x 1080 => 2560 x 1080
        // 玩家的螢幕比開發環境寬，若背景圖要填滿畫面，只能固定寬度，因此調整比例後，背景圖將被吃掉上下一部分
        else
            background.localScale *= screenRatio / defaultRatio; 
    }
}
