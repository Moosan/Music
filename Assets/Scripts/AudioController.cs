using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController :MonoBehaviour{
    [SerializeField]
    private GameObject obj;
    private AudioSource audioSorce {get; set;}
    private string deviceName {get{return deviceNameFunc();}}

    private float[] _waveData=new float[1024];
    private float[] _spectrumnData = new float[8192];

    [SerializeField,Range(7000f,10000f)]
    private int LowPassFilter;

    [SerializeField, Range(10, 1000)]
    private int HighPassFilter;

    [SerializeField, Range(0, 1)]
    private float Threshold;

    [SerializeField, Range(0, 32)]
    public int Count;
    
    void Start() {
        
        audioSorce = GetComponent<AudioSource>();
        var chanels = audioSorce.clip.channels;
        Debug.Log(chanels);
        audioSorce.loop = true;
        /*Debug.Log(deviceName);
        audioSorce.clip = Microphone.Start(deviceName,true,10,44100);
        while (Microphone.GetPosition(null)<=0) { }*/
        audioSorce.Play();

        //channelOneData = new float[1];
    }

    //*
    private float[] channelOneData;
    [SerializeField]
    private PlayMode mode;
    void OnAudioFilterRead(float[] data, int channels) {
        if (channels != 2) {
            Debug.Log("ステレオじゃないです");
        }
        if (mode == PlayMode.defalt) return;
        var length = data.Length;
        float[] newData = new float[length/2];
        for (int i = 0; i < length/2; i++) {
            newData[i] = data[2*i] - data[2*i + 1];

            if (mode == PlayMode.MelodyOnly)
            {
                var dat= data[2 * i] + data[2 * i + 1];
                var dat2= dat - newData[i];
                data[2 * i] = dat2; 
                data[2 * i + 1] = dat2;
            }
            else
            {
                data[2 * i] = newData[i];
                data[2 * i + 1] = newData[i];
            }
        }
    }
    //*/

    void Update() {
        audioSorce.GetOutputData(_waveData,1);
        audioSorce.GetSpectrumData(_spectrumnData, 1,FFTWindow.BlackmanHarris);
        //Debug.Log(GetVolumeOfWave(_waveData));
        obj.transform.localScale = Vector3.one * GetVolumeOfWave(_waveData);
        var data = GetSpectrum(_spectrumnData, Threshold,HighPassFilter,LowPassFilter).OrderByDescending(pair=>pair.Value).ToArray();
        string debugw = "";
        if (data.Length > Count)
        {
            for (int i = 0; i < Count; i++)
            {
                debugw += data[i].Key.name+":";
            }
        }
        else
        {
            foreach (var w in data)
            {
                debugw += w.Key.name + " : ";
            }
        }
        if (debugw == "")
        {
            Debug.Log("スペクトルがありません");
        }
        else
        {
            Debug.Log(debugw);
        }
    }

    private string deviceNameFunc()
    {
        string[] devices = Microphone.devices;
        return devices[0];
    }

    private static float GetVolumeOfWave(float[] waveData) {
        return 100*waveData.Select(x => x * x).Sum() / waveData.Length;
    }

    private static Dictionary<Onkai,float> GetSpectrum(float[] waveData,float threshold,int highPass,int lowPass) {
        Dictionary<Onkai, float> spectrumDatas = new Dictionary<Onkai, float>();
        var F = AudioSettings.outputSampleRate;
        var Q = waveData.Length;
        Onkai tyotomaenoonkai=new Onkai(highPass);
        bool saisyo = false;
        Onkai saisyono=new Onkai(highPass);
        for(int i = 0; i < waveData.Length; i++)
        {
            float spectrumW = i * F / (2.0f * Q);
            if (spectrumW < highPass) continue;
            if (spectrumW > lowPass) continue;
            var spectrumData = Mathf.Abs(waveData[i]);
            Onkai onkai = new Onkai(spectrumW);

            if (!saisyo)
            {
                saisyono = onkai;
                saisyo = true;
            }
            if (spectrumDatas.Count > 1)
            {
                if (onkai.onka == tyotomaenoonkai.onka && onkai.kens == tyotomaenoonkai.kens)
                {
                    spectrumDatas[tyotomaenoonkai] += spectrumData;
                }
                else
                {
                    if (spectrumDatas[tyotomaenoonkai] < threshold)
                    {
                        bool a = spectrumDatas.Remove(tyotomaenoonkai);
                        if (!a)
                        {
                            Debug.LogError("指定されたデータはありません:" + tyotomaenoonkai.name);
                        }
                    }
                    else
                    {
                        //Debug.Log(tyotomaenoonkai.frequency + "のとき" + spectrumDatas[tyotomaenoonkai]);
                    }
                    spectrumDatas[onkai] = spectrumData;
                    tyotomaenoonkai = onkai;
                }
            }
            else
            {
                spectrumDatas[onkai] = spectrumData;
                tyotomaenoonkai = onkai;
            }
        }

        spectrumDatas.Remove(tyotomaenoonkai);
        spectrumDatas.Remove(saisyono);
        return spectrumDatas;
    }
    
}
public enum PlayMode {
defalt,MelodyOnly,WithoutMelody
}