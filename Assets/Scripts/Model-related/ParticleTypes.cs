using System.Collections.Generic;
using UnityEngine;

public class ModelParticle
{
    static int count = 0;
    int num = 0;
    public List<ModelParticle> gravityTo;
    private Vector __velocity;
    public Vector velocity
    {
        get { return __velocity; }
        set
        {
            foreach (var x in value.coords)
            {
                if (float.IsNaN(x))
                    throw new System.Exception("NaN in Velocity");
            }
            __velocity = value;
        }
    }
    private Vector __pos;
    public Vector pos
    {
        get { return __pos; }
        set 
        {
            foreach (var x in value.coords)
            {
                if (float.IsNaN(x))
                    throw new System.Exception("NaN in Coords");
            }
            __pos = value; 
        }
    }
    public float weight;
    public VisualizerParticle visualizer;
    public GameObject visualize;
    // Update is called once per frame
    public void fHalfStep()
    {
        Vector givenVelocity = new Vector(velocity.dims);

        for (int i = 0; i < gravityTo.Count; i++)
        {
            if (gravityTo[i] == this)
                continue;
            Vector offset = gravityTo[i].pos - pos;
            Vector dir = CalculateGravity(offset);
            if (dir.containsNaN)
            {
                throw new System.Exception($"NaN in dir at {i}");
            }
            givenVelocity += dir * gravityTo[i].weight;
            if (givenVelocity.containsNaN)
            {
                throw new System.Exception($"NaN in givenVelocity at {i}");
            }

        }
        velocity += givenVelocity;
    }
    public void Visualize()
    {
        visualizer.Visualize();
    }
    public List<Vector> fHalfStep(bool trainMode)
    {
        Vector givenVelocity = new Vector(velocity.dims);
        List<Vector> velocities = new List<Vector>();

        for (int i = 0; i < gravityTo.Count; i++)
        {
            if (gravityTo[i] == this)
                continue;
            Vector offset = gravityTo[i].pos - pos;
            Vector dir = CalculateGravity(offset) * gravityTo[i].weight;
            velocities.Add(dir);
            givenVelocity += velocities[i];
        }
        velocity += givenVelocity;
        return velocities;
    }
    public void sHalfStep()
    {
        pos += velocity;
    }
    public void Step()
    {
        fHalfStep();
        sHalfStep();
    }
    public List<Vector> Step(bool trainMode)
    {
        List<Vector> retme = new List<Vector>();
        retme = fHalfStep(trainMode);
        sHalfStep();
        return retme;
    }
    private Vector CalculateGravity(Vector dir)
    {
        float distance = dir.lenght;
        if (distance == 0)
            return new Vector(dir.dims);
        dir = dir.norm / (float)distance;
        return dir;
    }
    public ModelParticle(int dims)
    {
        velocity = new Vector(dims);
        pos = new Vector(dims);
        visualize = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        visualizer = visualize.AddComponent<VisualizerParticle>();
        visualizer.particle = this;
        num = count++;
    }
    public static bool operator ==(ModelParticle a, ModelParticle b) { return a.num == b.num; }
    public static bool operator !=(ModelParticle a, ModelParticle b) { return a.num != b.num; }
    public static ModelParticle random(int dims)
    {
        ModelParticle result = new ModelParticle(dims);
        for (int i = 0; i < result.pos.dims; i++)
        {
            result.pos.coords[i] = Random.Range(-1000f, 1000f);
        }
        result.weight = Random.value;
        return result;
    }
}
