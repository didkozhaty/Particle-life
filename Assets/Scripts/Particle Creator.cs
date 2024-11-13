using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCreator : MonoBehaviour
{
    private float random => Random.Range(-Config.fieldSize / 2, Config.fieldSize / 2);
    public void OnMouseDown()
    {
        GameObject particle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        particle.transform.position = new Vector3(random,random,0);
        particle.transform.localScale = Config.particleScale;
        particle.AddComponent<CreatedParticle>();
        ParticleUI.Create(particle);
    }
    public void OnMouseDrag()
    {
    }
}
