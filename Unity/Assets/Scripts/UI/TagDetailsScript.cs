using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TagDetailsScript : MonoBehaviour
{

    [SerializeField] private TMP_Text UserText;
    [SerializeField] private TMP_Text IdText;

    [SerializeField] private TagInterractions _tagInterractions;
    // Start is called before the first frame update
    void Start()
    {
        AnchorDTO tag = FileAndNetworkUtils.getObjectFromApi<AnchorDTO>("/api/AnchorsAPI/" + CrossSceneInfoStatic.TagForTagDetails);
        tag.user = FileAndNetworkUtils.getObjectFromApi<User>("/api/UsersAPI/" + tag.userId);
        
        UserText.text = "Placed by " + tag.user.nickName;
        IdText.text = tag.identifier;
        
        tag.pictureUrl = "https://i.pinimg.com/originals/20/79/03/2079033abc8314be554f9d24f562a199.jpg";

        
        if(!string.IsNullOrEmpty(tag.pictureUrl))
            this.GetComponent<ImageToTag>().SetImage(tag.pictureUrl);
        
        _tagInterractions.DisplayInterractions(tag.interactions);
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("AzureSpatialAnchorsBasicDemo");
    }
}
