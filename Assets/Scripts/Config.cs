using System.Collections;
using System.Collections.Generic;
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
        isLaunched = _isLaunched;
        particleSize = _particleSize;
        fieldSize = _fieldSize;
        scaleVelocity = _scaleVelocity;
        if (isLaunched )
            CreatedParticle.ReleaseAll();
        else 
            Particle.UnReleaseAll();
        foreach(GameObject particle in Particle.particles)
        {
            particle.transform.localScale = new Vector3(particleSize, particleSize, particleSize );
        }
        foreach (CreatedParticle particle in CreatedParticle.particles)
        {
            particle.gameObject.transform.localScale = new Vector3(particleSize, particleSize, particleSize);
        }
        field.transform.localScale = new Vector3(fieldSize, fieldSize, 0.1f);
    }
    private void Start()
    {
    }
}
