using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizerParticle : MonoBehaviour
{
    public ModelParticle particle;
    public void Visualize()
    {
        Vector3 pos = particle.pos.vec3;
        pos.z = 0;
        transform.position = pos;
    }
}
