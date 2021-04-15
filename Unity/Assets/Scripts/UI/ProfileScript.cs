using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileScript : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text communityText;
    [SerializeField] private TMP_Text emailText;
    [SerializeField] private TMP_Text phoneText;
    [SerializeField] private TMP_Text socialsText;
    [SerializeField] private TMP_Text enterpriseText;
    [SerializeField] private TMP_Text missionText;
    // Start is called before the first frame update

    enum State
    {
        Profile,
        Tags,
        Community
    }
    
    void Start()
    {
        User user = FileAndNetworkUtils.currentUser;
        if (user.communityId != -1)
            communityText.text = FileAndNetworkUtils.getObjectFromApi<Community>("/api/CommunitiesAPI/" + user.communityId).name;
        else
            communityText.text = "None yet";
        Debug.Log("USER INFOS : ");
        Debug.Log(user.nickName);
        Debug.Log(user.community);
        Debug.Log(user.email);
        nameText.text = user.nickName;
        emailText.text = user.email;
        phoneText.text = user.phoneNumber;
        socialsText.text = user.socialMedia;
        enterpriseText.text = user.enterprise;
        missionText.text = user.mission;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
