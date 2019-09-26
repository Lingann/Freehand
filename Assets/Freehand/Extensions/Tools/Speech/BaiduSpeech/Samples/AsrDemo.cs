using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Baidu.Aip.Speech;
public class AsrDemo : MonoBehaviour
{
    public int devPid = 1537;
    public string APIKey = "";
    public string SecretKey = "";

    public Text DescriptionText;

    private AudioClip _clipRecord;
    private ASR _asr;

    void Start()
    {
        _asr = new ASR(APIKey, SecretKey, devPid);
        StartCoroutine(_asr.GetAccessToken());

        DescriptionText.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DescriptionText.text = "Listening...";

            // WebGL 不支持Microphone 
            _clipRecord = Microphone.Start(null, false, 30, 16000);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            DescriptionText.text = "Recognizing...";
            Microphone.End(null);
            var data = ASR.ConvertAudioClipToPCM16(_clipRecord);
            StartCoroutine(_asr.Recognize(data, s =>
            {
                DescriptionText.text = s.result != null && s.result.Length > 0 ? s.result[0] : "未识别到声音";

            }));
        }
    }
}
