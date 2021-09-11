using System;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.Maps.Unity;

/// <summary>
/// Controls the text that displays the horizontal map's zoom level on its control menu
/// </summary>
class Zoom : MonoBehaviour
{
    #region Fields
    private MapRenderer _mapRenderer;
    private Text _labelText;
    #endregion

    #region Methods
    private void Start()
    {
        _mapRenderer = GetComponentInParent<MapRenderer>();

        // Pull the text from the child Text item 
        _labelText = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        // Second GetComponentInParent in case Starts loaded in a funny order
        if (_mapRenderer == null)
        {
            _mapRenderer = GetComponentInParent<MapRenderer>();
        }
        // Pull the text from the child Text item 
        _labelText = GetComponentInChildren<Text>();
        string updateText = Math.Round((double)_mapRenderer.ZoomLevel, 2).ToString();
        _labelText.text = updateText;
    }
    #endregion
}
