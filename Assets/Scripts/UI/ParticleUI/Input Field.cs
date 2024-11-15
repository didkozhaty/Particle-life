using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputField : MonoBehaviour
{
    public TMP_InputField inputField;
    public Action<TMP_Text> updateValue = (text) => { };
    public Action<string> submit = (text) => { };
    private bool isActive = false;
    private void Awake()
    {
        gameObject.AddComponent<TMP_InputField>();
        inputField = GetComponent<TMP_InputField>();
        inputField.textComponent = GetComponent<TMP_Text>();
        inputField.onSelect.AddListener(OnSelect);
        inputField.onEndEdit.AddListener(Submit);

    }
    private void Submit(string text)
    {
        submit(text);
        isActive = false;
    }
    private void OnSelect(string text)
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            updateValue(inputField.textComponent);
        }
    }
}
