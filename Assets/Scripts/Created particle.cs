using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedParticle : MonoBehaviour
{
    public static List<CreatedParticle> particles = new List<CreatedParticle>();
    public float weight;
    public Vector3 velocity = Vector3.zero;
    private void Awake()
    {
        weight = Random.Range(-1000,1000) / 1000f;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_Metallic", 0.8f);
        renderer.material.color = Color.red * ((weight + 1)/2f);
        particles.Add(this);
    }
    private void OnMouseDrag()
    {
        var worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        gameObject.transform.position = worldPoint;
    }
    public void Release()
    {
        gameObject.AddComponent<Particle>();
        gameObject.GetComponent<Particle>().weight = weight;
        gameObject.GetComponent<Particle>().velocity = velocity;
        particles.Remove(this);
        Destroy(this);
    }
    public static void ReleaseAll()
    {
        while (particles.Count > 0)
        {
            particles[0].Release();
        }
    }
}
