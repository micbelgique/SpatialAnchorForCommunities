using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Networking;

public class ScreenCapture : MonoBehaviour
{
    [SerializeField] private Canvas _uiCanvas;
    [SerializeField] private string ApiBaseUrl;
    
    private static ScreenCapture _instance;
    private Camera perspectiveCam;
    private bool _takeScreenshotOnNextFrame;
    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();
    
    


    private void Start()
    {
        _instance = this;
        perspectiveCam = Camera.main; 
        //perspectiveCam = gameObject.GetComponent<Camera>();
    }
    
    private IEnumerator TakeScreenShotCoRoutine(int width, int height, string anchorId)
    {
        yield return frameEnd;

        _instance._uiCanvas.gameObject.SetActive(false);
        Texture2D renderResult =
            new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        renderResult.ReadPixels(rect, 0,0);
            
        Stream stream = new MemoryStream(renderResult.EncodeToPNG());
        _instance._uiCanvas.gameObject.SetActive(true);
        Upload(stream, anchorId);
    }

    private void TakeScreenshot(int width, int height, string anchorId)
    {
        StartCoroutine(TakeScreenShotCoRoutine(width, height, anchorId));
    }

    public static void TakeScreenShot_static(int width, int heigth, string anchorId)
    {
        
        _instance.TakeScreenshot(width, heigth, anchorId);
        
    }

    private async void Upload(Stream stream, string anchorId)
    {
        stream.Seek(0, SeekOrigin.Begin);
        using (var httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(ApiBaseUrl);
            
            var request = new HttpRequestMessage(HttpMethod.Post, $"ImageAPI/uploadTest?anchorId={anchorId}");

            using (var requestContent = new StreamContent(stream))
            {
                request.Content = requestContent;

                using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStreamAsync();
                }
                
            }
        }
        
    }

}

