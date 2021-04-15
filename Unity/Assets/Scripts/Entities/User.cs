using System;

[Serializable]
public class User
{
    public string id;
    public string nickName;
    public string email;
    public string phoneNumber;
    public string socialMedia;
    public string enterprise;
    public string mission;
    public string community;
    public int communityId;
    public string password;
    
    public User(string nickame, string email, string phoneNumber, string socialMedia, string function, string mission, string password)
    {
        this.nickName = nickame;
        this.email = email;
        this.phoneNumber = phoneNumber;
        this.socialMedia = socialMedia;
        this.enterprise = function;
        this.mission = mission;
        this.password = password;
    }

    public User()
    {
        
    }
}
