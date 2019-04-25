using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fu.Media;
using System.IO;
using Fu;
namespace Fu.Ali.Speech
{
    public class TTSSample : MonoBehaviour
    {
        TTS tts;
        //[SerializeField] [TextArea] private string content;
        [SerializeField] private string apiKey;
        [SerializeField] private string token;
        [SerializeField] Text text;
        [SerializeField] Text resultPath;
        [SerializeField] InputField content;

        private AudioClip clip;
        private int index = 0;
        private AudioSource audioSource;
        private ChinarFileController fileController;

        // Use this for initialization
        void Start()
        {
            tts = new TTS(apiKey,token);
            audioSource = GetComponent<AudioSource>();
            fileController = GetComponent<ChinarFileController>();
            fileController.onSaveFile += WriteFile;
        }

        private void Play(TTSResponse response)
        {
            text.text = "合成成功";
            audioSource.clip = response.clip;
            clip = response.clip;
            audioSource.Play();
        }

        public void StartSynthesis()
        {
            text.text = "正在合成！";
            StartCoroutine(tts.Synthesis(content.text, Play));
        }

        public void Save()
        {
            fileController.SaveProject();
        }

        public void WriteFile(string filepath, string fileTitle)
        {
            index++;
            string filename = "audio" + index;
            SaveWav.Save(filepath, clip);
            resultPath.text = filepath;
        }

        private void OnGUI()
        {
            //if (GUILayout.Button("开始合成"))
            //{
            //    text.text = "正在合成！";
            //    StartCoroutine(tts.Synthesis(content.text, Play));
            //}
        }
    }

}
