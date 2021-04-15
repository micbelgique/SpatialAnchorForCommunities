using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CommunityUIScript : MonoBehaviour
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text AddressText;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private RawImage CommunityImage;
    [SerializeField] private GameObject CommunityPanel;
    [SerializeField] private GameObject NoCommunityPanel;

    private Community _community;
    // Start is called before the first frame update
    void Start()
    {
        if(FileAndNetworkUtils.currentUser.communityId != -1)
            DisplayCommunityPage();
        else
            DisplayNoCommunityPage();
    }

    private void DisplayCommunityPage()
    {
        CommunityPanel.SetActive(true);
        NoCommunityPanel.SetActive(false);
        _community = FileAndNetworkUtils.getObjectFromApi<Community>("/api/CommunitiesAPI/" + FileAndNetworkUtils.currentUser.communityId);
        Debug.Log("URL : " + _community.pictureUrl);
        StartCoroutine(DownloadImage(_community.pictureUrl));
        NameText.text = _community.name;
        AddressText.text = _community.address;
    }
    
    private void DisplayNoCommunityPage()
    {
        CommunityPanel.SetActive(false);
        NoCommunityPanel.SetActive(true);
        AddressText.text = "You can request membership of the following communities : ";
    }
    
    IEnumerator DownloadImage(string MediaUrl)
    {   
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if(request.isNetworkError || request.isHttpError) 
            Debug.Log(request.error);
        else
            CommunityImage.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
    }

    public void OnHyperlinkClicked()
    {
        Application.OpenURL(_community.infoUrl);
    }
    
}
