using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Freehand.Ali.Speech
{
    public delegate void TTSAction(TTSResponse ttsResponse);

    public class TTS
    {

        // 阿里服务器链接
        private const string UrlTts = "https://nls-gateway.cn-shanghai.aliyuncs.com/stream/v1/tts";

        public AliAIConfig config { get { return _config; } }

        private AliAIConfig _config;

        /// <summary>
        ///  上传地址
        /// </summary>
        public string postUrl
        {
            get
            {
                var param = new Dictionary<string, string>();
                param.Add("appkey", config.tts.appkey);
                param.Add("text", config.tts.text);
                param.Add("token", config.tts.token);
                param.Add("format", config.tts.format);
                param.Add("sample_rate	", config.tts.sample_rate.ToString());
                param.Add("voice", config.tts.voice);
                param.Add("volume", config.tts.volume.ToString());
                param.Add("speech_rate", config.tts.speech_rate.ToString());
                param.Add("pitch_rate", config.tts.pitch_rate.ToString());
                string url = config.domain.ttsServer;
                int i = 0;
                foreach (var p in param)
                {
                    url += i != 0 ? "&" : "?";
                    url += p.Key + "=" + p.Value;
                    i++;
                }
                return url;
            }
        }

        /// <summary>
        /// 语音合成
        /// </summary>
        public TTS(AliAIConfig aliAIConfig)
        {
            _config = aliAIConfig;
        }

        public IEnumerator Synthesis(string text, TTSAction callback)
        {
            _config.tts.text = text;

#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_UWP
            var www = UnityWebRequestMultimedia.GetAudioClip(postUrl, AudioType.WAV);
#else
            var www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
#endif
            //Debug.Log(www.url);
            yield return www.SendWebRequest();

            if (string.IsNullOrEmpty(www.error))
            {
                var type = www.GetResponseHeader("Content-Type");
                //Debug.Log("response type: " + type);

                if (type.Contains("audio/mpeg"))
                {
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_UWP
                    var clip = DownloadHandlerAudioClip.GetContent(www);
                    var response = new TTSResponse { clip = clip };
#else
                    var response = new TtsResponse {clip = DownloadHandlerAudioClip.GetContent(www) };
#endif
                    callback(response);
                }
                else
                {
                    Debug.LogError(www.downloadHandler.text);
                    callback(JsonUtility.FromJson<TTSResponse>(www.downloadHandler.text));
                }
            }
            else
            {
                Debug.LogError(www.error);
                callback(null);
            }
        }
    }

    /// <summary>
    ///     语音合成结果
    /// </summary>
    [Serializable]
    public class TTSResponse
    {
        public int err_no;
        public string err_msg;
        public string sn;
        public int idx;

        public bool Success
        {
            get { return err_no == 0; }
        }
        public AudioClip clip;
    }
}
