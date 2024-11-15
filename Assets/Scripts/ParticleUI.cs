using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Xml;

public class Prefabs
{
    public static GameObject text => GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Text"));
    public static GameObject window => GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ParticleWindow"));
}
/*public class ParticleUI : MonoBehaviour
{
    // Start is called before the first frame update
    public static void Create(GameObject gameObject)
    {
        GameObject closeButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject dragField = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject window = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject weightAlign = Prefabs.text;
        GameObject weightField = Prefabs.text;
        GameObject velocityAlign = Prefabs.text;
        GameObject x = Prefabs.text;
        GameObject velocityX = Prefabs.text;
        GameObject y = Prefabs.text;
        GameObject velocityY = Prefabs.text;
        window.transform.localScale = new Vector3(5,3,1);
        window.transform.position = new Vector3(0, 0, 0);
        dragField.transform.localScale = new Vector3(5,1,1);
        dragField.transform.position = new Vector3(0, 1, -1);
        closeButton.transform.localScale = Vector3.one;
        closeButton.transform.position = new Vector3(2, 1, -2);
        weightAlign.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2);
        weightAlign.transform.position = new Vector3(-1.5f, 0, -0.5f);
        weightAlign.GetComponent<TextMeshPro>().fontSize = 5.25f;
        weightAlign.GetComponent<TextMeshPro>().text = "Weight";
        weightField.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3);
        weightField.transform.position = new Vector3(1,0,-0.6f);
        weightField.GetComponent<TextMeshPro>().fontSize = 8.95f;
        weightField.GetComponent<TextMeshPro>().overflowMode = TextOverflowModes.Ellipsis;
        velocityAlign.transform.position = new Vector3(-2, -1, -0.6f);
        velocityAlign.GetComponent<TextMeshPro>().fontSize = 8.95f;
        velocityAlign.GetComponent<TextMeshPro>().text = "V:";
        x.transform.position = new Vector3(-1, -1, -0.6f);
        x.GetComponent<TextMeshPro>().fontSize = 8.95f;
        x.GetComponent<TextMeshPro>().text = "X";
        velocityX.transform.position = new Vector3(0, -1, -0.6f);
        velocityX.GetComponent<TextMeshPro>().enableAutoSizing = true;
        velocityX.GetComponent<TextMeshPro>().fontSizeMin = 4f;
        velocityX.GetComponent<TextMeshPro>().overflowMode = TextOverflowModes.Ellipsis;
        y.transform.localScale = Vector3.one;
        y.transform.position = new Vector3(1, -1, -0.6f);
        y.GetComponent<TextMeshPro>().fontSize = 8.95f;
        y.GetComponent<TextMeshPro>().text = "X";
        velocityY.transform.position = new Vector3(2, -1, -0.6f);
        velocityY.GetComponent<TextMeshPro>().enableAutoSizing = true;
        velocityY.GetComponent<TextMeshPro>().fontSizeMin = 4f;
        velocityY.GetComponent<TextMeshPro>().overflowMode = TextOverflowModes.Ellipsis;

        weightField.AddComponent<TextField>();
        weightField.GetComponent<TextField>().submit = (str) => { gameObject.GetComponent<CreatedParticle>().weight = float.Parse(str); };
        weightField.GetComponent<TextField>().updateText = (text) => { text.text = gameObject.GetComponent<CreatedParticle>().weight.ToString(); };
        velocityX.AddComponent<TextField>();
        velocityX.GetComponent<TextField>().submit = (str) => { gameObject.GetComponent<CreatedParticle>().velocity.x = float.Parse(str); };
        velocityX.GetComponent<TextField>().updateText = (text) => { text.text = gameObject.GetComponent<CreatedParticle>().velocity.x.ToString(); };
        velocityY.AddComponent<TextField>();
        velocityY.GetComponent<TextField>().submit = (str) => { gameObject.GetComponent<CreatedParticle>().velocity.y = float.Parse(str); };
        velocityY.GetComponent<TextField>().updateText = (text) => { text.text = gameObject.GetComponent<CreatedParticle>().velocity.y.ToString(); };

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class CloseButton : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    public void Add(GameObject obj)
        { objects.Add(obj); }
    private void OnMouseUpAsButton()
    {
        while (objects.Count > 0)
        {
            GameObject obj = objects[0];
            objects.RemoveAt(0);
            Destroy(obj);
        }
        Destroy(gameObject);
    }
}*/

/*public class DragField : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    private List<Vector3> offsets = new List<Vector3>();
    public void Add(GameObject obj)
    { 
        objects.Add(obj);
        offsets.Add(Vector3.zero);
    }
    private void Awake()
    {
        Add(gameObject);
    }
    private void OnMouseDown()
    {
        Vector3 CursorPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        for(int i = 0; i < offsets.Count; i++) 
        {
            offsets[i] = CursorPos - objects[i].transform.position;
        }
    }
    private void OnMouseDrag()
    {
        Vector3 CursorPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.position = CursorPos + offsets[i];
        }
    }
}*/

public class TextField : MonoBehaviour
{
    public Action<string> submit = (data) => { };
    public Action<TMP_Text> updateText = (text) => { };
    public TMP_InputField inputField;
    public TMP_Text text;
    private bool isEditing => inputField.isFocused;
    void Start()
    {
        // Add or use existing TMP_InputField component
        inputField = gameObject.AddComponent<TMP_InputField>();
        text = GetComponent<TMP_Text>();
        inputField.textComponent = text;
        inputField.placeholder = text; // Optionally show placeholder as same object
        inputField.onValueChanged.AddListener(OnTextChange);
    }

    void OnTextChange(string newText)
    {
        Debug.Log("Current input: " + newText);
    }
    private void Update()
    {
        updateText(text);
    }
}
