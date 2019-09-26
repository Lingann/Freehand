using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace Baidu.Aip.Speech
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
        // * 合成的文本，使用UTF-8编码
        public string tex;

        // * access token
        public string tok;

        // * 用户唯一标识 
        public string cuid;

        // * 客户端类型，web端写固定端1
        public int ctp;

        // * 语言选择， 固定值 zh。目前只有中英文混合模式
        public string lan;

        // 语速 0-15
        public int spd;

        // 音调 0-15
        public int pit;

        // 音量 0-15
        public int vol;

        // 音色
        public Pronouncer per;

        // 格式，默认mp3
        public int aue;
    }
    public enum Pronouncer
    {
        Female, // 0为普通女声
        Male, // 1为普通男声
        Duxiaoyao, // 3为情感合成-度逍遥
        Duyaya // 4为情感合成-度丫丫
    }

    public class TTS : BaseRequireToken
    {
        // 百度服务器链接
        private const string UrlTts = "http://tsn.baidu.com/text2audio";

        /// <summary>
        ///  上传地址
        /// </summary>
        public string postUrl
        {
            get
            {
                var param = new Dictionary<string, string>();
                param.Add("tex", ttsArgs.tex);
                param.Add("tok", ttsArgs.tok);
                param.Add("cuid", ttsArgs.cuid);
                param.Add("ctp", ttsArgs.ctp.ToString());
                param.Add("lan", ttsArgs.lan);
                param.Add("spd", ttsArgs.spd.ToString());
                param.Add("pit", ttsArgs.pit.ToString());
                param.Add("vol", ttsArgs.vol.ToString());
                param.Add("per", ((int)ttsArgs.per).ToString());
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_UWP
                param.Add("aue", "6"); // set to wav, default is mp3
#endif
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
        /// <param name="apiKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="speed">语速(0-9)~5</param>
        /// <param name="pit">语调(0,9)~5</param>
        /// <param name="vol">音量(0,15)~5</param>
        /// <param name="per">声音(女生，男生，😊，😳)</param>
        public TTS(string apiKey, string secretKey) : base(apiKey, secretKey)
        {
            ttsArgs = new TTSArgs();

            ttsArgs.cuid = SystemInfo.deviceUniqueIdentifier;

            ttsArgs.ctp = 1;

            // 语言设置
            ttsArgs.lan = "zh";

            // 音速设置
            ttsArgs.spd = 6;

            // 音调设置
            ttsArgs.pit = 5;

            // 音量设置
            ttsArgs.vol = 5;

            // 音色设置 女声
            ttsArgs.per = Pronouncer.Female;

            // 文件上传格式设置，6为wav
            ttsArgs.aue = 6;
        }

        public IEnumerator Synthesis(string text, TTSAction callback)
        {
            yield return PreAction();

            if (tokenFetchStatus == BaseRequireToken.TokenFetchStatus.Failed)
            {
                Debug.LogError("Token was fetched failed. Please check your APIKey and SecretKey");
                callback(new TTSResponse()
                {
                    err_no = -1,
                    err_msg = "Token was fetched failed. Please check your APIKey and SecretKey"
                });
                yield break;
            }

            ttsArgs.tex = text;

            ttsArgs.tok = Token;

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

                if (type.Contains("audio"))
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
