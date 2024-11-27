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
            // ���࿡ ����� �����̶��
            if(webRequest.result == UnityWebRequest.Result.Success)
            {
                // �츮�� ���ϴ� �����͸� ó��
                // print(webRequest.downloadHandler.text);
                // File.WriteAllBytes(Application.dataPath + "/image.jpg", webRequest.downloadHandler.data);

                // ================================

                // ���� �� �����͸� ��û�� Ŭ������ ������.
                if(info.onComplete != null)
                {
                    info.onComplete(webRequest.downloadHandler);
                }
            }
            // �׷��� �ʴٸ� (Error ���)
            else
            {
                // Error�� ������ ��� 
                Debug.LogError("Net Error :" +  webRequest.error);
            }
        }

    }
}
