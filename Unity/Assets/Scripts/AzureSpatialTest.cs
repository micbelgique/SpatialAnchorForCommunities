using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Azure.SpatialAnchors.Unity;
using UnityEngine;
using System.Threading.Tasks;
using Microsoft.Azure.SpatialAnchors;
using UnityEngine.XR.ARFoundation;
using Microsoft.Azure.SpatialAnchors.Unity.Examples;
using UnityEngine.Android;
using UnityEngine.UI;

public class AzureSpatialTest : MonoBehaviour
{

    #region Variables

    private CloudSpatialAnchor _savedAnchor;
    private bool startedAsa = false;
    private bool isErrorActive = false;
    public string model = "visit-card";
    public static GameObject[] otherObjects;
    private AnchorLocateCriteria anchorLocateCriteria = null;
    private Text feedbackBox;
    public bool hasSaved, isRotatingLeft, isRotatingRight;
    public delegate void MyEventHandler(GameObject e);
    public event MyEventHandler EventTriggered;
    #endregion // Private Variables

    #region Unity Inspector Variables
    [SerializeField] private Button saveButton;
    [SerializeField] private Button rotateLeftButton;
    [SerializeField] private Button rotateRightButton;
    [SerializeField] private GameObject anchoredObjectPrefab = null;
    [SerializeField] private SpatialAnchorManager cloudManager = null;
    [SerializeField] private string ApiURL;
    private CloudSpatialAnchorWatcher currentWatcher;

    private UIHandlerScript _uiHandler;

    #endregion // Unity Inspector Variables
    
    #region Properties
    public GameObject AnchoredObjectPrefab { get { return anchoredObjectPrefab; } }
    public SpatialAnchorManager CloudManager { get { return cloudManager; } }
    private GameObject SpawnedObject { get; set; }
    public string userID { get; set; }
    public User currentUser { get; set; }
    private ReferencePointCreator ReferencePointCreator { get; set; }
    #endregion // Properties
    
    void Start()
    {
        _uiHandler = this.GetComponent<UIHandlerScript>();
        
        StartCoroutine(StartLocation());
        currentUser = FileAndNetworkUtils.getCurrentUser();
        /*Debug.Log("CURRENT USER : " + this.currentUser.nickName);
        Debug.Log("CURRENT USER JOB : " + currentUser.mission);
        Debug.Log("CURRENT USER ID : " + currentUser.id);
        
        saveButton = XRUXPicker.Instance.GetDemoButton();*/

        feedbackBox = XRUXPicker.Instance.GetFeedbackText();

        ReferencePointCreator = FindObjectOfType<ReferencePointCreator>();
        ReferencePointCreator.OnObjectPlacement += ReferencePointCreator_OnObjectPlacement;

        feedbackBox.text = "Starting Session";
        if (ARSession.state == ARSessionState.SessionTracking)
        {
            var startAzureSession = StartAzureSession();
        }
        else
        {
            ARSession.stateChanged += ARSession_stateChanged;
        }
        
    }

    private void Update()
    {
        if(isRotatingLeft)
            SpawnedObject.transform.Rotate(0, -3, 0);
        if(isRotatingRight)
            SpawnedObject.transform.Rotate(0, 3, 0);
    }

    #region AzureSpatialAnchors

    private void ARSession_stateChanged(ARSessionStateChangedEventArgs obj)
    {
        Debug.Log($"ar session {obj.state}");
        if (obj.state == ARSessionState.SessionTracking && !startedAsa)
        {
            var startAzureSession = StartAzureSession();
        }
    }

    public void SendScreenCapture()
    {
        ScreenCapture.TakeScreenShot_static(Screen.width, Screen.height, _savedAnchor.Identifier);
        _uiHandler.SetScreenCaptureVisible(false);
    }
    
    public async Task StartAzureSession()
    {
        startedAsa = true;
        anchorLocateCriteria = new AnchorLocateCriteria();
        
        CloudManager.SessionUpdated += CloudManager_SessionUpdated;
        CloudManager.AnchorLocated += CloudManagerOnAnchorLocated;
        CloudManager.LocateAnchorsCompleted += CloudManager_LocateAnchorsCompleted;
        CloudManager.LogDebug += CloudManagerOnLogDebug;
        CloudManager.Error += CloudManager_Error;
        
        await CloudManager.CreateSessionAsync();

        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(ApiURL + "/api/AnchorsAPI"); //.Create(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", CityId, API_KEY));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        var result = JsonUtility.FromJson<Response<Anchor>>(jsonResponse);

        anchorLocateCriteria.Identifiers = result.data.Select(anchor => anchor.identifier).ToArray();

        await CloudManager.StartSessionAsync();

        if (currentWatcher != null)
        {
            currentWatcher.Stop();
            currentWatcher = null;
        }
        currentWatcher = CreateWatcher();
        if (currentWatcher == null)
        {
            Debug.Log("Either cloudmanager or session is null, should not be here!");
        }

    }
    
    protected CloudSpatialAnchorWatcher CreateWatcher()
    {
        if ((CloudManager != null) && (CloudManager.Session != null) && anchorLocateCriteria.Identifiers.Length > 0)
        {
            return CloudManager.Session.CreateWatcher(anchorLocateCriteria);
        }
        else
        {
            return null;
        }
    }

    public virtual async Task SaveCurrentObjectAnchorToCloudAsync()
    {
        feedbackBox.text = "Saving Spawned Game Object";
        // Get the cloud-native anchor behavior
        CloudNativeAnchor cna = SpawnedObject.GetComponent<CloudNativeAnchor>();
        
        
        // If the cloud portion of the anchor hasn't been created yet, create it
        if (cna.CloudAnchor == null) { cna.NativeToCloud(); }

        // Get the cloud portion of the anchor
        CloudSpatialAnchor cloudAnchor = cna.CloudAnchor;

        // In this sample app we delete the cloud anchor explicitly, but here we show how to set an anchor to expire automatically
        cloudAnchor.Expiration = DateTimeOffset.Now.AddDays(7);

        String test = CloudManager.SessionStatus == null ? "null" : "not null";
        feedbackBox.text = $"Cloud manager is ready: {CloudManager.IsReadyForCreate} and status: {test}";

        while (!CloudManager.IsReadyForCreate)
        {
            await Task.Delay(330);
            float createProgress = CloudManager.SessionStatus.RecommendedForCreateProgress;
            feedbackBox.text = $"Move your device to capture more environment data: {createProgress:0%}";
        }

        bool success = false;

        feedbackBox.text = "Saving...";

        try
        {
            // Actually save
            await CloudManager.CreateAnchorAsync(cloudAnchor);

            // Store
            var currentCloudAnchor = cloudAnchor;

            // Success?
            success = currentCloudAnchor != null;

            if (success && !isErrorActive)
            {
                // Await override, which may perform additional tasks
                // such as storing the key in the AnchorExchanger
                feedbackBox.text = $"Saved Anchor succesfully: {currentCloudAnchor.Identifier}";
                _savedAnchor = currentCloudAnchor;
                
                //POSTING ANCHOR
                string jsonString = JsonUtility.ToJson(new  AnchorDTO {identifier = currentCloudAnchor.Identifier, model = this.model, userId = this.currentUser.id, longitude = Input.location.lastData.longitude, latitude = Input.location.lastData.latitude, srid = 4326});
                Debug.Log("JSON ANCHOR : " + jsonString);

                using (HttpClient client = new HttpClient())
                {
                    HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    print("MESSAGE : " + httpContent.ReadAsStringAsync().Result);
                    HttpResponseMessage httpResponseMessage = client.PostAsync($"{ApiURL}/api/AnchorsAPI", httpContent).Result;
                    if(httpResponseMessage.IsSuccessStatusCode)
                    {
                        HttpContent content = httpResponseMessage.Content;
                        string response = content.ReadAsStringAsync().Result;
                        feedbackBox.text += $"\n API Response: {response}";
                    }
                    else
                    {
                        feedbackBox.text += $"\n API ERROR Response: {httpResponseMessage.StatusCode}";
                    }
                }
                _uiHandler.SetScreenCaptureVisible(true);
                await OnSaveCloudAnchorSuccessfulAsync();
            }
            else
            {
                OnSaveCloudAnchorFailed(new Exception("Failed to save, but no exception was thrown."));
            }
        }
        catch (Exception ex)
        {
            OnSaveCloudAnchorFailed(ex);
        }
    }

    protected virtual Task OnSaveCloudAnchorSuccessfulAsync()
    {
        // To be overridden.
        saveButton.gameObject.SetActive(false);
        rotateLeftButton.gameObject.SetActive(false);
        rotateRightButton.gameObject.SetActive(false);
        return Task.CompletedTask;
    }

    protected virtual void OnSaveCloudAnchorFailed(Exception exception)
    {
        // we will block the next step to show the exception message in the UI.
        isErrorActive = true;
        feedbackBox.text = $"Error : {exception.ToString()}";
        hasSaved = false;

        //UnityDispatcher.InvokeOnAppThread(() => this.feedBack.text = string.Format("Error: {0}", exception.ToString()));
    }


    #endregion

    #region EventHandlers
    private void CloudManager_Error(object sender, SessionErrorEventArgs args)
    {
        throw new NotImplementedException();
    }

    private void CloudManagerOnLogDebug(object sender, OnLogDebugEventArgs args)
    {
        //throw new NotImplementedException();
    }

    private void CloudManagerOnAnchorLocated(object sender, AnchorLocatedEventArgs args)
    {
        if (args.Status == LocateAnchorStatus.Located)
        {
            feedbackBox.text = $"Found anchor: {args.Anchor.Identifier}";
            var currentCloudAnchor = args.Anchor;
            UnityDispatcher.InvokeOnAppThread(() =>
            {
                Pose anchorPose = Pose.identity;
                
#if UNITY_ANDROID || UNITY_IOS
                anchorPose = currentCloudAnchor.GetPose();
#endif
                // HoloLens: The position will be set based on the unityARUserAnchor that was located.
                //SpawnOrMoveCurrentAnchoredObject(anchorPose.position, anchorPose.rotation);
                GameObject spawned = gameObject.GetComponent<ModelSpawnerScript>()
                    .SpawnAnchoredObject(args.Anchor.Identifier, anchorPose.position, anchorPose.rotation);
                EventTriggered?.Invoke(spawned);
            });
        }
    }
    
    private void CloudManager_LocateAnchorsCompleted(object sender, LocateAnchorsCompletedEventArgs args)
    {
        //throw new NotImplementedException();
    }

    private void CloudManager_SessionUpdated(object sender, SessionUpdatedEventArgs args)
    {
        Debug.Log(args.Status);
        //throw new NotImplementedException();
    }

    private void ReferencePointCreator_OnObjectPlacement(Transform transform)
    {
        if (hasSaved) return;

        saveButton.gameObject.SetActive(true);
        rotateLeftButton.gameObject.SetActive(true);
        rotateRightButton.gameObject.SetActive(true);
        if (SpawnedObject != null)
        {
            Destroy(SpawnedObject);
        }
        
        //SpawnNewObject(transform.position, transform.rotation);
        SpawnedObject = gameObject.GetComponent<ModelSpawnerScript>().SpawnNewObject(model, transform.position, transform.rotation);
        //GET INFO BASED ON USERID
        

    }
    #endregion // EventHandlers
    
    IEnumerator StartLocation()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            Debug.Log("LOCATION: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

    }
}
