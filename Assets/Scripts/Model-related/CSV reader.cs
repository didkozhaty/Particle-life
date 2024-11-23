using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

class CSV
{
    static string[][] Read(string path)
    {
        List<string[]> data = new List<string[]>();
        // Read and process CSV file
        using (var reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                data.Add(line.Split(',')); // Split by comma (adjust if using a different delimiter)
            }
        }
        return data.ToArray();
    }
}
