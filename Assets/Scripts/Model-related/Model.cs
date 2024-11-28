using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Windows;

public enum ModelType
{
    velocity, // answerdepends on final velocity of the input particle
    distance, // answer depends on distance to answer particles from input particle
    coordsOut, // x > y ? [1,0] : [0,1]
    coordChange, // answer depends on change coords at input coords (x1 - x2 > y1 - y2 ? [0,1] : [1,0])
    outputCoords // x & y for input, z & w for output

}

public class Model
{
    List<ModelParticle> all;
    List<ModelParticle> answer;
    List<ModelParticle> bias;
    List<Vector> allPos;
    ModelParticle input;
    ModelType modelType;
    int inputs;
    int outputs;
    public Model(int inputs, int outputs, int biases, ModelType type)
    {
        all = new();
        allPos = new();
        if (type == ModelType.distance)
        {
            answer = new(outputs);
            for (int i = 0; i < outputs; i++)
                answer.Add(ModelParticle.random(Mathf.Max(inputs, outputs)));
            bias = new List<ModelParticle>(biases);
            for (int i = 0; i < biases; i++)
                bias.Add(ModelParticle.random(Mathf.Max(inputs, outputs)));
            input = new ModelParticle(inputs);
            all.AddRange(answer);
            all.AddRange(bias);
        } else if ( type == ModelType.velocity || type == ModelType.coordsOut || type == ModelType.coordChange)
        {
            answer = new(outputs);
            for (int i = 0; i < outputs; i++)
                answer.Add(ModelParticle.random(Mathf.Max(inputs, outputs)));
            bias = new List<ModelParticle>(biases);
            for (int i = 0; i < biases; i++)
                bias.Add(ModelParticle.random(Mathf.Max(inputs, outputs)));
            input = new ModelParticle(inputs);
            all.AddRange(answer);
            all.AddRange(bias);
        }
        else
        {
            answer = new(outputs);
            for (int i = 0; i < outputs; i++)
                answer.Add(ModelParticle.random(inputs + outputs));
            bias = new List<ModelParticle>(biases);
            for (int i = 0; i < biases; i++)
                bias.Add(ModelParticle.random(inputs + outputs));
            input = new ModelParticle(inputs);
            all.AddRange(answer);
            all.AddRange(bias);
        }
        foreach (var particle in all)
        {
            allPos.Add(particle.pos.copy);
        }
        input.gravityTo = all;
        this.inputs = inputs;
        this.outputs = outputs;
        modelType = type;
    }
    public float[] predict(float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            try
            {
                input.pos.coords[i] = inputs[i];
            }
            catch (Exception)
            {

                throw;
            }
        }
        for (int i = 0; i < 100; i++)
        {
            input.Step();
        }
        float[] answers = getAnswer(inputs);
        for (int i = 0; i < all.Count; i++)
        {
            all[i].pos = allPos[i].copy;
            all[i].velocity.zero();
        }
        return answers;
    }
    public float[][] predict(float[][] inputs)
    {
        List<float[]> predictions = new List<float[]>();
        foreach(var i in inputs)
        {
            predictions.Add(predict(i));
        }
        return predictions.ToArray();
    }
    public int GetAnswer(float[] inputs)
    {
        float answer = float.NegativeInfinity;
        int index = 0;
        float[] answers = predict(inputs);
        for (int i = 0; i < answers.Length; i++)
        {
            if(answers[i] > answer)
            {
                answer = answers[i];
                index = i;
            }
        }
        return index;
    }
    public int[] GetAnswer(float[][] inputs)
    {
        List<int> answers = new();
        foreach(var i in inputs)
        {
            answers.Add(GetAnswer(i));
        }
        return answers.ToArray();
    }
    public void train(float[] inputs, float[] rightAnswers)
    {
        List<Vector> velocities = new List<Vector>(all.Count);
        List<Vector> coordChange = new List<Vector>(all.Count);
        Vector avgPos = new Vector(input.pos.dims);
        for (int i = 0; i < this.inputs; i++)
        {
            input.pos.coords[i] = inputs[i];
        }
        List<Vector> iter = input.Step(true);
        coordChange = iter;
        velocities = iter;
        for (int i = 1; i < 100; i++)
        {
            avgPos += input.pos;
            iter = input.Step(true);
            for (int j = 0; j < iter.Count; j++)
            {
                try
                {
                    coordChange[j] += velocities[j];
                }
                catch (Exception)
                {
                    Debug.Log($"{j},{coordChange.Count},{velocities.Count},{iter.Count}");
                    throw;
                }
                velocities[j] += iter[j];
            }
        }
        avgPos /= 100;
        float[] answers = getAnswer(inputs);
        for(int i = 0; i < all.Count; i++)
        {
            for (int j = 0; j < answers.Length; j++)
            {
                correctingAnswers(answers, rightAnswers[j], j, inputs, velocities[i], coordChange[i],avgPos, i);
            }
        }
        for (int i = 0; i < all.Count; i++)
        {
            all[i].pos = allPos[i].copy;
            all[i].velocity.zero();
        }
    }
    public void train(float[][] inputs, float[][] rightAnswers)
    {
        var xy = inputs.Zip(rightAnswers, (X, Y) => new { x = X, y = Y });
        foreach (var i in xy)
        {
            train(i.x,i.y);
        }
    }
    public string ModelGrade(float[][] x, float[][] y)
    {
        List<int> rAnswers = new List<int>();
        foreach (var i in y)
        {
            rAnswers.Add(maxIndex(i));
        }
        List<int> gAnswers = new List<int>();
        gAnswers.AddRange(GetAnswer(x));
        float accuracy;
        List<int[]> answers = new List<int[]>();
        for (int i = 0;i < y[0].Length;i++)
        {
            answers.Add(new int[y[0].Length]);
        }
        float right = 0;
        for (int i = 0; i < rAnswers.Count; i++)
        {
            try
            {
                answers[gAnswers[i]][rAnswers[i]]++;
            }
            catch (Exception)
            {
                Debug.Log($"{i},{gAnswers[i]},{rAnswers[i]}\n{answers.Count},{answers[0].Length},{rAnswers.Count},{gAnswers.Count}");
            }
        }
        List<float[]> precision = new List<float[]>();
        for (int i = 0; i < y[0].Length; i++)
        {
            precision.Add(new float[2]);
        }
        for (int i = 0; i < answers.Count; i++)
        {
            for (int j = 0; j < answers[i].Length; j++)
            {
                if(i == j)
                    precision[i][0] = answers[i][j];
                else
                {
                    precision[i][1] += answers[i][j];
                }
            }
        }
        precision.ForEach(i => right += i[0]);
        accuracy = right / rAnswers.Count;
        string retme = $"accuracy: {accuracy}\n";
        for (int i = 0; i < precision.Count; i++)
        {
            try
            {
                retme += $"precision: {precision[i][0] / (precision[i].Sum())},";
            }
            catch (DivideByZeroException)
            {
                retme += $"precision: 0\n";
            }
            try
            {
                retme += $"recall: {precision[i][0] / rAnswers.Where(a => a == i).Count()}\n";
            }
            catch (DivideByZeroException)
            {
                retme += $"recall: 1\n";
            }
             
        }
        return retme;
    }

    private int maxIndex(float[] arr)
    {
        float max = arr[0];
        int index = 0;
        int current = 0;
        foreach (var item in arr)
        {
            if(item > max)
            {
                max = item;
                index = current;
            }
            current++;
        }
        return index;
    }

    private void correctingAnswers(float[] givenAnswers, float rightAnswer, int answerIndex, float[] inputs, Vector velocity, Vector s, Vector avgPos, int particleIndex)
    {
        if (float.IsNaN(givenAnswers[answerIndex]))
            throw new System.Exception("NaN in answer");
        ModelParticle correcting = all[particleIndex];
        float velocityToAnswer;
        if (s.containsNaN)
            throw new System.Exception("NaN in s");
        if (modelType == ModelType.velocity)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer;
            correcting.weight += mistake * velocity[answerIndex];
        }
        else if (modelType == ModelType.distance)
        {
            Vector velPos = new Vector(correcting.pos.dims);
            for(int i = 0; i < inputs.Length; i++)
            {
                velPos[i] = inputs[i];
            }
            velPos += s;
            Vector mistake = velPos - all[answerIndex].pos;
            float howMuch = 0;
            for (int i = 0; i < mistake.dims; i++)
                howMuch += mistake[i];
            howMuch = howMuch > 0 ? 1 : -1;
            correcting.weight += mistake.lenght * howMuch;
        }
        else if (modelType == ModelType.coordsOut)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer;
            correcting.weight += mistake * velocity[answerIndex];
        }
        else if (modelType == ModelType.coordChange)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer;
            correcting.weight += mistake * velocity[answerIndex];
        }
        else if (modelType == ModelType.outputCoords)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer;
            correcting.weight += mistake * velocity[answerIndex + this.inputs];
        }


        if (modelType == ModelType.velocity)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer;
            allPos[particleIndex] += velocity.norm / mistake;
        }
        else if (modelType == ModelType.distance)
        {
            Vector velPos = new Vector(correcting.pos.dims);
            for (int i = 0; i < inputs.Length; i++)
            {
                velPos[i] = inputs[i];
            }
            velPos += s;
            Vector mistake = velPos - all[answerIndex].pos;
            float howMuch = 0;
            for (int i = 0; i < mistake.dims; i++)
                howMuch += mistake[i];
            howMuch = howMuch > 0 ? 1 : -1;
            allPos[particleIndex] += mistake * howMuch;
        }
        else if (modelType == ModelType.coordsOut)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer;
            allPos[particleIndex] += s * mistake;
        }
        else if (modelType == ModelType.coordChange)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer;
            allPos[particleIndex] += s * mistake;
        }
        else if (modelType == ModelType.outputCoords)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer;
            allPos[particleIndex] += s * mistake;
        }
    }
    private float[] getAnswer(float[] inputData)
    {
        List<float> answer = new List<float>();
        float max = float.NegativeInfinity;
        if (modelType == ModelType.velocity) { 
            for (int i = 0; i < outputs; i++)
                answer.Add(input.velocity[i]);
        } else if (modelType == ModelType.distance) {
            for (int i = 0; i < outputs; i++)
                answer.Add((this.answer[i].pos - input.pos).lenght);
        } else if (modelType == ModelType.coordsOut)
        {
            for (int i = 0; i < outputs; i++)
                answer.Add(input.pos[i]);
        } else if (modelType == ModelType.coordChange)
        {
            for (int i = 0; i < outputs; i++)
            {
                if (inputData.Where(a => float.IsNaN(a)).Count() > 0)
                    throw new Exception("NaN in input");
                if(input.pos.containsNaN)
                    throw new Exception("NaN in pos");
                answer.Add(inputData[i] - input.pos[i]);
            }    
        } else if (modelType == ModelType.outputCoords)
        {
            for (int i = 0; i < outputs; i++)
                answer.Add(input.pos[inputs + i]);
        }
        answer.ForEach(i => { if (i > max) max = i; });
        for (int i = 0; i < answer.Count; i++)
        {
            answer[i] /= max;
        }
        return answer.ToArray();
    }
    public void Visualize()
    {
        foreach (var item in all)
        {
            item.Visualize();
        }
        foreach (var item in answer)
        {
            item.visualizer.gameObject.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
    }
}
