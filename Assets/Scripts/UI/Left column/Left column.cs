using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.Windows;
using UnityEngine.UI;

public class Leftcolumn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject inputF = GameObject.Find("Field size input");
        GameObject inputS = GameObject.Find("Sphere size input");
        GameObject inputV = GameObject.Find("Scale velocity input");
        GameObject launcher = GameObject.Find("Start/Stop");
        inputF.GetComponent<InputField>().submit = (text) =>
        {
            Config.ChangeFieldSize(float.Parse(text));
        };
        inputS.GetComponent<InputField>().submit = (text) =>
        {
            Config.ChangeParticleSize(float.Parse(text));
        };
        inputV.GetComponent<InputField>().submit = (text) =>
        {
            Config.scaleVelocity = float.Parse(text);
        };
        inputF.GetComponent<InputField>().updateValue = (text) => { text.text = Config.fieldSize.ToString(); };
        inputS.GetComponent<InputField>().updateValue = (text) => { text.text = Config.particleSize.ToString(); };
        inputV.GetComponent<InputField>().updateValue = (text) => { text.text = Config.scaleVelocity.ToString(); };
        inputF.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.DecimalNumber;
        inputS.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.DecimalNumber;
        inputV.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.DecimalNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
