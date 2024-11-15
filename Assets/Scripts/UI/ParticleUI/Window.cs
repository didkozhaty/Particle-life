using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleWindow : MonoBehaviour
{
    static GameObject inputW => instance.transform.GetChild(2).gameObject;
    static GameObject inputVX => instance.transform.GetChild(5).gameObject;
    static GameObject inputVY => instance.transform.GetChild(7).gameObject;
    static GameObject inputPX => instance.transform.GetChild(10).gameObject;
    static GameObject inputPY => instance.transform.GetChild(12).gameObject;
    public static GameObject instance;
    public static CreatedParticle particle;
    public static void Create(CreatedParticle particle)
    {
        if(instance)
            Destroy(instance);
        GameObject canvas = GameObject.Find("Canvas");
        instance = Prefabs.window;
        instance.transform.parent = canvas.transform;
        instance.transform.position = Input.mousePosition;
        inputW.AddComponent<InputField>();
        inputVX.AddComponent<InputField>();
        inputVY.AddComponent<InputField>();
        inputPX.AddComponent<InputField>();
        inputPY.AddComponent<InputField>();
        inputW.GetComponent<InputField>().submit = (text) =>
        {
            particle.weight = float.Parse(text);
        };
        inputVX.GetComponent<InputField>().submit = (text) =>
        {
            try
            {
                particle.velocity.x = float.Parse(text);
            }
            catch { }
        };
        inputVY.GetComponent<InputField>().submit = (text) =>
        {
            try
            {
                particle.velocity.y = float.Parse(text);
            }
            catch { }
        };
        inputPX.GetComponent<InputField>().submit = (text) =>
        {
            try
            {
                Vector3 pos = particle.gameObject.transform.position;
                pos.x = float.Parse(text);
                particle.gameObject.transform.position = pos;
            }
            catch { }
        };
        inputPY.GetComponent<InputField>().submit = (text) =>
        {
            try
            {
                Vector3 pos = particle.gameObject.transform.position;
                pos.y = float.Parse(text);
                particle.gameObject.transform.position = pos;
            }
            catch { }
        };
        inputW.GetComponent<InputField>().updateValue = (text) => { text.text = particle.weight.ToString(); };
        inputVX.GetComponent<InputField>().updateValue = (text) => { text.text = particle.velocity.x.ToString(); };
        inputVY.GetComponent<InputField>().updateValue = (text) => { text.text = particle.velocity.y.ToString(); };
        inputPX.GetComponent<InputField>().updateValue = (text) => { text.text = particle.gameObject.transform.position.x.ToString(); };
        inputPY.GetComponent<InputField>().updateValue = (text) => { text.text = particle.gameObject.transform.position.y.ToString(); };
        inputW.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.DecimalNumber;
        inputVX.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.DecimalNumber;
        inputVY.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.DecimalNumber;
        inputPX.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.DecimalNumber;
        inputPY.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.DecimalNumber;
        ParticleWindow.particle = particle;
    }
}
