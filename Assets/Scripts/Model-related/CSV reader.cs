using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class CSV
{
    public static string[][] Read(string path)
    {
        List<string[]> data = new List<string[]>();
        bool isFirst = true;
        using (var reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                MatchCollection strings = Regex.Matches(line, @"(?:(?:""([^""]*)"")|([^"",]*))(?:,|$)");
                var result = strings.Cast<Match>().Select(m => m.Groups[1].Success ? m.Groups[1].Value : m.Groups[2].Value).ToList();
                for (int i = 0; i < result.Count; i++)
                {
                    if (Regex.IsMatch(result[i], @"[0-9]+\.[0-9]+"))
                    {
                        result[i] = Regex.Replace(result[i], @"\.", ",");
                    }
                }
                data.Add(result.ToArray());
                if (isFirst)
                {
                    Debug.Log(line);
                    isFirst = false;
                }
            }
        }
        return data.ToArray();
    }
    public static Dictionary<string, string[]> toDict(string[][] data) 
    {
        Dictionary<string, string[]> result = new();
        Func<int, string[]> readColumn = (i) =>
        {
            string[] result = new string[data.Length - 1];
            for (int j = 0; j < data[j + 1].Length; j++)
            {
                try
                {
                    result[j] = data[j + 1][i];
                }
                catch (Exception)
                {
                    Debug.Log($"i: {i}, j:{j}, data: {data[0].Length}, {data.Length}");
                    throw;
                }

            }
            return result;
        };
        /*for (int i = 0; i < data[0].Length; i++)
        {
            result.Add(data[0][i], new string[data.Length - 1]);
        }*/
        for (int i = 0; i < data[0].Length; i++)
        {
            result[data[0][i]] = readColumn(i);
            /*for (int j = 0; j < data.Length - 1; j++)
            {
                try
                {
                    result[data[0][i]][j] = data[j + 1][i];
                }
                catch (Exception)
                {
                    Debug.Log($"i: {i}, j:{j}, data: {data[0].Length}, {data.Length}");
                    throw;
                }
            }*/
        }
        string keys = "";
        foreach (string key in result.Keys)
            keys += $"{key}: {result[key][0]}\n";
        Debug.Log(keys);
        return result;
    }
    
}
