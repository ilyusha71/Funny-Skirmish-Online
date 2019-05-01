/***************************************************************************
 * KocmoLaser Flying
 * 卡斯摩激光飛行
 * Last Updated: 2018/10/11
 * Description:
 * 1. 繼承Ammo彈藥腳本，包含初始化與物件池管理
 * 2. 在PUN生成實例後，myRigidbody.AddForce在OnEnable執行會出現激光生成位置過前的問題
 *     透過Flying Initialize飛行初始化協程進行延後觸發
 ***************************************************************************/
using UnityEngine;

namespace Kocmoca
{
    public class BigLaserFlying : Ammo
    {
        public GameObject effect;
        public ObjectPoolData objPoolData;
        private TrailRenderer vfx;

    }
}