using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class HUDSticker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public static HUDSticker highlightedHUDSticker = null;
    private float lastClickTime = 0f;
    private Image image;
    private RectTransform rectTransform;

    public Vector3 worldPosition;
    public float signatureRadius;
    public bool globalVisibility = false;

    protected Vector3 direction { get { return (worldPosition - Ship.LPC.worldPosition).normalized; } }
    protected float distance { get { return Vector3.Distance(worldPosition, Ship.LPC.worldPosition); } }
    protected Vector3 target3Position { get { return direction * (Camera.main.farClipPlane - 10.0f); } }

    public List<ContextMenuOption.Commands> CMOCommands;

    protected virtual void Awake() {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

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
    public virtual void Arrived()
    {
        StopCoroutine(DisposeCoroutine());
        Debug.Log("Arrived at " + gameObject.name);
    }
    public virtual void Leaving()
    {
        StartCoroutine(DisposeCoroutine());
        Debug.Log("Leaving " + gameObject.name);
    }
    private IEnumerator DisposeCoroutine()
    {
        yield return new WaitForSeconds(30.0f);

        Timeout();
    }
    protected virtual void Timeout()
    {

    }

    protected void WorldSpaceToScreenSpace() {

        if (distance < Camera.main.farClipPlane || globalVisibility)
        {
            rectTransform.position = Camera.main.WorldToScreenPoint(target3Position);
            Vector3 viewportPoint = Camera.main.WorldToViewportPoint(target3Position);
            image.enabled = viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0;
        }
        else image.enabled = false;
    }
}