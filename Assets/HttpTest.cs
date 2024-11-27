using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

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
            info.url = "https://jsonplaceholder.typicode.com/posts";

            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                string jsonData = "{\"data\": " + downloadHandler.text + " }"; 
                print(jsonData);
                // jsonData�� PostInfoArray ������ �ٲ���
                allPostInfo = JsonUtility.FromJson<PostInfoArray>(jsonData); // json �����͸� C# ��ü�� �ٲٱ�
            };

            StartCoroutine(HttpManager.GetInstance().Get(info));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HttpInfo info = new HttpInfo();
            info.url = "https://ssl.pstatic.net/melona/libs/1515/1515567/5a8751410fddb0f462dc_20241125132718754.jpg";

            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                File.WriteAllBytes(Application.dataPath + "/image2.jpg", downloadHandler.data);
            };

            StartCoroutine(HttpManager.GetInstance().Get(info));
        }
    }
}
