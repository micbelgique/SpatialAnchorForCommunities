using System.IO;
using System.Net;
using Microsoft.Azure.SpatialAnchors.Unity;
using UnityEngine;

public class ModelSpawnerScript : MonoBehaviour
{
    private string ApiURL = "http://hiw-communities.azurewebsites.net";
    
    [SerializeField] private  GameObject businessCardPrefab = null;
    [SerializeField] private  GameObject communityPrefab = null;
    
    public GameObject SpawnAnchoredObject(string AnchorId, Vector3 position, Quaternion rotation)
    {
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(ApiURL + "/api/AnchorsAPI/" + AnchorId);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Anchor anchorFound = JsonUtility.FromJson<Anchor>(jsonResponse);
        Debug.Log("JSON RESPONSE : " + jsonResponse);
        Debug.Log("JSON RESPONSE USER : " + anchorFound.userId);
        
        GameObject spawnedObject =  Instantiate(GetModelFromModelID(anchorFound.model), position, rotation);
        if (anchorFound.model.Equals("visit-card"))
        {
            anchorFound.user = FileAndNetworkUtils.getObjectFromApi<User>("/api/UsersAPI/" + anchorFound.userId);
            Debug.Log("visit card user : " + anchorFound.user.nickName);
            spawnedObject.GetComponent<BusinessCardScript>().UpdateInfos(
                anchorFound.user.nickName,
                anchorFound.user.mission,
                anchorFound.user.email,
                anchorFound.user.phoneNumber,
                anchorFound.user.socialMedia);
        }
        spawnedObject.AddComponent<AnchorInterraction>();
        spawnedObject.GetComponent<AnchorInterraction>().SetAnchorId(anchorFound.identifier);

        return spawnedObject;
    }
    
    public GameObject SpawnNewObject(string model, Vector3 position, Quaternion rotation)
    {
        GameObject newGameObject;
        newGameObject = Instantiate(GetModelFromModelID(model), position, rotation);

        if (model.Equals("visit-card"))
        {
            User currentUser = GameObject.Find("AzureSpatialAnchors").GetComponent<AzureSpatialTest>().currentUser;
            newGameObject.GetComponent<BusinessCardScript>().UpdateInfos(
                currentUser.nickName,
                currentUser.mission,
                currentUser.email,
                currentUser.phoneNumber,
                currentUser.socialMedia);
        }
        //GET INFO BASED ON USERID
        newGameObject.AddComponent<CloudNativeAnchor>();
        //newGameObject.AddComponent<AnchorInterraction>();
        return newGameObject;
    }
    
    public GameObject GetModelFromModelID(string modelID)
    {
        //TO DO : get model from api
        Debug.Log("MODEL ID : " + modelID);
        Debug.Log(modelID);
        switch (modelID)
        {
            case "visit-card":
                return businessCardPrefab;
            case "community":
                int communityId = gameObject.GetComponent<AzureSpatialTest>().currentUser.communityId;
                string community = FileAndNetworkUtils.getObjectFromApi<Community>("/api/CommunitiesAPI/" + communityId)
                    .name;
                Debug.Log("Community : " + community);
                if (community.Equals("none")) return null;
                return Resources.Load<GameObject>("Prefabs/Models/Communities/" + community);
            default:
                return Resources.Load<GameObject>("Prefabs/Models/Others/" + modelID);
        }
    }
}
