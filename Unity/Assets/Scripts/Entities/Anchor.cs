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
}

public class AnchorDTO
{
     public string identifier;
     public string modelID;
     public int userID;
}

