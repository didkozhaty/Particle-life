using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragField : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    GameObject window;
    Vector2 winOffset;
    Vector2 offset;
    
    public void Awake()
    {
        window = transform.parent.gameObject;
        winOffset = transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        window.transform.position = eventData.position + offset;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        offset = window.transform.position.ConvertTo<Vector2>() - eventData.position;
    }
}
