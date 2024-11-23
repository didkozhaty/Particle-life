using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleCreator : MonoBehaviour
{
    private float random => Random.Range(-Config.fieldSize / 2, Config.fieldSize / 2);
    private void Start()
    {
        if(!GetComponent<Button>())
            gameObject.AddComponent<Button>();
        GetComponent<Button>().onClick.AddListener(CreateParticle);
    }
    public void CreateParticle()
    {
        GameObject particle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        particle.transform.position = new Vector3(random,random,0);
        particle.transform.localScale = Config.particleScale;
        particle.AddComponent<CreatedParticle>();
    }
    public void OnMouseDrag()
    {
    }
}
