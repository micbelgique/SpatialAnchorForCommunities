using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Anchor
{
     public string identifier;
     public User user;
     public string model;
     public string userId;
}

[Serializable]
public class AnchorDTO
{
     public int id;
     public string identifier;
     public string model;
     public string userId;
     public User user;
     public List<TagInterraction> interactions;
     public string creationDate;
     public string lastUpdateDate;
     public float longitude;
     public float latitude;
     public int srid;
     public string pictureUrl;
}

