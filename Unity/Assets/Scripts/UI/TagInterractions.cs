using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TagInterractions : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject template;

    private ResultData<TagInterraction> _resultData;

    [SerializeField] private string endpoint;
    

    public void DisplayInterractions(List<TagInterraction> tagInterractions)
    {
        foreach (var interraction in tagInterractions)
        {
            Debug.Log("INTERRACTION USER : " + FileAndNetworkUtils.getObjectFromApi<User>("/api/UsersAPI/" + interraction.userId).nickName);

            User interractUser = FileAndNetworkUtils.getObjectFromApi<User>("/api/UsersAPI/" + interraction.userId);
                    
            GameObject go = Instantiate(template, transform);
            go.transform.Find("UserText").GetComponent<TMP_Text>().text = interractUser.nickName;
            go.transform.Find("DateText").GetComponent<TMP_Text>().text = interraction.creationDate;
            go.transform.Find("MessageText").GetComponent<TMP_Text>().text = interraction.message;
            
            if(!string.IsNullOrEmpty(interraction.pictureUrl))
                go.GetComponent<ImageToTag>().SetImage(interraction.pictureUrl);

        }
    }
    
    
    private void ItemClicked(string item)
    {
        Debug.Log("Item Clicked : "+ item);
    }
}
