using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { Config.ToggleLaunched(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
