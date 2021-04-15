using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CommunityAccordion : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject template;

    private List<User> _resultData;

    [SerializeField] private Text numberText;

    private void Start()
    {
        using (HttpClient client = new HttpClient())
        {
            var httpResponseMessage =
                client.GetAsync("https://hiw-communities.azurewebsites.net/api/CommunitiesAPI/" + FileAndNetworkUtils.currentUser.communityId + "?full=true").Result;
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string res = httpResponseMessage.Content.ReadAsStringAsync().Result;
                Debug.Log(res);
                _resultData = JsonUtility.FromJson<Community>(res).users;

                foreach (var user in _resultData)
                {
                    GameObject go = Instantiate(template, transform);
                    go.transform.Find("UserName").GetComponent<Text>().text = user.nickName;
                    go.transform.Find("UserMission").GetComponent<Text>().text = user.mission;

                    go.GetComponent<Button>().onClick.AddListener(delegate() { ItemClicked(user.nickName); });
                }
                
                UpdateText(_resultData.Count);

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
    }
}
