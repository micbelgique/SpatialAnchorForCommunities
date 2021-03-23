using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CommunityCard : MonoBehaviour
{
    public Text communityName;
    public Text communityDescription;
    public RawImage communityImage;

    public void SetCommunityName(string name)
    {
        communityName.text = name;
    }

    public void SetCommunityDescription(string description)
    {
        communityDescription.text = description;
    }

    public void SetCommunityImage(string url)
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
            communityImage.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
    } 

}
