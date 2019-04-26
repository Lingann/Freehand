using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace Freehand.Ali.Speech
{
    /// <summary>
    /// 用户解析token的json数据
    /// </summary>
    [System.Serializable]
    class TokenResponse
    {
        public string NlsRequestId = "";
        public string RequestId = "";

        public Token Token = null;
    }

    [System.Serializable]
    class Token
    {
        public int ExpireTime = 0;
        public string Id = "";
        public string UserID = "";
    }

    public class BaseRequireToken
    {
        protected enum TokenFetchStatus
        {
            NotFetched,
            Fetching,
            Success,
            Failed
        }

        public string Token;

        /// <summary>
        /// 百度云应用的API KEY
        /// </summary>
        public string APIKey { get; private set; }

        /// <summary>
        /// 百度云应用的Secret Key
        /// </summary>
        public string SecretKey { get; private set; }

        protected TokenFetchStatus tokenFetchStatus = TokenFetchStatus.NotFetched;


        public struct TokenArgs
        {
            // 阿里云颁发给您的访问服务所用的密钥ID，请填入您的阿里云账号的AccessKey ID
            public string AccessKeyId;
            // 该API的名称 CreateToken
            public string Action;
            // 该API的版本好，固定值2019-02-28
            public string Version;
            // 响应返回的类型：JSON
            public string Format;
            // 服务的地域ID,固定值 cn-shanghai
            public string RegionId;
            // 请求的时间戳。日期格式按照ISO 8601标准表示，并需要使用UTC时间，时区为：+0。格式为YYYY-MM-DDThh:mm:ssZ。例如2019-04-03T06:15:03Z 为UTC时间2019年4月3日6点15分03秒。
            public string Timestamp;
            // 签名算法：HMAC-SHA1
            public string SignatureMethod;
            // 签名算法版本：1.0
            public string SignatureVersion;
            // 唯一随机数uuid，用于请求的防重放攻击，每次请求唯一，不能重复使用。格式为xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx(8-4-4-4-12)，例如8d1e6a7a-f44e-40d5-aedb-fe4a1c80f434
            public string SignatureNonce;
            // 由所有请求参数计算出的签名结果，生成方法请参考下文签名机制。
            public string Signature;
        }



        public BaseRequireToken(string accessKey, string accessKeySecret)
        {
            APIKey = accessKey;
            SecretKey = accessKeySecret;
        }

        public IEnumerator GetAccessToken()
        {
            string autoHost = "http://nls-meta.cn-shanghai.aliyuncs.com/";
            var uri = string.Format("{0}?grant_type=client_credentials&client_id={1}&client_secret={2}", autoHost, APIKey, SecretKey);
            var www = UnityWebRequest.Get(uri);
            yield return www.SendWebRequest();
                //Debug.Log(www.downloadHandler.text);
            if (string.IsNullOrEmpty(www.error)) {
                var result = JsonUtility.FromJson<TokenResponse>(www.downloadHandler.text);
                Token = result.Token.Id;
                Debug.Log("Token has been fetched successfully");
                tokenFetchStatus = TokenFetchStatus.Success;
            }
            else
            {
                Debug.LogError(www.error);
                Debug.LogError("Token was fetched failed. Please check your APIKey and SecretKey");
                tokenFetchStatus = TokenFetchStatus.Failed;
            }
        }

        protected IEnumerator PreAction()
        {
            if (tokenFetchStatus == TokenFetchStatus.NotFetched)
            {
                Debug.Log("Token has not been fetched, now fetching...");
                yield return GetAccessToken();
            }

            if (tokenFetchStatus == TokenFetchStatus.Fetching)
                Debug.Log("Token is still being fetched, waiting...");

            while (tokenFetchStatus == TokenFetchStatus.Fetching)
            {
                yield return null;
            }
        }
    }
}
