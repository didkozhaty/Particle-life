using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Resizer : MonoBehaviour
{
    public Vector2 location;
    public Vector2 size;
    public Vector2 fieldSize;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        GameObject canvas = GameObject.Find("Canvas");
        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
        Vector2 point = Camera.main.WorldToViewportPoint(new Vector3(-Config.fieldSize/2, 0, 0));
        point.y = 1;
        Vector2 size;
        Vector2 canvSize = transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta;
        Vector2 fieldSize = canvSize * point;
        size = fieldSize * this.size / this.fieldSize;
        rectTransform.sizeDelta = size;
        Vector2 border = (size - canvSize)/2;
        Vector2 temp = border + size * location / this.size;
        temp *= new Vector2(1, -1);
        rectTransform.localPosition = temp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
