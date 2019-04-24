/***************************************************** 
** 内容简述： 
** 创 建 人： 
** 创建日期
** 修改记录： 
日期        版本      修改人    修改内容    
*****************************************************/  

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Baidu.Aip.Speech
{
    /// <summary>
    /// 用户解析token的json数据
    /// </summary>
    [System.Serializable]
    class TokenResponse
    {
        public string access_token = null;
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

        public BaseRequireToken(string apiKey, string secretKey)
        {
            APIKey = apiKey;
            SecretKey = secretKey;
        }

        public IEnumerator GetAccessToken()
        {
            string autoHost = "https://aip.baidubce.com/oauth/2.0/token";
            var uri = string.Format("{0}?grant_type=client_credentials&client_id={1}&client_secret={2}", autoHost, APIKey, SecretKey);
            var www = UnityWebRequest.Get(uri);
            yield return www.SendWebRequest();

            if (string.IsNullOrEmpty(www.error))
            {
                //Debug.Log(www.downloadHandler.text);
                var result = JsonUtility.FromJson<TokenResponse>(www.downloadHandler.text);
                Token = result.access_token;
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

