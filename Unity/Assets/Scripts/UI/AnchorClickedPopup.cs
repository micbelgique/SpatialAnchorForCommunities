using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnchorClickedPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text AnchorText;
    [SerializeField] private TMP_Text UserText;
    [SerializeField] private TMP_InputField MessageInputField;

    private AnchorDTO anchor;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.localScale = new Vector3(0, 0, 0);
    }


    public void ShowPopup(string anchorId)
    {
        this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        anchor = FileAndNetworkUtils.getObjectFromApi<AnchorDTO>("/api/AnchorsAPI/" + anchorId);
        AnchorText.text = anchor.identifier;
        UserText.text = "Placed by " + anchor.user.nickName;
    }

    public void ClosePopup()
    {
        this.gameObject.transform.localScale = new Vector3(0, 0, 0);
        MessageInputField.text = "";
    }

    public void OnSendButtonClicked()
    {
        var anchorInterraction = new TagInterraction{userId = anchor.user.id, anchorIdentifier = anchor.identifier, message = MessageInputField.text};
        FileAndNetworkUtils.postObjectToApi("/api/InteractionsAPI", anchorInterraction);
        ClosePopup();
        this.GetComponent<Animator>().SetTrigger("ToPage1");
    }

    
    
    public void OnDetailsButtonClicked()
    {
        CrossSceneInfoStatic.TagForTagDetails = anchor.identifier;
        SceneManager.LoadScene("TagDetails");
        ClosePopup();
    }
}
