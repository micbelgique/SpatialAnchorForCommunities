using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public int id;
    public string nickName;
    public string email;
    public string phoneNumber;
    public string socialMedia;
    public string function;
    public string mission;

    public User(string nickame, string email, string phoneNumber, string socialMedia, string function, string mission)
    {
        this.nickName = nickame;
        this.email = email;
        this.phoneNumber = phoneNumber;
        this.socialMedia = socialMedia;
        this.function = function;
        this.mission = mission;
    }

    public User()
    {
        
    }
}
