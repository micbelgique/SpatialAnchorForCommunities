using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessCardScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro Name, Job, Email, Phone, Social;
    [SerializeField]
    private GameObject planeGameObject;
    public string AnchorId { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        SetJob("Intern at MIC");
        SetEmail("test@mic.be");
        SetPhone("0470123456");
        SetSocial("@TEST");
        
        //Texture2D  texture = Resources.Load("Textures/test") as Texture2D;
        //this.SetProfilePicture(texture);
    }

    public void UpdateInfos()
    {
        

        //GET INFO BASED ON USERID IN ANCHOR
        SetName("coucou");
        //SetEmail(AnchorId);
    }

    public void SetName(string name)
    {
        Name.text = name;
    }
    
    public void SetJob(string job)
    {
        Job.text = job;
    }
    
    public void SetEmail(string email)
    {
        Email.text = email;
    }
    
    public void SetPhone(string phone)
    {
        Phone.text = phone;
    }
    
    public void SetSocial(string social)
    {
        Social.text = social;
    }

    public void SetProfilePicture(Texture2D texture)
    {
        Material material = new Material(Shader.Find("Diffuse"));
        material.mainTexture = texture;
        planeGameObject.GetComponent<Renderer>().material = material;
    }
}
