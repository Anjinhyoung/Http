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

    // Body ������
    public string body = "";

    // contentType 
    public string contentType = "";

    // ��� ���� �� ȣ��Ǵ� �Լ� ���� ����
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
            go.AddComponent<HttpManager>(); // �� �ڵ带 �����ϰ� �Ʒ� �ڵ尡 �ٷ� ������� �ʰ� Awake�� ȣ��ǰ� �� ���� �Ʒ� �ڵ� ����
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

    // Get: �������� �����͸� ��ȸ ��û
    public IEnumerator Get(HttpInfo info)
    {
        // string img = "https://ssl.pstatic.net/melona/libs/1517/1517456/84e03b6a69bac171d541_20241126154454802.png";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(info.url))
        {
            // ������ ��û ������ (������ �� �� ���� ��ٸ���, �񵿱� ���)
            yield return webRequest.SendWebRequest();

            // �������� ������ �Դ�.
            DoneRequest(webRequest, info);
        }
    }

    // �������� ���� ������ �����͸� ��������
    public IEnumerator Post(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, info.body, info.contentType))
        {
            // ������ ��û ������ (������ �� �� ���� ��ٸ���, �񵿱� ���)
            yield return webRequest.SendWebRequest();

            // �������� ������ �Դ�.
            DoneRequest(webRequest, info);
        }
    }

     // ���� ���ε� (form - data)
     public IEnumerator UploadFileByFormData(HttpInfo info)
     {
         // info.data ���� ������ ��ġ 
         // info.data�� �ִ� ������ byte �迭�� �о����.
         byte[] data = File.ReadAllBytes(info.body);


         // data�� MulitpartForm ���� ����
         List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
         formData.Add(new MultipartFormFileSection("file", data, "image.jpg", info.contentType));

         using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, formData))
         {
             // ������ ��û ������ (������ �� �� ���� ��ٸ���, �񵿱� ���)
             yield return webRequest.SendWebRequest();

             // �������� ������ �Դ�.
             DoneRequest(webRequest, info);
         }
     }

    // ���� ���ε� 
    public IEnumerator UploadFileByByte(HttpInfo info)
    {
        // info.data ���� ������ ��ġ 
        // info.data�� �ִ� ������ byte �迭�� �о����.
        byte[] data = File.ReadAllBytes(info.body);

        using (UnityWebRequest webRequest = new UnityWebRequest(info.url, "POST"))
        {
            // ���ε� �ϴ� ������ 
            webRequest.uploadHandler = new UploadHandlerRaw(data);
            webRequest.uploadHandler.contentType = info.contentType;

            // ���� �޴� ������ ����
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // ������ ��û ������ (������ �� �� ���� ��ٸ���, �񵿱� ���)
            yield return webRequest.SendWebRequest();

            // �������� ������ �Դ�.
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
         // �׷��� �ʴٸ� (Error ���)
         else
         {
             // Error�� ������ ��� 
             Debug.LogError("Net Error :" + webRequest.error);
         }
     }
}
