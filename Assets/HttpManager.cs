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
            // 만약에 결과가 정상이라면
            if(webRequest.result == UnityWebRequest.Result.Success)
            {
                // 우리가 원하는 데이터를 처리
                // print(webRequest.downloadHandler.text);
                // File.WriteAllBytes(Application.dataPath + "/image.jpg", webRequest.downloadHandler.data);

                // ================================

                // 응답 온 데이터를 요청한 클래스로 보내자.
                if(info.onComplete != null)
                {
                    info.onComplete(webRequest.downloadHandler);
                }
            }
            // 그렇지 않다면 (Error 라면)
            else
            {
                // Error의 이유를 출력 
                Debug.LogError("Net Error :" +  webRequest.error);
            }
        }

    }
}
