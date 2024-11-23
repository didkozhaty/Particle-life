using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
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
        if (type == ModelType.distance)
        {
            answer = new List<ModelParticle>(outputs);
            bias = new List<ModelParticle>(biases);
            input = new ModelParticle(inputs);
            all.AddRange(answer);
            all.AddRange(bias);
            for (int i = 0; i < all.Count; i++)
            {
                all[i] = ModelParticle.random(inputs);
            }
        } else if ( type == ModelType.velocity || type == ModelType.coordsOut || type == ModelType.coordChange)
        {
            answer = new List<ModelParticle>(outputs);
            bias = new List<ModelParticle>(biases);
            input = new ModelParticle(inputs);
            all.AddRange(answer);
            all.AddRange(bias);
            for (int i = 0; i < all.Count; i++)
            {
                all[i] = ModelParticle.random(Mathf.Max(inputs,outputs));
            }
        }
        else
        {
            answer = new List<ModelParticle>(outputs);
            bias = new List<ModelParticle>(biases);
            input = new ModelParticle(inputs);
            all.AddRange(answer);
            all.AddRange(bias);
            for (int i = 0; i < all.Count; i++)
            {
                all[i] = ModelParticle.random(inputs + outputs);
            }
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
        for (int i = 0; i <= inputs.Length; i++)
        {
            input.pos[i] = inputs[i];
        }
        for (int i = 0; i < 100; i++)
        {
            all.ForEach(x => { x.fHalfStep(); });
            all.ForEach(x => { x.sHalfStep(); });
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
    public void train(float[] inputs, float[] rightAnswers)
    {
        List<Vector> velocities = new List<Vector>(all.Count);
        List<Vector> coordChange = new List<Vector>(all.Count);
        Vector avgPos = new Vector(input.pos.dims);
        for (int i = 0; i <= inputs.Length; i++)
        {
            input.pos[i] = inputs[i];
        }
        for (int i = 0; i < 100; i++)
        {
            avgPos += input.pos;
            List<Vector> iter = input.Step(true);
            for (int j = 0; j < iter.Count; j++)
            {
                coordChange[j] += velocities[j];
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
    private void correctingAnswers(float[] givenAnswers, float rightAnswer, int answerIndex, float[] inputs, Vector velocity, Vector s, Vector avgPos, int particleIndex)
    {
        ModelParticle correcting = all[particleIndex];
        float velocityToAnswer;
        if (modelType == ModelType.velocity)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer - 0.5f;
            correcting.weight += mistake * velocity[answerIndex] * 0.1f;
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
            correcting.weight += mistake.lenght * howMuch * 0.1f;
        }
        else if (modelType == ModelType.coordsOut)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer - 0.5f;
            correcting.weight += mistake * velocity[answerIndex] * 0.1f;
        }
        else if (modelType == ModelType.coordChange)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer - 0.5f;
            correcting.weight += mistake * velocity[answerIndex] * 0.1f;
        }
        else if (modelType == ModelType.outputCoords)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer - 0.5f;
            correcting.weight += mistake * velocity[answerIndex + this.inputs] * 0.1f;
        }


        if (modelType == ModelType.velocity)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer - 0.5f;
            mistake *= 2;
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
            allPos[particleIndex] += mistake * howMuch * 0.1f;
        }
        else if (modelType == ModelType.coordsOut)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer - 0.5f;
            mistake *= 2;
            allPos[particleIndex] += s * mistake * 0.1f;
        }
        else if (modelType == ModelType.coordChange)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer - 0.5f;
            mistake *= 2;
            allPos[particleIndex] += s * mistake * 0.1f;
        }
        else if (modelType == ModelType.outputCoords)
        {
            float mistake = givenAnswers[answerIndex] - rightAnswer - 0.5f;
            mistake *= 2;
            allPos[particleIndex] += s * mistake * 0.1f;
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
                answer.Add(inputData[i] - input.pos[i]);
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
}
