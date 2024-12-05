using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class Datasets
{
    public static Dictionary<string, float[][]> titaticTrain => __titanicTrain();
    private static Dictionary<string, float[][]> __titanicTrain()
    {
        Dictionary<string, string[]> set = CSV.toDict(CSV.Read(@"Assets/Scripts/Datasets/titainc/train.csv"));
        Dictionary<string, float[][]> result = new();
        //string[] dataKeys = { "Pclass", "Age", "SibSp", "Parch", "Fare"};
        string[] dataKeys = { "Age"};
        string[] specificDataKeys = { "Sex" };
        string[] answerKeys = { "Survived" };
        result.Add("x", extractData(set, dataKeys, specificDataKeys, (key, data) => { return data == "male" ? 1 : 0; }));
        result.Add("y", extractAnswers(set, answerKeys));
        return deleteNaNs(result);
    }
    private static Dictionary<string, float[][]> deleteNaNs(Dictionary<string, float[][]> dict)
    {
        Func<float[], bool> containsNaN = (arr) =>
        {
            return arr.Where(i => float.IsNaN(i)).Count() > 0;
        };
        List<float[]> x = new List<float[]>();
        List<int> NaNs = new List<int>();
        List<float[]> y = new List<float[]>();
        Dictionary<string, float[][]> result = new();
        for (int i = 0; i < dict["x"].Length; i++)
            if (!containsNaN(dict["x"][i]) && !containsNaN(dict["y"][i]))
            {
                x.Add(dict["x"][i]);
                y.Add(dict["y"][i]);
            }
        result.Add("x", x.ToArray());
        result.Add("y", y.ToArray());
        return result;
    }
    private static float[][] extractData(Dictionary<string, string[]> set, string[] dataKeys, string[] specificDataKeys, Func<string, string, float> specificDataExtract)
    {
        List<float[]> result = new();
        for (int i = 0; i < set[dataKeys[0]].Length; i++)
        {
            result.Add(new float[dataKeys.Length + specificDataKeys.Length]);
        }
        Action<int> readColumn = (keyIndex) =>
        {
            int index = 0;
            foreach (var i in set[dataKeys[keyIndex]])
            {
                try
                {
                    result[index][keyIndex] = float.Parse(i);
                }
                catch (Exception)
                {
                    result[index][keyIndex] = float.NaN;
                }
                index++;
            }
        };
        Action<int> readSpecificColumn = (keyIndex) =>
        {
            int index = 0;
            foreach (var i in set[specificDataKeys[keyIndex]])
            {
                try
                {
                    result[index++][keyIndex + dataKeys.Length] = specificDataExtract(specificDataKeys[keyIndex], i);
                }
                catch (Exception)
                {
                    result[index][keyIndex + dataKeys.Length] = float.NaN;
                }
            }
        };
        for (int i = 0; i < dataKeys.Length; i++)
        {
            readColumn(i);
        }
        for (int i = 0; i < specificDataKeys.Length; i++)
        {
            readSpecificColumn(i);
        }
        return result.ToArray();
    }
    private static float[][] extractAnswers(Dictionary<string, string[]> set, string[] answerKeys)
    {
        List<float[]> result = new();
        for (int i = 0; i < set[answerKeys[0]].Length; i++)
        {
            result.Add(new float[answerKeys.Length * 2]);
        }
        Action<int> readColumn = (keyIndex) =>
        {
            int index = 0;
            foreach (var i in set[answerKeys[keyIndex]])
            {
                try
                {
                    float answer = float.Parse(i);
                    result[index][keyIndex] = answer;
                }
                catch (Exception)
                {
                    result[index][keyIndex] = float.NaN;
                }
                index++;
            }
        };
        Action<int> readColumn2 = (keyIndex) =>
        {
            int index = 0;
            foreach (var i in set[answerKeys[keyIndex]])
            {
                try
                {
                    float answer = float.Parse(i);
                    result[index][keyIndex + answerKeys.Length] = Mathf.Abs(answer - 1);
                    index++;
                }
                catch (Exception)
                {
                    result[index][keyIndex + answerKeys.Length] = float.NaN;
                }
            }
        };
        for (int i = 0; i < answerKeys.Length; i++)
        {
            readColumn(i);
            readColumn2(i);
        }
        return result.ToArray();
    }

}
