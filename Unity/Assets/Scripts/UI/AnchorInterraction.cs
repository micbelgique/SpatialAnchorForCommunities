using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorInterraction : MonoBehaviour
{

    public string anchorId;
    
    public void SetAnchorId(string anchorId)
    {
        this.anchorId = anchorId;
    }

    public void OnItemClicked()
    {
        Debug.Log("TOUCHED ITEM " + anchorId);
        GameObject.Find("PopupWindow").gameObject.GetComponent<AnchorClickedPopup>().ShowPopup(anchorId);
    }
}
