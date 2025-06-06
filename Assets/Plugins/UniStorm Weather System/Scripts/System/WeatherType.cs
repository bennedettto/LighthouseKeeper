﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniStorm
{
    [CreateAssetMenu(fileName = "New Weather Type", menuName = "UniStorm/New Weather Type")]
    public class WeatherType : ScriptableObject
    {
        public string WeatherTypeName = "New Weather Type";
        public CloudProfile CloudProfileComponent;

        public float SunIntensity = 1;
        public float CloudShadowIntensity = 1;
        public float MoonIntensity = 1;

        //Fog
        public int MinimumFogLevel = 300;
        public int MaximumFogLevel = 600;
        public float FogDensity = 0.0015f;
        public float FogLightFalloff = 0.65f;
        public float FogHeight = 0.73f;
        public float CameraFogHeight = 0.85f;
        public Yes_No OverrideFogColor = Yes_No.No;
        public Yes_No OverrideCameraFogHeight = Yes_No.No;
        public Gradient FogColor;

        //Wind
        public float WindSpeedLevel = 0.85f;
        public float WindBendingLevel = 0.2f;
        public float WindIntensity = 0.25f;

        //Audio
        public float WeatherVolume = 1;
        public AudioClip WeatherSound;

        //Particle Effect
        public Yes_No PrecipitationWeatherType = Yes_No.No;
        public Yes_No UseWeatherSound = Yes_No.No;

        public SeasonEnum Season;
        public enum SeasonEnum
        {
            All = 0,
            Spring = 1,
            Summer = 2,
            Fall = 3,
            Winter = 4
        }

        public int CloudHeight = 1000;
        public CloudLevelEnum CloudLevel;
        public enum CloudLevelEnum
        {
            Clear = 0,
            MostlyClear = 1,
            PartyCloudy = 2,
            MostlyCloudy = 3,
            Cloudy = 4,
            DontChange = 5
        }

        public TemperatureTypeEnum TemperatureType = TemperatureTypeEnum.AboveFreezing;
        public enum TemperatureTypeEnum
        {
            BelowFreezing = 0,
            AboveFreezing = 1,
            Both = 2
        }

        public ShaderControlEnum ShaderControl = ShaderControlEnum.None;
        public enum ShaderControlEnum
        {
            Rain = 0,
            Snow = 1,
            None = 2
        }

        public Yes_No OverrideCloudColor = Yes_No.No;
        public Gradient CloudColor;
        public Yes_No UseLightning = Yes_No.No;
        public Yes_No CustomizeWeatherIcon = Yes_No.No;
        public Yes_No WaitForCloudLevel = Yes_No.No;
        public Texture WeatherIcon;
        public enum Yes_No
        {
            Yes = 0,
            No = 1
        }

        public void CreateWeatherSound()
        {
            UniStormSystem UniStormSystemObject = FindAnyObjectByType<UniStormSystem>();
            if (UniStormSystemObject.enabled)
            {
                GameObject Temp = new GameObject();
                Temp.AddComponent<AudioSource>();
                AudioSource _AS = Temp.GetComponent<AudioSource>();
                _AS.clip = WeatherSound;
                _AS.volume = 0;
                _AS.loop = true;
                UnityEngine.Audio.AudioMixer m_AudioMixer = Resources.Load("UniStorm Audio Mixer") as UnityEngine.Audio.AudioMixer;
                _AS.outputAudioMixerGroup = m_AudioMixer.FindMatchingGroups("Master/Weather")[0];
                Temp.name = WeatherTypeName + " (UniStorm)";
                Temp.transform.SetParent(GameObject.Find("UniStorm Sounds").transform);
                Temp.transform.position = new Vector3(Temp.transform.parent.position.x, Temp.transform.parent.position.y, Temp.transform.parent.position.z);
                UniStormSystemObject.WeatherSoundsList.Add(_AS);
            }
        }
    }
}