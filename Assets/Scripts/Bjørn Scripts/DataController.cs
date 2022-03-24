using UnityEngine;

namespace PlayerPreferences
{
    [CreateAssetMenu(menuName = "dataControllerScrub", fileName = "dataControllerScrub")]
    public class DataController : ScriptableObject
    {
        public int currentChannel;
        public float channelCount;
        public float channel1Start;
        public float channel2Start;
        public float channel1Length;
        public float channel2Length;
        public float channel1Time;
        public float channel2Time;
        public float hertz;

        public float masterVolume;
        public float sfxVolume;
        public float ambianceVolume;
        public float musicVolume;

        private void Awake()
        {
            GetPlayerData();
        }

        public void GetPlayerData()
        {
            currentChannel = PlayerPrefs.GetInt("currentChannel");
            channelCount = PlayerPrefs.GetFloat("channelCount");
            channel1Start = PlayerPrefs.GetFloat("channel1Start");
            channel2Start = PlayerPrefs.GetFloat("channel2Start");
            channel1Length = PlayerPrefs.GetFloat("channel1Length");
            channel2Length = PlayerPrefs.GetFloat("channel2Length");
            channel1Time = PlayerPrefs.GetFloat("channel1Time");
            channel2Time = PlayerPrefs.GetFloat("channel2Time");
            hertz = PlayerPrefs.GetFloat("hertz");
            masterVolume = PlayerPrefs.GetFloat("masterVolume");
            sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
            ambianceVolume = PlayerPrefs.GetFloat("ambianceVolume");
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
        }

        public void SetPlayerData()
        {
            PlayerPrefs.SetInt("currentChannel", currentChannel);
            PlayerPrefs.SetFloat("channelCount", channelCount);
            PlayerPrefs.SetFloat("channel1Start", channel1Start);
            PlayerPrefs.SetFloat("channel2Start", channel2Start);
            PlayerPrefs.SetFloat("channel1Length", channel1Length);
            PlayerPrefs.SetFloat("channel2Length", channel2Length);
            PlayerPrefs.SetFloat("channel1Time", channel1Time);
            PlayerPrefs.SetFloat("channel2Time", channel2Time);
            PlayerPrefs.SetFloat("hertz", hertz);
            PlayerPrefs.SetFloat("masterVolume", masterVolume);
            PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
            PlayerPrefs.SetFloat("ambianceVolume", ambianceVolume);
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
        }

        public void ErasePlayerData(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public void DestroyAllPlayerData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}