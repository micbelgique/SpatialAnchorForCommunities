using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeepLinkingScript : MonoBehaviour
{
    public static DeepLinkingScript Instance { get; private set; }
    public string deeplinkURL;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;                
            Application.deepLinkActivated += onDeepLinkActivated;
            if (!String.IsNullOrEmpty(Application.absoluteURL))
            {
                // Cold start and Application.absoluteURL not null so process Deep Link.
                onDeepLinkActivated(Application.absoluteURL);
            }
            // Initialize DeepLink Manager global variable.
            else deeplinkURL = "[none]";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
 
    private void onDeepLinkActivated(string url)
    {
        // Update DeepLink Manager global variable, so URL can be accessed from anywhere.
        deeplinkURL = url;
        
// Decode the URL to determine action. 
// In this example, the app expects a link formatted like this:
// unitydl://mylink?scene1
        string[] splittedURL = url.Split('?');
        if (splittedURL.Length <= 1) return;
        FileAndNetworkUtils.SaveUser(splittedURL[1]);
        SceneManager.LoadScene("AzureSpatialAnchorsBasicDemo", LoadSceneMode.Single); //AzureSpatialAnchorsBasicDemo
    }
}