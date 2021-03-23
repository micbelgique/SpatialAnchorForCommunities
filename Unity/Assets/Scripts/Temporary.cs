using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temporary : MonoBehaviour
{
    private bool _camAvailable;
    private WebCamTexture _backCam;
    private Texture _defaultBackGround;

    public RawImage _background;
    public AspectRatioFitter fit;

    private void Start()
    {
        _defaultBackGround = _background.texture;

        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            _camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                _backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (_backCam == null)
        {
            Debug.Log("Unable to find camera");
            return;
        }
        
        _backCam.Play();
        _background.texture = _backCam;

        _camAvailable = true;
    }

    private void Update()
    {
        if (!_camAvailable)
            return;

        float ratio = (float) _backCam.width / (float) _backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = _backCam.videoVerticallyMirrored ? -1f : 1f;
        _background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -_backCam.videoRotationAngle;
        _background.rectTransform.localEulerAngles= new Vector3(0, 0, orient);

    }
}
