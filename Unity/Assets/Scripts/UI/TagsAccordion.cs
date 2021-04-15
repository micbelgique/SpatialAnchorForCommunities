using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TagsAccordion : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject template;

    private ResultData<AnchorDTO> _resultData;

    [SerializeField] private string endpoint;

    [SerializeField] private bool byUser;
    [SerializeField] private bool displayLocation;
    
    [SerializeField] private Text numberText;


    private void Start()
    {
        using (HttpClient client = new HttpClient())
        {
            var httpResponseMessage =
                client.GetAsync("https://hiw-communities.azurewebsites.net" + endpoint + (byUser ? FileAndNetworkUtils.currentUser.id : FileAndNetworkUtils.currentUser.communityId.ToString())).Result;
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string res = httpResponseMessage.Content.ReadAsStringAsync().Result;
                Debug.Log(res);
                _resultData = JsonUtility.FromJson<ResultData<AnchorDTO>>(res);


                foreach (var tag in _resultData.data)
                {
                    Debug.Log("TAG USER : " + FileAndNetworkUtils.getObjectFromApi<User>("/api/UsersAPI/" + tag.userId).nickName);

                    User tagUser = FileAndNetworkUtils.getObjectFromApi<User>("/api/UsersAPI/" + tag.userId);
                    
                    GameObject go = Instantiate(template, transform);
                    if (byUser)
                    {
                        go.transform.Find("UserLabel").GetComponent<Text>().text = "Times checked :";
                        go.transform.Find("UserText").GetComponent<Text>().text = "11";
                    }
                    else
                        go.transform.Find("UserText").GetComponent<Text>().text = tagUser.nickName;
                    go.transform.Find("DateText").GetComponent<Text>().text = tag.creationDate;

                    if (displayLocation)
                    {
                        Community tagCommu = FileAndNetworkUtils.getObjectFromApi<Community>("/api/CommunitiesAPI/" + tagUser.communityId);
                        go.transform.Find("LocationText").GetComponent<Text>().text = tagCommu.address;
                    }
                    else
                    {
                        go.transform.Find("LocationLabel").gameObject.SetActive(false);
                        go.transform.Find("LocationText").gameObject.SetActive(false);
                    }

                    tag.pictureUrl = "https://i.pinimg.com/originals/20/79/03/2079033abc8314be554f9d24f562a199.jpg";
                    
                    if(!string.IsNullOrEmpty(tag.pictureUrl))
                        go.GetComponent<ImageToTag>().SetImage(tag.pictureUrl);

                    go.GetComponent<Button>().onClick.AddListener(delegate() { ItemClicked(tag.identifier); });
                }

                UpdateText(_resultData.data.Length);


            }
            else
            {
                Debug.Log("Error request");
            }
        }
    }

    private void UpdateText(int nbOfElements)
    {
        if (numberText is null) return;
        numberText.text += $" ({nbOfElements})";
    }
    
    private void ItemClicked(string item)
    {
        Debug.Log("Item Clicked : "+ item);
        CrossSceneInfoStatic.TagForTagDetails = item;
        SceneManager.LoadScene("TagDetails");
    }
}
