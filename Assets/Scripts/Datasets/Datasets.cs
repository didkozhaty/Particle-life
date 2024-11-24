using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Datasets
{
    public static Dictionary<string, float[][]> titatic => __titanic();
    private static Dictionary<string, float[][]> __titanic()
    {
        Dictionary<string, string[]> set = CSV.toDict(CSV.Read(@"E:\Unity\Particle life\Assets\Scripts\Datasets\titanic.csv"));
        Dictionary<string, float[][]> result = new();
        string[] dataKeys = { "Pclass", "Age", "SibSp", "Parch", "Fare"};
        string[] specificDataKeys = { "Sex" };
        string[] answerKeys = { "Survived" };
        result.Add("x", extractData(set, dataKeys, specificDataKeys, (key, data) => { return data == "male" ? 1 : 0; })["x"]);
        result.Add("y", extractAnswers(set, answerKeys)["y"]);
        return result;
    }
    private static Dictionary<string, float[][]> extractData(Dictionary<string, string[]> set, string[] dataKeys, string[] specificDataKeys, Func<string, string, float> specificDataExtract)
    {
        Dictionary<string, float[][]> result = new();
        result.Add("x", new float[dataKeys.Length + specificDataKeys.Length][]);
        for (int i = 0; i < result["x"].Length; i++)
        {
            result["x"][i] = new float[set[dataKeys[0]].Length];
        }
        for (int i = 0; i < dataKeys.Length; i++)
        {
            for (int j = 0; j < set[dataKeys[i]].Length; j++)
            {
                try
                {
                    result["x"][i][j] = float.Parse(set[dataKeys[i]][j]);
                }
                catch (Exception)
                {
                    result["x"][i][j] = 0;
                    Debug.Log($"Err: |{set[dataKeys[i]][j]}|");
                }
            }
        }
        for (int i = dataKeys.Length; i < dataKeys.Length + specificDataKeys.Length; i++)
        {
            for (int j = 0; j < set[specificDataKeys[i-dataKeys.Length]].Length; j++)
            {
                result["x"][i][j] = specificDataExtract(set[specificDataKeys[i - dataKeys.Length]][j], specificDataKeys[i - dataKeys.Length]);
            }
        }
        return result;
    }
    private static Dictionary<string, float[][]> extractAnswers(Dictionary<string, string[]> set, string[] answerKeys)
    {
        Dictionary<string, float[][]> result = new();
        result.Add("y", new float[answerKeys.Length * 2][]);
        for (int i = 0; i < result["y"].Length; i++)
        {
            result["y"][i] = new float[set[answerKeys[0]].Length];
        }
        for (int i = 0; i < answerKeys.Length; i++)
        {
            for (int j = 0; j < set[answerKeys[i]].Length; j++)
            {
                try
                {
                    float answer = float.Parse(set[answerKeys[i]][j]);
                    result["y"][i][j] = answer;
                }
                catch (Exception)
                {
                    Debug.Log($"Error: |{set[answerKeys[i]][j]}|");
                    set[answerKeys[i]][j] = "0";
                    result["y"][i][j] = 0;
                }
            }
        }
        for (int i = 0; i < answerKeys.Length; i++)
        {
            for (int j = 0; j < set[answerKeys[i]].Length; j++)
            {
                float answer = float.Parse(set[answerKeys[i]][j]);
                result["y"][i + answerKeys.Length][j] = Mathf.Abs(answer - 1);
            }
        }
        return result;
    }

}
