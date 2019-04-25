using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freehand.Ali.Speech
{

    public class AliAIConfig
    {
        public AliDomain domain;
        public TTSConfig tts;
        public ASRConfig asr;
    }

    [System.Serializable]
    public class AliDomain
    {
        // 语音合成
        public string ttsServer;
        // 语音生成
        public string asrServer;
    }

    [System.Serializable]
    public class TTSConfig
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

    [System.Serializable]
    public class ASRConfig
    {

    }
}
