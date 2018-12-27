/***************************************************************************
 * UI Scaler
 * 適用於UGUI的螢幕自適應
 * Last Updated: 2018/12/27
 * Description:
 * UGUI的CanvasScaler提供matchWidthOrHeight的參數，UI自適應的權重分配
 * 然而玩家的螢幕比例有可能比開發環境寬或窄，本腳本即為進階的自適應功能
 * 注意：各UI比例的調整不適用於背景圖，請參考另一腳本BackgroundScaler.cs
 ***************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class UIScaler : MonoBehaviour
{
    private CanvasScaler canvasScaler;
    float defaultRatio;
    float screenRatio;

    void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
    }

    void Start()
    {
        defaultRatio = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;
        screenRatio = Screen.width / (float)Screen.height;

        if (defaultRatio > screenRatio) // 1920 x 1080 => 1440 x 900 
            canvasScaler.matchWidthOrHeight = 0; // 玩家的螢幕比開發環境窄，因此固定寬度來調整比例
        else // 1920 x 1080 => 2560 x 10800 
            canvasScaler.matchWidthOrHeight = 1; // 玩家的螢幕比開發環境寬，因此固定高度來調整比例
    }
}
