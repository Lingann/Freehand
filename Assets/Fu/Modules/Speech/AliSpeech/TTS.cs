using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Fu.Ali.Speech
{
    public delegate void TTSAction(TTSResponse ttsResponse);
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

    public struct TTSArgs
    {
        // 应用appkey
        public string appkey;

        // * 合成的文本，使用UTF-8编码
        public string text;

        // * access token
        public string token;

        // 音频编码格式，支持的格式: pcm,wav,mp3 默认是pcm
        public string format;

        // 音频采样率，支持16000Hz,8000Hz,默认16000Hz
        public int sample_rate;

        // 发音人 ，默认是xiaoyun
        public string voice;

        // 音量，范围是0~100，默认是50
        public int volume;

        // 语速， 范围是-500~500，默认是0
        public int speech_rate;

        // 语调，范围是-500~500,可选，默认是0
        public int pitch_rate;
    }

    public class TTS
    {
        // 百度服务器链接
        private const string UrlTts = "https://nls-gateway.cn-shanghai.aliyuncs.com/stream/v1/tts";

        /// <summary>
        ///  上传地址
        /// </summary>
        public string postUrl
        {
            get
            {
                var param = new Dictionary<string, string>();
                param.Add("appkey", ttsArgs.appkey);
                param.Add("text", ttsArgs.text);
                param.Add("token", ttsArgs.token);
                param.Add("format", ttsArgs.format);
                param.Add("sample_rate	", ttsArgs.sample_rate.ToString());
                param.Add("voice", ttsArgs.voice);
                param.Add("volume", ttsArgs.volume.ToString());
                param.Add("speech_rate", ttsArgs.speech_rate.ToString());
                param.Add("pitch_rate", ttsArgs.pitch_rate.ToString());
                string url = UrlTts;
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

        private TTSArgs ttsArgs;

        /// <summary>
        /// 语音合成
        /// </summary>
        public TTS(string apiKey,string token)
        {
            ttsArgs = new TTSArgs();

            ttsArgs.appkey = apiKey;

            ttsArgs.text = "";

            // 语言设置
            ttsArgs.token = token;

            // 音频编码格式
            ttsArgs.format = "wav";

            // 音频采样率
            ttsArgs.sample_rate = 16000;

            // 音色设置
            ttsArgs.voice = "sicheng";

            // 音量设置 女声
            ttsArgs.volume = 50;

            // 语速
            ttsArgs.speech_rate = 0 ;

            ttsArgs.pitch_rate = 0;
        }

        public IEnumerator Synthesis(string text, TTSAction callback)
        {
            ttsArgs.text = text;

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
                callback(null);
                Debug.LogError(www.error);
            }
        }

    }
}
