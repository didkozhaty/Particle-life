using System.Collections.Generic;
using UnityEngine;

public class ModelParticle
{
    public List<ModelParticle> gravityTo;
    public Vector velocity;
    public Vector pos;
    public float weight;
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
            givenVelocity += dir * gravityTo[i].weight;

        }
        velocity += givenVelocity;
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
        double distance = dir.lenght;
        dir = dir.norm / (float)distance;
        return dir;
    }
    public ModelParticle(int dims)
    {
        velocity = new Vector(dims);
        pos = new Vector(dims);
    }
    public static ModelParticle random(int dims)
    {
        ModelParticle result = new ModelParticle(dims);
        for (int i = 0; i < result.pos.dims; i++)
        {
            result.pos[i] = Random.Range(-10f, 10f);
        }
        result.weight = Random.value;
        return result;
    }
}
