using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct PostInfo
{
    public int userId;
    public int id;
    public string title;
    public string body;
}

[System.Serializable]
public struct PostInfoArray
{
    public List<PostInfo> data;
}

public class HttpInfo
{
    public string url = "";

    // Body 데이터
    public string body = "";

    // contentType 
    public string contentType = "";

    // 통신 성공 후 호출되는 함수 담을 변수
    public Action<DownloadHandler> onComplete;
}

public class HttpManager : MonoBehaviour
{
    static HttpManager instance;

    public static HttpManager GetInstance()
    {
        if(instance == null)
        {
            GameObject go = new GameObject();
            go.name = "HttpManager";
            go.AddComponent<HttpManager>(); // 이 코드를 실행하고 아래 코드가 바로 실행되지 않고 Awake가 호출되고 그 다음 아래 코드 실행
        }

        return instance;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Get: 서버에게 데이터를 조회 요청
    public IEnumerator Get(HttpInfo info)
    {
        // string img = "https://ssl.pstatic.net/melona/libs/1517/1517456/84e03b6a69bac171d541_20241126154454802.png";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(info.url))
        {
            // 서버에 요청 보내기 (응답이 올 때 까지 기다리기, 비동기 방식)
            yield return webRequest.SendWebRequest();

            // 서버에게 응답이 왔다.
            DoneRequest(webRequest, info);
        }
    }

    // 서버에게 내가 보내는 데이터를 생성해줘
    public IEnumerator Post(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, info.body, info.contentType))
        {
            // 서버에 요청 보내기 (응답이 올 때 까지 기다리기, 비동기 방식)
            yield return webRequest.SendWebRequest();

            // 서버에게 응답이 왔다.
            DoneRequest(webRequest, info);
        }
    }

     // 파일 업로드 (form - data)
     public IEnumerator UploadFileByFormData(HttpInfo info)
     {
         // info.data 에는 파일의 위치 
         // info.data에 있는 파일을 byte 배열로 읽어오자.
         byte[] data = File.ReadAllBytes(info.body);


         // data를 MulitpartForm 으로 셋팅
         List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
         formData.Add(new MultipartFormFileSection("file", data, "image.jpg", info.contentType));

         using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, formData))
         {
             // 서버에 요청 보내기 (응답이 올 때 까지 기다리기, 비동기 방식)
             yield return webRequest.SendWebRequest();

             // 서버에게 응답이 왔다.
             DoneRequest(webRequest, info);
         }
     }

    // 파일 업로드 
    public IEnumerator UploadFileByByte(HttpInfo info)
    {
        // info.data 에는 파일의 위치 
        // info.data에 있는 파일을 byte 배열로 읽어오자.
        byte[] data = File.ReadAllBytes(info.body);

        using (UnityWebRequest webRequest = new UnityWebRequest(info.url, "POST"))
        {
            // 업로드 하는 데이터 
            webRequest.uploadHandler = new UploadHandlerRaw(data);
            webRequest.uploadHandler.contentType = info.contentType;

            // 응답 받는 데이터 공간
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // 서버에 요청 보내기 (응답이 올 때 까지 기다리기, 비동기 방식)
            yield return webRequest.SendWebRequest();

            // 서버에게 응답이 왔다.
            DoneRequest(webRequest, info);
        }
    }

    public IEnumerator DownloadSprtie(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(info.url))
        {
            yield return webRequest.SendWebRequest();
            DoneRequest(webRequest, info);
        }
    }
     void DoneRequest(UnityWebRequest webRequest, HttpInfo info)
     {
         if (webRequest.result == UnityWebRequest.Result.Success)
         {
             if (info.onComplete != null)
             {
                 info.onComplete(webRequest.downloadHandler);
             }
         }
         // 그렇지 않다면 (Error 라면)
         else
         {
             // Error의 이유를 출력 
             Debug.LogError("Net Error :" + webRequest.error);
         }
     }
}
