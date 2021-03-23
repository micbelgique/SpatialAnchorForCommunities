using System;
using System.Collections.Generic;
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
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AzureSpatialTest : MonoBehaviour
{

    #region Variables
    private bool startedAsa = false;
    private bool isErrorActive = false;
    private string model = "visit-card";
    public static GameObject[] otherObjects;
    private AnchorLocateCriteria anchorLocateCriteria = null;

    private Button saveButton = null;
    private Text feedbackBox;

    private bool hasSaved, isRotatingLeft, isRotatingRight;
    
    public delegate void MyEventHandler(GameObject e);
    public event MyEventHandler EventTriggered;
    #endregion // Private Variables

    #region Unity Inspector Variables
    [SerializeField] private GameObject anchoredObjectPrefab = null;
    [SerializeField] private GameObject businessCardPrefab = null;
    [SerializeField] private GameObject communityPrefab = null;
    [SerializeField] private Camera snapshotCam = null;

    [SerializeField] private SpatialAnchorManager cloudManager = null;

    [SerializeField] private Button RotateLeftButton;
    [SerializeField] private Button RotateRightButton;
    
    [SerializeField] private Button CommunityButton;
    [SerializeField] private Button BusinessCardButton;
    [SerializeField] private TMP_Dropdown OtherObjectsTMPDropdown;

    private string ApiURL = "http://hiw-communities.azurewebsites.net";
    
    private CloudSpatialAnchorWatcher currentWatcher;

    #endregion // Unity Inspector Variables
    
    #region Properties
    public GameObject AnchoredObjectPrefab { get { return anchoredObjectPrefab; } }
    public SpatialAnchorManager CloudManager { get { return cloudManager; } }
    private GameObject SpawnedObject { get; set; }

    public int userID { get; set; }

    private ReferencePointCreator ReferencePointCreator { get; set; }

    #endregion // Properties

    // Start is called before the first frame update
    void Start()
    {
        //GETTING USER ID
        /*string path;
        
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
        
        Debug.Log("DATA PATH ANDROID : " + path);
        
        
        string _filePath= path + "/userID.txt";
        Debug.Log("DATA PATH : " + _filePath);
        
        //Debug.Log("DATA PATH : " + Application.dataPath + "/userID.txt");
        FileStream _file = new System.IO.FileStream(_filePath,FileMode.OpenOrCreate);
        if (_file.Length < 1)
        {
            Debug.Log("CREATING USERID.TXT");
            userID = "John Doe";
            byte[] bytesCreate = Encoding.ASCII.GetBytes(userID);
            _file.Write(bytesCreate, 0, bytesCreate.Length); //Write
        }
        else
        {
            Debug.Log("READING USERID.TXT");
            byte[] bytesRead = new byte[_file.Length];
            _file.Read(bytesRead, 0, bytesRead.Length);
            userID = Encoding.ASCII.GetString(bytesRead);
        }

        Debug.Log("USERID : " + userID);*/

        feedbackBox = XRUXPicker.Instance.GetFeedbackText();
        saveButton = XRUXPicker.Instance.GetDemoButton();

        ReferencePointCreator = FindObjectOfType<ReferencePointCreator>();
        ReferencePointCreator.OnObjectPlacement += ReferencePointCreator_OnObjectPlacement;

        feedbackBox.text = "Starting Session";
        saveButton.gameObject.SetActive(false);
        if (ARSession.state == ARSessionState.SessionTracking)
        {
            var startAzureSession = StartAzureSession();
        }
        else
        {
            ARSession.stateChanged += ARSession_stateChanged;
        }

        
        Debug.Log("name");

        SnapshotCamera snapCam = snapshotCam.GetComponent<SnapshotCamera>();
        snapCam = SnapshotCamera.MakeSnapshotCamera();
        snapCam.defaultScale = new Vector3(0.3f, 0.3f, 0.3f);
        Texture2D buttonImage = snapCam.TakePrefabSnapshot(Resources.Load("Prefabs/Models/Communites/LOGO 3D Variant", typeof(GameObject)) as GameObject, Color.gray);
        Sprite image = Sprite.Create (buttonImage, new Rect (0, 0, 128, 128), new Vector2 ());
        CommunityButton.image.sprite = image;
        
        snapCam.defaultScale = new Vector3(5,5,5);
        buttonImage = snapCam.TakePrefabSnapshot(Resources.Load("Prefabs/Models/BusinessCards/ArBusinessCardUp", typeof(GameObject)) as GameObject, Color.gray);
        image = Sprite.Create (buttonImage, new Rect (0, 0, 128, 128), new Vector2 ());
        BusinessCardButton.image.sprite = image;
        
        OtherObjectsTMPDropdown.options.Clear();
        otherObjects = Resources.LoadAll<GameObject>("Prefabs/Models/Others");
        foreach (GameObject go in otherObjects)
        {
            Debug.Log(go.name);
            snapCam.defaultScale = new Vector3(18,18,18);
            buttonImage = snapCam.TakePrefabSnapshot(go, Color.gray);
            image = Sprite.Create (buttonImage, new Rect (0, 0, 128, 128), new Vector2 ());

            OtherObjectsTMPDropdown.options.Add (new TMP_Dropdown.OptionData() {text = go.name, image = image});
        }

        OtherObjectsTMPDropdown.RefreshShownValue();
    }

    private void Update()
    {
        if(isRotatingLeft)
            SpawnedObject.transform.Rotate(0, -3, 0);
        if(isRotatingRight)
            SpawnedObject.transform.Rotate(0, 3, 0);
    }

    private void SetImageButton(Button button, string modelID)
    {
        GameObject prefab = GetModelFromModelID(modelID);
        SnapshotCamera snapCam = snapshotCam.GetComponent<SnapshotCamera>();
        snapCam = SnapshotCamera.MakeSnapshotCamera();
        Texture2D buttonImage = snapCam.TakePrefabSnapshot(prefab);
        Sprite image = Sprite.Create (buttonImage, new Rect (0, 0, 128, 128), new Vector2 ());
        CommunityButton.image.sprite = image;
    }
    
    public void onPointerDownRotateLeftButton()
    {
        isRotatingLeft = true;
    }
    public void onPointerUpRotateLeftButton()
    {
        isRotatingLeft = false;
    }
    public void onPointerDownRotateRightButton()
    {
        isRotatingRight = true;
    }
    public void onPointerUpRotateRightButton()
    {
        isRotatingRight = false;
    }

    public void onCommunityButtonClicked()
    {
        this.model = "community";
    }
    public void onBusinessCardButtonClicked()
    {
        this.model = "visit-card";
    }
    public void OnOtherModelSelected()
    {
        Debug.Log("Selected " + OtherObjectsTMPDropdown.options[OtherObjectsTMPDropdown.value].text);
        model = OtherObjectsTMPDropdown.options[OtherObjectsTMPDropdown.value].text;
    }
    private void ARSession_stateChanged(ARSessionStateChangedEventArgs obj)
    {
        Debug.Log($"ar session {obj.state}");
        if (obj.state == ARSessionState.SessionTracking && !startedAsa)
        {
            var startAzureSession = StartAzureSession();
        }
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

    protected virtual async Task SaveCurrentObjectAnchorToCloudAsync()
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
                
                //POSTING ANCHOR
                string jsonString = JsonUtility.ToJson(new  AnchorDTO {identifier = currentCloudAnchor.Identifier, modelID = this.model, userID = this.userID});
                Debug.Log("JSON : " + jsonString);

                using (HttpClient client = new HttpClient())
                {
                    HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    Debug.Log("MESSAGE : " + httpContent.ReadAsStringAsync());
                    HttpResponseMessage httpResponseMessage = client.PostAsync(ApiURL + "/api/AnchorsAPI", httpContent).Result;
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
        RotateLeftButton.gameObject.SetActive(false);
        RotateRightButton.gameObject.SetActive(false);
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

    public async void OnSaveButtonClicked()
    {
        saveButton.interactable = false;
        RotateLeftButton.interactable = false;
        RotateRightButton.interactable = false;
        hasSaved = true;
        await SaveCurrentObjectAnchorToCloudAsync();
    }

    
    #region EventHandlers
    private void CloudManager_Error(object sender, SessionErrorEventArgs args)
    {
        Debug.Log($"Cloud Error: {args.ErrorMessage}");
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
                SpawnAnchoredObject(args.Anchor.Identifier, anchorPose.position, anchorPose.rotation);
                
            });
        }
    }
    
    private void CloudManager_LocateAnchorsCompleted(object sender, LocateAnchorsCompletedEventArgs args)
    {
        //throw new NotImplementedException();
    }

    private void CloudManager_SessionUpdated(object sender, SessionUpdatedEventArgs args)
    {
        Debug.Log($"Cloud Session: {args.Status}");
        //throw new NotImplementedException();
    }

    private void ReferencePointCreator_OnObjectPlacement(Transform transform)
    {
        if (hasSaved) return;

        if (SpawnedObject != null)
        {
            Destroy(SpawnedObject);
        }
        
        SpawnNewObject(transform.position, transform.rotation);
        //GET INFO BASED ON USERID
        RotateLeftButton.gameObject.SetActive(true);
        RotateRightButton.gameObject.SetActive(true);
        

        saveButton.gameObject.SetActive(true);
    }

    private void SpawnAnchoredObject(string AnchorId, Vector3 position, Quaternion rotation)
    {
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(ApiURL + "/api/AnchorsAPI/" + AnchorId);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Anchor anchorFound = JsonUtility.FromJson<Anchor>(jsonResponse);
        Debug.Log("JSON RESPONSE : " + jsonResponse);

        GameObject spawned;
        if (anchorFound.model.Equals("visit-card"))
        {
            spawned = Instantiate(businessCardPrefab, position, rotation);
            //Debug.Log("ANCHOR ID : " + spawned.GetComponent<BusinessCardScript>().AnchorId);
            spawned.GetComponent<BusinessCardScript>().UpdateInfos();
        }
        else
        {
            spawned = Instantiate(GetModelFromModelID(anchorFound.model), position, rotation);
        }
        EventTriggered?.Invoke(spawned);
    }
    
    private void SpawnNewObject(Vector3 position, Quaternion rotation)
    {
        GameObject newGameObject;
        if (model.Equals("visit-card"))
        {
            newGameObject = Instantiate(businessCardPrefab, position, rotation);
            newGameObject.GetComponent<BusinessCardScript>().UpdateInfos();
        }
        else
        {
            newGameObject = Instantiate(GetModelFromModelID(this.model), position, rotation);
        }
        //GET INFO BASED ON USERID
        newGameObject.AddComponent<CloudNativeAnchor>();
        SpawnedObject = newGameObject;
    }

    private GameObject GetModelFromModelID(string modelID)
    {
        //TO DO : get model from api
        Debug.Log("MODEL ID : " + modelID);
        if(model.Equals("community"))
            return communityPrefab;
        return Resources.Load<GameObject>("Prefabs/Models/Others/" + modelID);
    }
    
    #endregion // EventHandlers
    
    

    
    

}
