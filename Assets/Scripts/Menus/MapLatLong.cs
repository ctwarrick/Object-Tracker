using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.Maps.Unity;

/// <summary>
/// Base class which translates the lat/long of the center of the horizontal map into text for display
/// </summary>
public abstract class MapLatLong : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    protected MapRendererBase mapRendererBase;
    #endregion

    #region Fields
    protected Text text;
    protected LocationHandler locationHandler;
    #endregion

    #region Methods
    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Pull the text object from the child of the containing GameObject
        text = GetComponentInChildren<Text>();

        // Get the location handler from the main camera
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        locationHandler = mainCamera.GetComponent<LocationHandler>();
    }

    // Update is called once per frame
    protected abstract void Update();
    #endregion
}
