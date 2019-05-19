/***************************************************************************
 * Kocmocraft Manager
 * 宇航機管理員
 * Last Updated: 2019/05/18
 * 
 * v19.0518
 * 1. 原KocmocraftManager已升級為KocmocraftCommander，主要用於Photon網路連線控制
 * 2. 主要負責宇航機於機庫時的基本設定，以及相關模組的初始化
 ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoca
{
    public class KocmocraftManager : MonoBehaviour
    {
        private int type;
        private void Reset()
        {
            type = int.Parse(name.Split(new char[2] { '(', ')' })[1]);
            KocmocraftDatabase index = UnityEditor.AssetDatabase.LoadAssetAtPath<KocmocraftDatabase>("Assets/_iLYuSha Wakaka Setting/ScriptableObject/Kocmocraft Database.asset");

            FireControlSystem[] fcs = GetComponentsInChildren<FireControlSystem>();
            for (int i = 0; i < fcs.Length; i++)
            {
                fcs[i].Preset(type);
            }

            GetComponentInChildren<EngineController>().Preset(index.kocmocraft[type].engine);
        }
    }
}
