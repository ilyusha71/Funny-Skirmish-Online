using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kocmoca;
using TMPro;
using System.Text;

public class TestText : MonoBehaviour
{
    ModuleData data = KocmocaData.Cuckoo;
    private string test;
    public TextMeshProUGUI wak;
    public int sdsdsd;

    public TextMeshProUGUI[] Awak;

    private void Start()
    {

        //sdsdsd= data.DpsHull;
        //stringX();
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 100; i++)
        {
            stringX();
            TEET();
            Arraa();
        }       
    }
    void stringX()
    {
        wak.color  = HangarData.TextColor[5];

        StringBuilder s = new StringBuilder();
        s.Append(test);
        s.Append("\n");
        s.Append(test);
        s.Append("\n");
        s.Append(test);
        s.Append("\n");
        s.Append(test);
        s.Append("\n");
        s.Append(test);
        s.Append("\n");


        s.Append(Mathf.RoundToInt(data.AmmoVelocity));
        s.Append(" mps");
        s.Append(data.DecayVelocity);
        s.Append("\n");

        s.Append(Mathf.RoundToInt(data.operationalRange));
        s.Append(" m");
        s.Append(data.DecayVelocity);
        s.Append("\n");

        //s.Append(data.PenetrationShield * 100);
        //s.Append(" %\n");
        //s.Append(data.PenetrationHull * 100);
        //s.Append(" %\n");

        //s.Append(data.DamageShield);
        //s.Append("\n");
        //s.Append(data.DamageHull);
        //s.Append("\n");
        //s.Append(data.DpsShield);
        //s.Append("\n");
        //s.Append(data.DpsHull);

        wak.text = s.ToString();
    }

    void TEET()
    {
        wak.color = HangarData.TextColor[5];
        //wak.text =
        //    test + "\n" +
        //    test + "\n" +
        //    test + "\n" +
        //    test + "\n" +
        //    test + "\n" +
        //    Mathf.RoundToInt(data.AmmoVelocity).ToString() + " mps" + data.DecayVelocity + "\n" +
        //    Mathf.RoundToInt(data.operationalRange).ToString() + " m" + data.DecayVelocity + "\n" +
        //    (data.PenetrationShield * 100).ToString() + " %\n" +
        //    (data.PenetrationHull * 100).ToString() + " %\n" +
        //    data.DamageShield + "\n" +
        //    data.DamageHull + "\n" +
        //    data.DpsShield + "\n" +
        //    data.DpsHull;
    }

    void Arraa()
    {
        Awak[0].color = HangarData.TextColor[5];
        Awak[1].color = HangarData.TextColor[3];
        Awak[2].color = HangarData.TextColor[5];
        Awak[3].color = HangarData.TextColor[9];
        Awak[4].color = HangarData.TextColor[5];
        Awak[5].color = HangarData.TextColor[8];
        Awak[6].color = HangarData.TextColor[5];
        Awak[7].color = HangarData.TextColor[2];
        Awak[8].color = HangarData.TextColor[5];
        Awak[9].color = HangarData.TextColor[0];
        Awak[10].color = HangarData.TextColor[5];
        Awak[11].color = HangarData.TextColor[11];
        Awak[12].color = HangarData.TextColor[4];


        Awak[0].text = test;
        Awak[1].text = test;
        Awak[2].text = test;
        Awak[3].text = test;
        Awak[4].text = test;


        //Awak[5].text = Mathf.RoundToInt(data.AmmoVelocity).ToString() + " mps" + data.DecayVelocity;
        //Awak[6].text = Mathf.RoundToInt(data.operationalRange).ToString() + " m" + data.DecayVelocity;
        //Awak[7].text = (data.PenetrationShield * 100).ToString() + " %";
        //Awak[8].text = (data.PenetrationHull * 100).ToString() + " %";
        //Awak[9].text = data.DamageShield;
        //Awak[10].text = data.DamageHull;
        //Awak[11].text = data.DpsShield;
        //Awak[12].text = data.DpsHull;
    }
}
