using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
public class UnityRequestClient
{
    public async static Task<T> Get<T>(string url)
    {

        using (var unityRequest = new UnityWebRequest(url, "GET"))
        {
            if (!string.IsNullOrEmpty(GameContext.Instance.Token))
            {
                unityRequest.SetRequestHeader("Authorization", "Bearer " + GameContext.Instance.Token);
            }
            unityRequest.SetRequestHeader("Content-Type", "application/json");
            unityRequest.downloadHandler = new DownloadHandlerBuffer();

            await unityRequest.SendWebRequest();

            if (unityRequest.error != null)
            {
#if DEBUG
                Debug.LogError(unityRequest.error);
#endif
                throw new InvalidOperationException("Error server " + unityRequest.error);
            }
            else
            {
                try
                {
                    byte[] results = unityRequest.downloadHandler.data;
                    var responseJson = Encoding.UTF8.GetString(results);
#if DEBUG
                    Debug.Log(responseJson);
#endif
                    return JsonConvert.DeserializeObject<T>(responseJson);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.LogError(ex.Message);
#endif
                    throw new InvalidOperationException("Error server " + ex.Message);
                }
            }

        }
        return default(T);
    }

    public async static Task<T> Post<T>(string url, object data)
    {

        using (var unityRequest = new UnityWebRequest(url, "POST"))
        {
            if (!string.IsNullOrEmpty(GameContext.Instance.Token))
            {
                unityRequest.SetRequestHeader("Authorization", "Bearer " + GameContext.Instance.Token);
            }
            unityRequest.SetRequestHeader("Content-Type", "application/json");

            var rpcRequestJson = JsonConvert.SerializeObject(data);
            var requestBytes = Encoding.UTF8.GetBytes(rpcRequestJson);
            var uploadHandler = new UploadHandlerRaw(requestBytes);

            uploadHandler.contentType = "application/json";
            unityRequest.uploadHandler = uploadHandler;
            unityRequest.downloadHandler = new DownloadHandlerBuffer();

            await unityRequest.SendWebRequest();

            if (unityRequest.error != null)
            {
#if DEBUG
                Debug.LogError(unityRequest.error);
#endif
                throw new InvalidOperationException("Error server " + unityRequest.error);
            }
            else
            {
                try
                {
                    byte[] results = unityRequest.downloadHandler.data;
                    var responseJson = Encoding.UTF8.GetString(results);
#if DEBUG
                    Debug.Log(responseJson);
#endif
                    return JsonConvert.DeserializeObject<T>(responseJson);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.LogError(ex.Message);
#endif
                    throw new InvalidOperationException("Error server " + ex.Message);
                }
            }

        }
        return default(T);
    }
}
