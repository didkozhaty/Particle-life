using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ModelRunner : MonoBehaviour
{
    Dictionary<string, float[][]> dataset;
    Model model;
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
        dataset = Datasets.titaticTrain;
        //Debug.Log(elapseTime());
        model = new(dataset["x"][0].Length, dataset["y"][0].Length, 0, ModelType.coordChange);
        Debug.Log($"{dataset["x"][0].Length}, {dataset["y"][0].Length}");
        //Debug.Log(elapseTime());
        Debug.Log(model.ModelGrade(dataset["x"], dataset["y"]));
        model.Visualize();
    }
    public void train()
    {
        model.train(dataset["x"], dataset["y"]);
        Debug.Log(model.ModelGrade(dataset["x"], dataset["y"]));
        model.Visualize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
