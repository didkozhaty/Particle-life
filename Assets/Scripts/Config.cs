using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Config : MonoBehaviour
{
    [SerializeField]
    public static bool isLaunched;
    public static float particleSize;
    public static float fieldSize;
    public static float scaleVelocity;
    public static Vector3 particleScale
    {
        get { return new Vector3(particleSize, particleSize, particleSize); }
    }
    public static Vector3 fieldOffset
    {
        get { return new Vector3(-fieldSize/2, -fieldSize/2, 0); }
    }
    public GameObject field;
    public bool _isLaunched;
    public float _particleSize;
    public float _fieldSize;
    public float _scaleVelocity;
    public void OnValidate()
    {
        if (isLaunched != _isLaunched)
        {
            ToggleLaunched();
        }
        if (particleSize != _particleSize)
        { 
            ChangeParticleSize(_particleSize);
        }
        if (fieldSize != _fieldSize)
        {
            ChangeFieldSize(_fieldSize);
        }
        scaleVelocity = _scaleVelocity;
    }
    public void ToggleLaunched()
    {
        isLaunched = !isLaunched;
        if (isLaunched)
            CreatedParticle.ReleaseAll();
        else
            Particle.UnReleaseAll();
    }
    public void ChangeParticleSize(float size)
    {
        particleSize = size;
        foreach (GameObject particle in Particle.particles)
        {
            particle.transform.localScale = new Vector3(particleSize, particleSize, particleSize);
        }
        foreach (CreatedParticle particle in CreatedParticle.particles)
        {
            particle.gameObject.transform.localScale = new Vector3(particleSize, particleSize, particleSize);
        }
    }
    public void ChangeFieldSize(float size)
    {
        fieldSize = size;
        Camera.main.orthographicSize = size/2;
        float fieldX = (Camera.main.transform.position - Camera.main.ViewportToWorldPoint(new Vector3(1, 0, size))).x+size/2;
        Camera.main.transform.position = new Vector3(fieldX, 0, -size);
        field.transform.localScale = new Vector3(fieldSize, fieldSize, 0.1f);
    }
}
