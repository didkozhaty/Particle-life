using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ModelRunner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
        double startTime = Time.time;
        Func<double> elapseTime = () => 
        {
            double retme = Time.timeAsDouble - startTime;
            startTime = Time.timeAsDouble;
            return retme;
        };
        Dictionary<string, float[][]> dataset = Datasets.titatic;
        //Debug.Log(elapseTime());
        Model model = new(dataset["x"][0].Length, dataset["y"][0].Length, 0, ModelType.outputCoords);
        Debug.Log($"{dataset["x"][0].Length}, {dataset["y"][0].Length}");
        //Debug.Log(elapseTime());
        Debug.Log(model.ModelGrade(dataset["x"], dataset["y"]));
        elapseTime();
        //Debug.Log($"Graduation time: {elapseTime()}");
        for (int i = 0; i < 50; i++)
        {
            model.train(dataset["x"], dataset["y"]);
        }
        Debug.Log($"Train time: {elapseTime()}");
        Debug.Log(model.ModelGrade(dataset["x"], dataset["y"]));
        //Debug.Log($"Graduation time: {elapseTime()}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
