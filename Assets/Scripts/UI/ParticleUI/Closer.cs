using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Closer : MonoBehaviour
{
    GameObject window;
    public void Awake()
    {
        window = transform.parent.parent.gameObject;
        if(!GetComponent<Button>())
            gameObject.AddComponent<Button>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Close);
    }
    private void Close()
    {
        Destroy(window);
    }
}
