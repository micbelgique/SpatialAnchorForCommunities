using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UIHandlerScript : MonoBehaviour
{
    public static GameObject[] otherObjects;
    private SnapshotCamera snapCam;
    private string userCommunity; 
    
    [SerializeField] private Button RotateLeftButton;
    [SerializeField] private Button RotateRightButton;
    [SerializeField] private Button CommunityButton;
    [SerializeField] private Button BusinessCardButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private TMP_Dropdown OtherObjectsTMPDropdown;
    [SerializeField] private Camera snapshotCam = null;
    [SerializeField] private GameObject businessCardPrefab;
    [SerializeField] private AzureSpatialTest AST;
    [SerializeField] private TMP_Text communityText;
    [SerializeField] private Button screenCapturePanel;

    // Start is called before the first frame update
    void Start()
    {
        screenCapturePanel.gameObject.SetActive(false);
        
        snapCam = snapshotCam.GetComponent<SnapshotCamera>();
        snapCam = SnapshotCamera.MakeSnapshotCamera();
        
        BusinessCardButton.image.sprite = GetSpriteFromModel(businessCardPrefab, 6);
        BusinessCardButton.gameObject.SetActive(true);

        if (gameObject.GetComponent<AzureSpatialTest>().currentUser.communityId == -1)
        {
            CommunityButton.gameObject.SetActive(false);
            communityText.gameObject.SetActive(false);
        }
        else
            CommunityButton.image.sprite = GetSpriteFromModelID("community", 0.3f);
        //userCommunity = gameObject.GetComponent<AzureSpatialTest>().currentUser.community;
        
        
        saveButton.gameObject.SetActive(false);
        RotateLeftButton.gameObject.SetActive(false);
        RotateRightButton.gameObject.SetActive(false);
        OtherObjectsTMPDropdown.options.Clear();
        otherObjects = Resources.LoadAll<GameObject>("Prefabs/Models/Others");
        foreach (GameObject go in otherObjects)
        {
            Debug.Log(go.name);
            snapCam.defaultScale = new Vector3(18,18,18);
            var buttonImage = snapCam.TakePrefabSnapshot(go, new Color(0, 0.475f, 0.839f));
            var image = Sprite.Create (buttonImage, new Rect (0, 0, 128, 128), new Vector2 ());

            OtherObjectsTMPDropdown.options.Add (new TMP_Dropdown.OptionData() {text = go.name, image = image});
        }

        OtherObjectsTMPDropdown.RefreshShownValue();
    }
    
    void Update () {
        // Code for OnMouseDown in the iPhone. Unquote to test.
        RaycastHit hit = new RaycastHit();
        for (int i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began)) {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out hit)) {
                    hit.transform.gameObject.GetComponent<AnchorInterraction>().OnItemClicked();
                }
            }
        }
    }

    public void onPointerDownRotateLeftButton()
    {
        AST.isRotatingLeft = true;
    }
    public void onPointerUpRotateLeftButton()
    {
        AST.isRotatingLeft = false;
    }
    public void onPointerDownRotateRightButton()
    {
        AST.isRotatingRight = true;
    }
    public void onPointerUpRotateRightButton()
    {
        AST.isRotatingRight = false;
    }
    public void onCommunityButtonClicked()
    {
        AST.model = "community";
    }
    public void onBusinessCardButtonClicked()
    {
        AST.model = "visit-card";
    }
    public void OnOtherModelSelected()
    {
        Debug.Log("Selected " + OtherObjectsTMPDropdown.options[OtherObjectsTMPDropdown.value].text);
        AST.model = OtherObjectsTMPDropdown.options[OtherObjectsTMPDropdown.value].text;
    }
    
    public async void OnSaveButtonClicked()
    {
        saveButton.interactable = false;
        RotateLeftButton.gameObject.SetActive(false);
        RotateRightButton.gameObject.SetActive(false);
        AST.hasSaved = true;
        await AST.SaveCurrentObjectAnchorToCloudAsync();
    }
    
    private Sprite GetSpriteFromModelID(string modelID, float scale)
    {
        snapCam.defaultScale = new Vector3(scale, scale, scale);
        Texture2D buttonImage = snapCam.TakePrefabSnapshot(gameObject.GetComponent<ModelSpawnerScript>().GetModelFromModelID(modelID), new Color(0, 0.475f, 0.839f));
        return Sprite.Create (buttonImage, new Rect (0, 0, 128, 128), new Vector2 ());
    }
    
    private Sprite GetSpriteFromModel(GameObject model, float scale)
    {
        snapCam.defaultScale = new Vector3(scale, scale, scale);
        Texture2D buttonImage = snapCam.TakePrefabSnapshot(model, new Color(0, 0.475f, 0.839f));
        return Sprite.Create (buttonImage, new Rect (0, 0, 128, 128), new Vector2 ());
    }

    public void OnProfileButtonClicked()
    {
        StartCoroutine("SceneSwitch");
    }

    public void SetScreenCaptureVisible(bool visible)
    {
        screenCapturePanel.gameObject.SetActive(visible);
    }
    
    IEnumerator SceneSwitch()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("Profile", LoadSceneMode.Additive);
        yield return load;
        SceneManager.UnloadSceneAsync("AzureSpatialAnchorsBasicDemo");
    }
}
