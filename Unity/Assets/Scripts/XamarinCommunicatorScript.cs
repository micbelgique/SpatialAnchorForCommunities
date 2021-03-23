using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XamarinCommunicatorScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ASK XAMARIN USER ID
        SetUserID(1);
        
        SendMsgToNative(new NativeMessageObject
        {
            functionName = "GetUserID",
            functionArg = new string[] {}
        });
        
    }

    public void SetUserID(int userID)
    {
        GameObject.Find("AzureSpatialAnchors").GetComponent<AzureSpatialTest>().userID = userID;
    }
    
    private void SendMsgToNative(NativeMessageObject msg)
    {
        //#if UNITY_ANDROID
        try
        {
            string json = JsonUtility.ToJson(msg);
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "com.mic.business_card");
            intent.Call<AndroidJavaObject>("putExtra", "data", json);
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call("sendBroadcast", intent);
        }
        catch (Exception ex)
        {
            Debug.Log($"---- SendMsgToNative Exception: {ex.Message} - {ex.StackTrace}");
        }
        //#endif

    }
    
    [Serializable]
    public class NativeMessageObject
    {
        public string functionName;
        public string[] functionArg;
    }
}
