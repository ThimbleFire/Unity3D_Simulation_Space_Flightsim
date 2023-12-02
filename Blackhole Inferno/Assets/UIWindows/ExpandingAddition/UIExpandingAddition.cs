using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Reflection;

/// <summary>
/// ExpandingAddition is a modular window designed to store other widow elements
/// </summary>
public class UIExpandingAddition : MonoBehaviour
{
    public enum Modules
    {
        LoadingBar
    };

    public Image symbol;
    public Text title;
    public Animation animation;

    public LoadingBar loadingBar;

    public delegate void ExpansionCompleteHandler();
    public event ExpansionCompleteHandler OnExpansionComplete;

    /// <summary> Initiate the animations that build the window</summary>
    public LoadingBar Build(Sprite _symbol, string _title, Color color)
    {
        //symbol.sprite = _symbol;
        title.text = _title;
        title.color = color;
        
        animation.Play("ExpandHorizontalBounds");

        return loadingBar;
    }
    
    public void OnExpandHorizontalBounds_Complete()
    {
        loadingBar.gameObject.SetActive(true);

        // expand the loading bar
    }
}