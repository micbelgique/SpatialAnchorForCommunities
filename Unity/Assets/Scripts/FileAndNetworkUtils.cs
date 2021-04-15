using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using Object = System.Object;

public class FileAndNetworkUtils : MonoBehaviour
{
    private static string userID;
    
    public static User currentUser { get; set; }

    public const string ApiURL = "http://hiw-communities.azurewebsites.net";

    public static User getCurrentUser()
    {
        //GETTING USER ID
        string path = GetPath();
        
        Debug.Log("DATA PATH ANDROID : " + path);
        
        string _filePath= path + "/user.txt";
        Debug.Log("DATA PATH : " + _filePath);


        try
        {
            FileStream _file = new System.IO.FileStream(_filePath,FileMode.OpenOrCreate);
            if (_file.Length < 1)
            {
                Debug.Log("CREATING USERID.TXT");
                return null;
            }
            else
            {
                Debug.Log("READING USERID.TXT");
                byte[] bytesRead = new byte[_file.Length];
                _file.Read(bytesRead, 0, bytesRead.Length);
                userID = Encoding.ASCII.GetString(bytesRead);
            }
            _file.Close();
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e);
            return null;
        }
        
        Debug.Log("USERID : " + userID);
        try
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://hiw-communities.azurewebsites.net" + "/api/UsersAPI/" + userID);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            currentUser = JsonUtility.FromJson<User>(jsonResponse);
        }
        catch (WebException e)
        {
            return null;
        }
//        Debug.Log("USER NAME : " + currentUser.nickName);
  //      Debug.Log("USER JOB : " + currentUser.mission);


        return currentUser;
    }

    public static void DeleteCurrentUser()
    {
        SaveUser("");
    }

    public static void SaveUser(string userID)
    {
        //GETTING USER ID
        string path = GetPath();
        
        Debug.Log("DATA PATH ANDROID : " + path);

        string _filePath= path + "/user.txt";
        
        Debug.Log("DATA PATH : " + _filePath);
        Debug.Log("CREATING USERID.TXT");
        Debug.Log("USERID : " + userID);
        
        FileStream _file = new System.IO.FileStream(_filePath,FileMode.Create);
        byte[] bytesCreate = Encoding.ASCII.GetBytes(userID);
        _file.Write(bytesCreate, 0, bytesCreate.Length); //Write
        _file.Close();
    }

    public static string GetPath()
    {
        string path;
        
/*#if UNITY_ANDROID && !UNITY_EDITOR
        
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                path = jo.Call<AndroidJavaObject>("getDir", "", 0).Call<string>("getAbsolutePath");
            }
        }
        path = path.Replace("app_", "");
        path += "files";
#else*/
        path = Path.GetFullPath(Application.persistentDataPath);
//#endif
        return path;
    }

    public static T getObjectFromApi<T>(string endpoint)
    {
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://hiw-communities.azurewebsites.net" + endpoint);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        return JsonUtility.FromJson<T>(jsonResponse);
    }

    public static HttpStatusCode postObjectToApi(string endpoint, object objectToPost)
    {
        string jsonString = JsonUtility.ToJson(objectToPost);
        Debug.Log("POST JSON : " + jsonString);

        using (HttpClient client = new HttpClient())
        {
            HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            print("POST MESSAGE : " + httpContent.ReadAsStringAsync().Result);
            HttpResponseMessage httpResponseMessage = client.PostAsync(ApiURL + endpoint, httpContent).Result;
            return httpResponseMessage.StatusCode;
        }
    }
    
    public static Texture2D DownloadImage(string MediaUrl)
    {   
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        request.SendWebRequest();
        if(request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            return null;
        }

        Stopwatch sw = new Stopwatch();
        while (!request.downloadHandler.isDone)
        {
            if (sw.ElapsedMilliseconds > 5000) return null;
        }
        return ((DownloadHandlerTexture) request.downloadHandler).texture;
    }
    
    
}
