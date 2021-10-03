using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using TMPro;

public class AddressTooltip : MonoBehaviour
{
    // Address is serialized because GetComponentInChildren throws janky
    // NullReferenceExceptions
    #region Fields
    private ToolTip _tooltip;
    private float _spawnTime;
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        _tooltip = GetComponent<ToolTip>();
        _spawnTime = 0.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        _spawnTime += Time.deltaTime;
        if (_spawnTime >3.0f)
        {
            Destroy(gameObject);
        }
    }
    public void SetAddress(string text)
    {
        _tooltip.ToolTipText = text;
    }
}
