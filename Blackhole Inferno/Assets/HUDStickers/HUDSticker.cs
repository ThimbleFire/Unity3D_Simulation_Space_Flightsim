using System;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDSticker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public static HUDSticker highlightedHUDSticker = null;
    private float lastClickTime = 0f;
    
    public float signatureRadius = 65.0f;

    /// <summary>
    ///  X = left & right
    ///  Y = up & down
    ///  Z = forward & backward
    /// </summary>
    public Vector3 worldPosition;
    public List<ContextMenuOption.Commands> CMOCommands;

    public RectTransform rectTransform;

    public float scale = 0.005f;

    public Sprite Sprite {get{return GetComponent<UnityEngine.UI.Image>().sprite; } }

    public void OnPointerClick(PointerEventData eventData) {
        if(eventData.button == PointerEventData.InputButton.Left) {
            float currentTime = Time.time;
            float doubleClickTimeThreshold = 0.3f;
            // double-click
            if (currentTime - lastClickTime < doubleClickTimeThreshold) {
                //CameraMove.instance.ResetDistance(transform.forward, signatureRadius);        
            }
            // single-click
            else lastClickTime = currentTime;
        }
        if( eventData.button == PointerEventData.InputButton.Right ) {
            ContextMenu.instance.OpenContextMenu(this);
        }
    }
    public void OnPointerEnter(PointerEventData eventData) {
        Tooltip.instance.Show(this.name);
        highlightedHUDSticker = this;
    }
    public void OnPointerExit(PointerEventData eventData) {
        Tooltip.instance.Hide();
       if(highlightedHUDSticker == this) {
            highlightedHUDSticker = null;
        }
    } 

    public void Update()
    {
        UpdateSizeInRelationToCameraDistance();        
    }

    protected virtual void LateUpdate() { 
        UpdateFaceTheCamera();        
        UpdateHUDStickerPositionsOnScreen();   
        //WorldSpaceToScreenSpace();      
    }
    protected void UpdateFaceTheCamera()
    {
        // Make the canvas face the camera
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
    protected void UpdateSizeInRelationToCameraDistance()
    {
        // Resize the UI element so that regardless of zoom, it shows at the correct size.
        var size = (Camera.main.transform.position - transform.position).magnitude;  
        transform.localScale = new Vector3(size,size,size) * scale;
    }
    
    protected void UpdateHUDStickerPositionsOnScreen()
    {   
        
    }
    void WorldSpaceToScreenSpace() {
        
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        rectTransform.position = screenPosition;
    }
}