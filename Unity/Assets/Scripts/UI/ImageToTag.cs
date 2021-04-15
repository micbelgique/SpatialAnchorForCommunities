using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageToTag : MonoBehaviour
{
    [SerializeField] private Image trueimage;
    
    public void SetImage(string url)
    {
        StartCoroutine(DownloadImage(url));
    }
    
    IEnumerator DownloadImage(string MediaUrl)
    {   
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if(request.isNetworkError || request.isHttpError) 
            Debug.Log(request.error);
        else
        {
            Texture2D texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            trueimage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            trueimage.color = Color.white;
        }
    } 
    
}
