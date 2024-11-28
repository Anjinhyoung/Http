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
            info.url = "https://jsonplaceholder.typicode.com/posts=안녕하세요";

            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler);

                //string jsonData = "{\"data\": " + downloadHandler.text + " }"; 
                //print(jsonData);
                // jsonData를 PostInfoArray 형으로 바꾸자
                //allPostInfo = JsonUtility.FromJson<PostInfoArray>(jsonData); // json 데이터를 C# 객체로 바꾸기
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

             
                // 다운로드 된 데이터를 Texture2D로 반환.
                DownloadHandlerTexture handler = downloadHandler as DownloadHandlerTexture; 
                Texture2D texture = handler.texture;

                // texture를 이용해서 Sprite로 변환
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                Image image = GameObject.Find("Image").GetComponent<Image>();
                image.sprite = sprite;
            };

            // StartCoroutine(HttpManager.GetInstance().Get(info));
            StartCoroutine(HttpManager.GetInstance().DownloadSprtie(info));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // 가상의 나의 데이터를 만들자
            UserInfo userInfo = new UserInfo();
            userInfo.name = "메타버스";
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
            info.body = "C:\\Users\\안진형\\OneDrive\\바탕 화면\\포톤세팅.png";
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
            info.body = "C:\\Users\\안진형\\OneDrive\\바탕 화면\\포톤세팅.png";
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