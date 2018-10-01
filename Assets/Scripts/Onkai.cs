using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onkai{
    public onkai onka { get; private set; }
    public kensu kens { get; private set; }
    public float frequency { get; private set; }
    public string name { get; set; }
    private const float baseOn= 427.474054109f;
    private const float R = 1.05946309436f;
    public Onkai(float frequency) {
        this.frequency = frequency;
        kens = FreqToKensu(frequency);
        onka = FreqToOnkai(frequency,kens);
        name = GetString(kens, onka);
    }
    private static kensu FreqToKensu(float freq) {
        int ks = 4;
        float bs = baseOn;
        bool ue;
        if (freq > bs)
        {
            ue = true;
        }
        else {
            ks--;
            ue = false;
        }
        while (ue) {
            bs *= 2;
            if (freq < bs) break;
            ks++;
            if (ks > 12)
            {
                Debug.LogError("大きすぎる周波数が出てます");
                break;
            }
        }
        while (!ue) {
            bs /= 2;
            if (freq > bs) break;
            ks--;

            if (ks < 0)
            {
                Debug.LogError("小さすぎる周波数が出てます");
                break;
            }
        }
        return IntToKensu(ks);
    }
    private static onkai FreqToOnkai(float freq,kensu ken) {
        var k = (int)ken;
        float bs;
        bs = baseOn * Mathf.Pow(2, (int)(k - 4));
        if (bs > freq){
            Debug.LogError(k + "謎"+":"+freq);
        }
        int intVal = 0;
        //Debug.Log("鍵数:"+k+" base音:"+bs+" 周波数:"+freq);
        while (true) {
            bs *= R;
            if (bs > freq) {
                break;
            }
            intVal++;

            if (intVal > 16) {
                Debug.LogError("音階をオーバーしています。");
                break;
            }
        }
        return IntToOnkai(intVal);
    }

    private static kensu IntToKensu(int intVal) {
        return (kensu)kensu.ToObject(typeof(kensu), intVal);
    }

    private static onkai IntToOnkai(int intVal)
    {
        return (onkai)onkai.ToObject(typeof(onkai), intVal);
    }

    private static string GetString(kensu kens,onkai onka)
    {
        string ke="ken"+(int)kens;
        string on;
        switch ((int)onka) {
            case 0:
                on = "a";
                break;
            case 1:
                on = "bf";
                break;
            case 2:
                on = "b";
                break;
            case 3:
                on = "c";
                break;
            case 4:
                on = "df";
                break;
            case 5:
                on = "d";
                break;
            case 6:
                on = "ef";
                break;
            case 7:
                on = "e";
                break;
            case 8:
                on = "f";
                break;
            case 9:
                on = "gf";
                break;
            case 10:
                on = "g";
                break;
            case 11:
                on = "af";
                break;
            default:
                on = "";
                Debug.LogError("音階が当てはまらねえぜ");
                break;
        }
        return ke+on;
    }

    public override string ToString()
    {
        return name;
    }

}
public enum onkai {
    a,bf,b,c,df,d,ef,e,f,gf,g,af
}
public enum kensu {
    ken0,ken1,ken2,ken3,ken4,ken5,ken6,ken7,ken8,ken9,ken10,ken11,ken12,ken13,ken14,ken15
    //440Hz(基調はken4のa)
}
