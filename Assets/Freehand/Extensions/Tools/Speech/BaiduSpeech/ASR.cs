using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
namespace Baidu.Aip.Speech
{
    public delegate void ASRAction(ASRResponse asrResponse); 
    // 返回的内容
    [System.Serializable]
    public class ASRResponse
    {
        // 错误码
        public int err_no;
        // 错误描述
        public string err_msg;
        // 语音数据唯一标识。如果反馈及debug提供sn
        public string sn;
        // 识别结果数组，提供1-5个候选结果，优先使用第一个结果。utf-8编码
        public string[] result;
    }

    public class ASR : BaseRequireToken
    {
        // 百度服务器链接
        private const string urlAsr = "https://vop.baidu.com/server_api";

        public int dev_pid { get; private set; }

        public ASR(string apiKey, string secretKey, int devPid = 1537) : base(apiKey, secretKey)
        {
            dev_pid = devPid;
        }

        /// <summary>
        ///  发送并返回语音识别结果
        /// </summary>
        /// <param name="audioData"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator Recognize(byte[] audioData,ASRAction callback)
        {
            yield return PreAction();

            // 如果获取失败，则会报出错误
            if(tokenFetchStatus == TokenFetchStatus.Failed)
            {
                Debug.LogError("Token fetched failed, please check your APIKey and SecretKey");
                yield break;
            }

            // 上传本地数据
            var uri = string.Format("{0}?dev_pid={1}&lan=zh&cuid={2}&token={3}", urlAsr, dev_pid,SystemInfo.deviceUniqueIdentifier, Token);

            var form = new WWWForm();

            // 音频数据放在HTTP BODY 中
            form.AddBinaryData("audio", audioData);

            // 使用Post方式进行上传本地 并返回相应数据
            var www = UnityWebRequest.Post(uri, form);

            // 语音数据的采样率和压缩格式在HTTP-HEADER里的Content-Type表明 (语音格式可选 pcm/wav/amr)
            www.SetRequestHeader("Content-Type", "audio/pcm;rate=16000");

            // 发送请求
            yield return www.SendWebRequest();

            // 如果请求没有报出错误，则说明请求成功，否则报错
            if (string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.downloadHandler.text);
                if (callback != null)
                {
                    // 调用回调事件
                    ASRResponse response = JsonUtility.FromJson<ASRResponse>(www.downloadHandler.text);

                    callback(response);
                }
            }
            else
            {
                Debug.LogError(www.error);
            }
        }


        /// <summary>
        /// 将Unity的AudioClip数据转化为PCM格式16bit数据
        /// </summary>
        /// <param name="clip"></param>
        /// <returns></returns>
        public static byte[] ConvertAudioClipToPCM16(AudioClip clip)
        {
            var samples = new float[clip.samples * clip.channels];

            clip.GetData(samples, 0);

            var samples_int16 = new short[samples.Length];

            for (var index = 0; index < samples.Length; index++)
            {
                var f = samples[index];
                samples_int16[index] = (short)(f * short.MaxValue);
            }

            var byteArray = new byte[samples_int16.Length * 2];
            Buffer.BlockCopy(samples_int16, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }

    }
}
