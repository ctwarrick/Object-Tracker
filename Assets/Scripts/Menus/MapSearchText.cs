using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapSearchText : MonoBehaviour
{
    #region Constants
    // This value controls how quickly to fade from 1 to 0 alpha (opaque to transparent) per frame.
    const float FadeFactor = 0.02f;
    // Number of seconds to wait before fading.
    const float FadeDelay = 0.5f;
    #endregion

    #region Fields
    TextMeshPro textMesh;
    float _fadeTime;
    float _alpha;
    // This is true if field is an input response, false if used for search text or blank
    bool _isResponse;
    // This is true if fading has begun
    bool _isFading;
    #endregion

    #region Properties
    public bool IsResponse
    {
        get { return _isResponse; }
        set { _isResponse = value; }
    }
    #endregion

    #region Methods
    void Start()
    {
        // Instantiate the prefab and make it blank
        textMesh = GetComponent<TextMeshPro>();

        // Set _isResponse false because field is blank
        _isResponse = false;

        // Grab the alpha value of the text for fading
        _alpha = textMesh.material.color.a;

        // Zero out the fade timer
        _fadeTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isResponse == true)
        {
            // Add the frame delta to the timer
            _fadeTime += Time.deltaTime;

            // If object has existed for 0.5sec, start fading
            if (_fadeTime >= FadeDelay)
            {
                _isFading = true;
            }

            // If fading, decrease transparency
            if (_isFading == true)
            {
                _alpha -= FadeFactor;
            }

            // If totally faded, make blank and then reset.
            if (_alpha <= 0.0f)
            {
                textMesh.text = "";
                _alpha = 1.0f;
                _fadeTime = 0.0f;
                _isResponse = false;
            }
        }
    }

    public void SetText(string text)
    {
        textMesh.text = text;
    }
    #endregion
}
