using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HttpTest : MonoBehaviour
{
    void Start()
    {
        
    }
    public PostInfoArray allPostInfo;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HttpInfo info = new HttpInfo();
            info.url = "https://jsonplaceholder.typicode.com/posts=�ȳ��ϼ���";

            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler);

                //string jsonData = "{\"data\": " + downloadHandler.text + " }"; 
                //print(jsonData);
                // jsonData�� PostInfoArray ������ �ٲ���
                //allPostInfo = JsonUtility.FromJson<PostInfoArray>(jsonData); // json �����͸� C# ��ü�� �ٲٱ�
            };

            StartCoroutine(HttpManager.GetInstance().Get(info));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HttpInfo info = new HttpInfo();
            info.url = "https://ssl.pstatic.net/melona/libs/1515/1515567/5a8751410fddb0f462dc_20241125132718754.jpg";

            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                // File.WriteAllBytes(Application.dataPath + "/image2.jpg", downloadHandler.data);

             
                // �ٿ�ε� �� �����͸� Texture2D�� ��ȯ.
                DownloadHandlerTexture handler = downloadHandler as DownloadHandlerTexture; 
                Texture2D texture = handler.texture;

                // texture�� �̿��ؼ� Sprite�� ��ȯ
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                Image image = GameObject.Find("Image").GetComponent<Image>();
                image.sprite = sprite;
            };

            // StartCoroutine(HttpManager.GetInstance().Get(info));
            StartCoroutine(HttpManager.GetInstance().DownloadSprtie(info));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // ������ ���� �����͸� ������
            UserInfo userInfo = new UserInfo();
            userInfo.name = "��Ÿ����";
            userInfo.age = 3;
            userInfo.height = 180.5f;

            HttpInfo info = new HttpInfo();
            info.url = "http://mtvs.helloworldlabs.kr:7771/api/json";
            info.body = JsonUtility.ToJson(userInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HttpInfo info = new HttpInfo();
            info.url = "http://mtvs.helloworldlabs.kr:7771/api/file";
            info.contentType = "multipart/form-data";
            info.body = "C:\\Users\\������\\OneDrive\\���� ȭ��\\���漼��.png";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                File.WriteAllBytes(Application.dataPath + "/ajh.jpg",downloadHandler.data);
            };
            StartCoroutine(HttpManager.GetInstance().UploadFileByFormData(info));
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            HttpInfo info = new HttpInfo();
            info.url = "http://mtvs.helloworldlabs.kr:7771/api/byte";
            info.contentType = "image/jpg";
            info.body = "C:\\Users\\������\\OneDrive\\���� ȭ��\\���漼��.png";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                File.WriteAllBytes(Application.dataPath + "/ajh.jpg", downloadHandler.data);
            };
            StartCoroutine(HttpManager.GetInstance().UploadFileByByte(info));
        }


    }
}

[System.Serializable]
public struct UserInfo
{
    public string name;
    public int age;
    public float height;

}