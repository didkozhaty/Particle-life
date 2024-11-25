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
        result.Add("x", extractData(set, dataKeys, specificDataKeys, (key, data) => { return data == "male" ? 1 : 0; }));
        result.Add("y", extractAnswers(set, answerKeys));
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
                    result[index][keyIndex] = 0;
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
                    result[index][keyIndex + dataKeys.Length] = 0;
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
                    set[answerKeys[keyIndex]][index] = "0";
                    result[index][keyIndex] = 0;
                }
                index++;
            }
        };
        Action<int> readColumn2 = (keyIndex) =>
        {
            int index = 0;
            foreach (var i in set[answerKeys[keyIndex]])
            {
                float answer = float.Parse(i);
                result[index][keyIndex + answerKeys.Length] = Mathf.Abs(answer - 1);
                index++;
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
