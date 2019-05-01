using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Kocmoca
{
    public partial class HangarRanger : CameraTrackingSystem
    {
        [System.Serializable]
        public class Turret : DataBlock
        {
            public TextMeshProUGUI[] item;
            public TextMeshProUGUI textTurretCount;
            public TextMeshProUGUI textMaxAutoAimAngle;
            public TextMeshProUGUI textRoundsPerMinute;
            public TextMeshProUGUI textMaxProjectileSpread;
            public TextMeshProUGUI textAmmoVelocity;
            public TextMeshProUGUI textOperationalRange;
            public TextMeshProUGUI textDamage;
            public TextMeshProUGUI textDPS;
            public TextMeshProUGUI textShieldPenetration;
            public TextMeshProUGUI textHullPenetration;

            public void SetData(int index)
            {
                moduleData = KocmocaData.KocmocraftData[index];
                textTitle.text = moduleData.TurretName;
                textTurretCount.text = moduleData.TurretCount.ToString() + "管"  ;
                textMaxAutoAimAngle.text = moduleData.MaxAutoAimAngle.ToString() + " 度";
                textRoundsPerMinute.text = moduleData.RoundsPerMinute.ToString() + " rpm";
                textMaxProjectileSpread.text = moduleData.MaxProjectileSpread.ToString() + " 度";
                textAmmoVelocity.text = Mathf.RoundToInt(moduleData.AmmoVelocity).ToString() + " mps" ;
                textOperationalRange.text = Mathf.RoundToInt(moduleData.operationalRange).ToString() + " m";
                textDamage.text = moduleData.Damage.ToString();
                textDPS.text = moduleData.DPS.ToString();
                textShieldPenetration.text = moduleData.ShieldPenetration.ToString() + "%";
                textHullPenetration.text = moduleData.HullPenetration.ToString() + "%";

                textTurretCount.color = HangarData.TextColor[index];
                textMaxAutoAimAngle.color = HangarData.TextColor[index];
                textRoundsPerMinute.color = HangarData.TextColor[index];
                textMaxProjectileSpread.color = HangarData.TextColor[index];
                textAmmoVelocity.color = HangarData.TextColor[index];
                textOperationalRange.color = HangarData.TextColor[index];
                textDamage.color = HangarData.TextColor[index];
                textDPS.color = HangarData.TextColor[index];

                for (int i = 0; i < item.Length; i++)
                {
                    item[i].color = HangarData.TextColor[index];
                }
            }
        }




        void LoadHangarData()
        {
            astromechDroid.SetData(hangarIndex);
            radar.SetData(hangarIndex);
            turret.SetData(hangarIndex);





            frameMain.color = HangarData.FrameColor[hangarIndex];
            frameClose.color = HangarData.ButtonColor[hangarIndex];
            // Tab
            tabDesign.color = HangarData.TabColor[hangarIndex];
            tabDubi.color = HangarData.TabColor[hangarIndex];
            tabPerformance.color = HangarData.TabColor[hangarIndex];
            tabAstromech.color = HangarData.TabColor[hangarIndex];
            tabRadar.color = HangarData.TabColor[hangarIndex];
            tabTurret.color = HangarData.TabColor[hangarIndex];
            tabMissile.color = HangarData.TabColor[hangarIndex];
            tabTextDesign.color = HangarData.TabColor[hangarIndex];
            tabTextDubi.color = HangarData.TabColor[hangarIndex];
            tabTextPerformance.color = HangarData.TabColor[hangarIndex];
            tabTextAstromech.color = HangarData.TabColor[hangarIndex];
            tabTextRadar.color = HangarData.TabColor[hangarIndex];
            tabTextTurret.color = HangarData.TabColor[hangarIndex];
            tabTextMissile.color = HangarData.TabColor[hangarIndex];



            textKocmocraftName.text = "" + DesignData.Kocmocraft[hangarIndex];
            textDubiName.text = "" + DesignData.Dubi[hangarIndex];

            textInfo.color = HangarData.TextColor[hangarIndex];
            textInfo.text = HangarData.Info[hangarIndex];

            textOKB.text = "" + DesignData.OKB[hangarIndex];
            textKocmocraft.text = "" + DesignData.Kocmocraft[hangarIndex];
            textCode.text = "" + DesignData.Code[hangarIndex];
            textDubi.text = "" + DesignData.Dubi[hangarIndex];
            textEngine.text = "" + DesignData.Engine[hangarIndex];

            textMaxHull.text = "" + KocmocraftData.Hull[hangarIndex];
            barHull.SetBar(KocmocraftData.Hull[hangarIndex]);
            textMaxShield.text = "" + KocmocraftData.Shield[hangarIndex];
            barShield.SetBar(KocmocraftData.Shield[hangarIndex]);
            textMaxEnergy.text = "" + KocmocraftData.Energy[hangarIndex];
            barEnergy.SetBar(KocmocraftData.Energy[hangarIndex]);
            textCruiseSpeed.text = (KocmocraftData.CruiseSpeed[hangarIndex] * 1.9438445f).ToString("0.00") + " knot";
            barCruise.SetBar(KocmocraftData.CruiseSpeed[hangarIndex]);
            textAfterburneSpeed.text = (KocmocraftData.AfterburnerSpeed[hangarIndex] * 1.9438445f).ToString("0.00") + " knot";
            barAfterburne.SetBar(KocmocraftData.AfterburnerSpeed[hangarIndex]);

            if (hangarIndex < hangarCount)
            {
                //WeaponData.GetWeaponData(hangarIndex);
                //textTurretCount.text = WeaponData.TurretCount[hangarIndex] + "x 突击激光炮";
                //textFireRPS.text = KocmoLaserCannon.FireRoundPerSecond + " rps";
                //textDamage.text = string.Format("{0} ~ {1} dmg", WeaponData.MinDamage, WeaponData.MaxDamage);
                //barDamage.SetBar(WeaponData.MaxDamage);
                //textMaxRange.text = WeaponData.MaxRange + " m";
                //barMaxRange.SetBar(WeaponData.MaxRange);
            }
            else
            {
                //textTurretCount.text = "---";
                //textFireRPS.text = "---";
                //textDamage.text = "---";
                //barDamage.SetBar(0);
                //textMaxRange.text = "--- m";
                //barMaxRange.SetBar(0);
            }

            // 三视图
            if (hangarIndex >= hangarCount) return;
            float wingspan = kocmocraftSize[hangarIndex].size.x;
            float length = kocmocraftSize[hangarIndex].size.z;
            float height = kocmocraftSize[hangarIndex].size.y;
            float max = Mathf.Max(wingspan, length);
            max = Mathf.Max(max, height);
            float maxSize = max * 0.5f;
            cameraTop.orthographicSize = maxSize;
            cameraSide.orthographicSize = maxSize;
            cameraFront.orthographicSize = maxSize;
            scaleWingspan.localScale = new Vector3(wingspan / max, 1, 1);
            scaleLength.localScale = new Vector3(length / max, 1, 1);
            scaleHeight.localScale = new Vector3(height / max, 1, 1);
            textWingspan.text = wingspan.ToString() + " m";
            textLength.text = length.ToString() + " m";
            textHeight.text = height.ToString() + " m";
        }

    }
}
