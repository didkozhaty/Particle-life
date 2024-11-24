using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ModelRunner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
        float startTime = Time.time;
        Func<float> elapseTime = () => 
        {
            float retme = Time.time - startTime;
            startTime = Time.time;
            return retme;
        };
        Dictionary<string, float[][]> dataset = Datasets.titatic;
        Debug.Log(elapseTime());
        Model model = new(dataset["x"].Length, dataset["y"].Length, 0, ModelType.velocity);
        Debug.Log($"{dataset["x"].Length}, {dataset["y"].Length}");
        Debug.Log(elapseTime());
        Debug.Log(model.ModelGrade(dataset["x"], dataset["y"]));
        Debug.Log($"Graduation time: {elapseTime()}");
        model.train(dataset["x"], dataset["y"]);
        Debug.Log($"Train time: {elapseTime()}");
        Debug.Log(model.ModelGrade(dataset["x"], dataset["y"]));
        Debug.Log($"Graduation time: {elapseTime()}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
