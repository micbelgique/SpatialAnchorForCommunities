using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfilePagesScript : MonoBehaviour
{
    [SerializeField] private Button ProfileButton;
    [SerializeField] private Button TagsButton;
    [SerializeField] private Button CommunityButton;

    private State _state = State.Profile;
    private static readonly int CommunityToProfile = Animator.StringToHash("CommunityToProfile");
    private static readonly int CommunityToTags = Animator.StringToHash("CommunityToTags");
    private static readonly int ProfileToCommunity = Animator.StringToHash("ProfileToCommunity");
    private static readonly int TagsToCommunity = Animator.StringToHash("TagsToCommunity");
    private static readonly int ProfileToTags = Animator.StringToHash("ProfileToTags");
    private static readonly int TagsToProfile = Animator.StringToHash("TagsToProfile");

    public enum State
    {
        Profile,
        Tags,
        Community
    }

    public void OnProfileButtonClicked()
    {
        
        switch (_state)
        {
            case State.Profile:
                break;
            case State.Tags:
                gameObject.GetComponent<Animator>().SetTrigger(TagsToProfile);
                break;
            case State.Community:
                gameObject.GetComponent<Animator>().SetTrigger(CommunityToProfile);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _state = State.Profile;

    }
    
    public void OnTagsButtonClicked()
    {

        switch (_state)
        {
            case State.Profile:
                gameObject.GetComponent<Animator>().SetTrigger(ProfileToTags);
                break;
            case State.Tags:
                break;
            case State.Community:
                gameObject.GetComponent<Animator>().SetTrigger(CommunityToTags);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _state = State.Tags;

    }
    
    public void OnCommunityButtonClicked()
    {

        switch (_state)
        {
            case State.Profile:
                gameObject.GetComponent<Animator>().SetTrigger(ProfileToCommunity);
                break;
            case State.Tags:
                gameObject.GetComponent<Animator>().SetTrigger(TagsToCommunity);
                break;
            case State.Community:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _state = State.Community;

    }

    public void OnBackButtonClicked()
    {
        StartCoroutine("SceneSwitch");
    }

    public void OnLogoutButtonClicked()
    {
        FileAndNetworkUtils.DeleteCurrentUser();
        SceneManager.LoadScene("Register");
    }
    
    IEnumerator SceneSwitch()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("AzureSpatialAnchorsBasicDemo", LoadSceneMode.Additive);
        yield return load;
        SceneManager.UnloadSceneAsync("Profile");
    }
}
