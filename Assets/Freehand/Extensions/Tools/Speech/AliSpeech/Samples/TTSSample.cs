using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Freehand.Media;

namespace Freehand.Ali.Speech
{
    public class TTSSample : MonoBehaviour
    {
        const string ALI_CONFIG_LOAD_PATH = "/Speech/aliConfig.json";
        
        [SerializeField] Text _log;
        [SerializeField] InputField _content;
        [SerializeField] Scrollbar _scrollbar;

        [SerializeField] InputField _voice;
        [SerializeField] Slider _volume;
        [SerializeField] Text _volume_text;
        [SerializeField] Slider _speech_rate;
        [SerializeField] Text _speech_ratee_text;
        [SerializeField] Slider _pitch_rate;
        [SerializeField] Text _pitch_rate_text;
        [SerializeField] InputField _sample_rate;

        private AudioClip clip;
        private int index = 0;
        private AudioSource audioSource;
        private ChinarFileController fileController;

        private TTS _tts;

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            fileController = GetComponent<ChinarFileController>();
            fileController.onSaveFile += WriteFile;
            StartCoroutine(WaitForLoadAliConfigFile());
        }

        private void Update()
        {
            _volume_text.text = _volume.value.ToString();
            _speech_ratee_text.text = _speech_rate.value.ToString();
            _pitch_rate_text.text = _pitch_rate.value.ToString();
        }

        private IEnumerator WaitForLoadAliConfigFile()
        {
            Log("正在加载配置文件...");

            string path = Application.streamingAssetsPath + ALI_CONFIG_LOAD_PATH;

            WWW www = new WWW(path);

            yield return www;

            AliAIConfig config = JsonUtility.FromJson<AliAIConfig>(www.text);

            if(config != null)
            {
                _tts = new TTS(config);
                InitConfig();
                Log("Successfully : 加载配置文件成功...");
            }
            else
            {
                Log("Error : 加载配置文件失败 > " + path);
                Debug.LogError("error: " + path);
            }
        }

        private void InitConfig()
        {
            _voice.text = _tts.config.tts.voice;
            _volume.value = (int)_tts.config.tts.volume;
            _speech_rate.value =(int) _tts.config.tts.speech_rate;
            _pitch_rate.value = (int)_tts.config.tts.pitch_rate;
            _sample_rate.text = _tts.config.tts.sample_rate.ToString();
        }

        private void SynthesisCallback(TTSResponse response)
        {
            if(response == null)
            {
                Log("合成失败，请检查Token是否获取到>>" + _tts.config.tts.token);
                return;
            }
            if (response.Success)
            {
                audioSource.clip = response.clip;
                clip = response.clip;
                Log("合成语音成功");
            }

        }

        private void WriteFile(string filepath, string fileTitle)
        {
            index++;
            string filename = "audio" + index;
            SaveWav.Save(filepath, clip);
            Log("保存文件成功>>" + filepath);
        }

        public void StartSynthesis()
        {
            Log("正在合成语音文件");
            _tts.config.tts.voice = _voice.text;
            _tts.config.tts.volume = (int)_volume.value;
            _tts.config.tts.speech_rate = (int)_speech_rate.value;
            _tts.config.tts.pitch_rate = (int)_pitch_rate.value;
            _tts.config.tts.sample_rate = int.Parse(_sample_rate.text);
            StartCoroutine(_tts.Synthesis(_content.text, SynthesisCallback));
        }

        public void Play()
        {
            audioSource.Play();
        }

        public void Save()
        {
            fileController.SaveProject();
        }

        public void Log(string text)
        {
            if(_log.text == "")
            {
                _log.text =  text;
            }
            else
            {
                _log.text += "\n" + text;
            }
            StartCoroutine(SetScrollbar());
        }

        IEnumerator SetScrollbar()
        {
            yield return null;
            _scrollbar.value = 0;
        }
    }

}
