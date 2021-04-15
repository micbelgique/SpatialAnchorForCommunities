using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.Android;
using Button = UnityEngine.UI.Button;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    [Header("Fields")] 
    [SerializeField] public TMP_InputField nickname;
    [SerializeField] public TMP_InputField email;
    [SerializeField] public TMP_InputField phone;
    [SerializeField] public TMP_InputField function;
    [SerializeField] public TMP_InputField mission;
    [SerializeField] public TMP_InputField password;

    [Header("Buttons")] 
    [SerializeField] public Button validateButton;

    [Header("Config")] 
    [SerializeField] public HIWConfig config;
    
    [Header("Errors")] 
    [SerializeField] public TMP_Text loginError;
    [SerializeField] public TMP_Text registerError;
    
    void Start()
    {
        Permission.RequestUserPermission(Permission.FineLocation);
        validateButton.interactable = false;
        if(FileAndNetworkUtils.getCurrentUser() != null)
            SceneManager.LoadScene("AzureSpatialAnchorsBasicDemo", LoadSceneMode.Single); //AzureSpatialAnchorsBasicDemo
    }

    private void FixedUpdate()
    {
        if (!String.IsNullOrEmpty(nickname.text)
            && !String.IsNullOrEmpty(email.text))
            validateButton.interactable = true;
        else
            validateButton.interactable = false;
    }

    public async void OnButtonSelected()
    {
        clearErrors();
        
        Debug.Log($"nickname = [{nickname.text}]");
        Debug.Log($"email = [{email.text}]");
        Debug.Log($"phone = [{phone.text}]");
        Debug.Log($"mission = [{mission.text}]");
        Debug.Log($"function = [{function.text}]");
        Debug.Log($"function = [{password.text}]");

        using (HttpClient client = new HttpClient())
        {
            User user = new User(
                this.nickname.text,
                this.email.text,
                this.phone.text,
                "social",
                this.function.text,
                this.mission.text,
                this.password.text);
            
            string jsonString = JsonUtility.ToJson(user);

            HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            Debug.Log("MESSAGE : " + jsonString);
            HttpResponseMessage httpResponseMessage = client.PostAsync($"{config.apiBaseUrl}/api/UsersAPI", httpContent).Result;
            if(httpResponseMessage.IsSuccessStatusCode)
            {
                HttpContent content = httpResponseMessage.Content;
                string response = content.ReadAsStringAsync().Result;
                //todo: register to local storage + send to next page
                Debug.Log(response);
                User savedUser = JsonUtility.FromJson<User>(response);
                Debug.Log("USER TO SAVE : " + savedUser.id);
                FileAndNetworkUtils.SaveUser(savedUser.id);
                SceneManager.LoadScene("AzureSpatialAnchorsBasicDemo", LoadSceneMode.Single); //AzureSpatialAnchorsBasicDemo
            }
            else
            {
                //todo: show user error
            }
        }
    }

    public void OnLoginButtonClicked()
    {
        clearErrors();
        /*string jsonString = JsonUtility.ToJson(user);
        
        HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        Debug.Log("MESSAGE : " + jsonString);
        HttpResponseMessage httpResponseMessage = client.PostAsync($"{config.apiBaseUrl}/api/UsersAPI", httpContent).Result;
        if(httpResponseMessage.IsSuccessStatusCode)
        {
            HttpContent content = httpResponseMessage.Content;
            string response = content.ReadAsStringAsync().Result;
            //todo: register to local storage + send to next page
            Debug.Log(response);
            User savedUser = JsonUtility.FromJson<User>(response);
            Debug.Log("USER TO SAVE : " + savedUser.id);
            FileReaderScript.SaveUser(savedUser.id);
            SceneManager.LoadScene("AzureSpatialTest", LoadSceneMode.Single); //AzureSpatialAnchorsBasicDemo
        }
        else
        {
            //todo: show user error
        }*/

        loginError.text = "Error : email or password incorrect";
    }

    public void clearErrors()
    {
        loginError.text = "";
        registerError.text = "";
    }
    
}
