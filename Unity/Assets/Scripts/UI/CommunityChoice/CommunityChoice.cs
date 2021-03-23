using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.UI;

public class CommunityChoice : MonoBehaviour
{
    public GameObject template;
    public GameObject templateNone;

    private ResultData<Community> _resultData;

    private void Start()
    {
        using (HttpClient client = new HttpClient())
        {
            var httpResponseMessage = client.GetAsync("https://hiw-communities.azurewebsites.net/api/CommunitiesAPI").Result;
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string res = httpResponseMessage.Content.ReadAsStringAsync().Result;
                Debug.Log(res);
                _resultData = JsonUtility.FromJson<ResultData<Community>>(res);
                                
                foreach (var community in _resultData.data)
                {
                    GameObject go = Instantiate(template, transform);
                    go.GetComponent<CommunityCard>().SetCommunityName(community.name);
                    go.GetComponent<CommunityCard>().SetCommunityDescription(community.address);
                    go.GetComponent<CommunityCard>().SetCommunityImage(community.pictureUrl);
                    
                    go.GetComponent<Button>().onClick.AddListener(delegate()
                    {
                        ItemClicked(community.name);
                    });
                }

                GameObject game = Instantiate(templateNone, transform);
                game.GetComponent<Button>().onClick.AddListener(delegate()
                {
                    ItemClicked("none");
                });
            }
            else
            {
                Debug.Log("Error request");
            }
        }
    }

    private void ItemClicked(string community)
    {
        Debug.Log(community);
    }
    
}
