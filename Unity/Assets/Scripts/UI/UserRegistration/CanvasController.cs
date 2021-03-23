using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using UnityEngine;
using TMPro;
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

    [Header("Buttons")] 
    [SerializeField] public Button validateButton;

    [Header("Config")] 
    [SerializeField] public HIWConfig config;
    
    void Start()
    {
        validateButton.interactable = false;
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
        Debug.Log($"nickame = [{nickname.text}]");
        Debug.Log($"email = [{email.text}]");
        Debug.Log($"phone = [{phone.text}]");
        Debug.Log($"mission = [{mission.text}]");
        Debug.Log($"function = [{function.text}]");

        //SceneManager.LoadScene("AzureSpatialAnchorsBasicDemo", LoadSceneMode.Single);

        using (HttpClient client = new HttpClient())
        {
            User user = new User(
                this.nickname.text,
                this.email.text,
                this.phone.text,
                "social",
                this.function.text,
                this.mission.text);
            
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
                SceneManager.LoadScene("Communities", LoadSceneMode.Single); //AzureSpatialAnchorsBasicDemo
            }
            else
            {
                //todo: show user error
            }
        }
    }

    private void SaveUserID(User user)
    {
        //GETTING USER ID
        string path;
        
        #if UNITY_ANDROID && !UNITY_EDITOR
                
                using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        path = jo.Call<AndroidJavaObject>("getDir", "", 0).Call<string>("getAbsolutePath");
                    }
                }
                path = path.Replace("app_", "");
                path += "files";
        #else
                path = Path.GetFullPath(Application.persistentDataPath);
        #endif

        string _filePath= path + "/userID.txt";
        
        FileStream _file = new System.IO.FileStream(_filePath,FileMode.OpenOrCreate);
        byte[] bytesCreate = Encoding.ASCII.GetBytes(user.id.ToString()); 
        _file.Write(bytesCreate, 0, bytesCreate.Length); //Write

    }
    
}
