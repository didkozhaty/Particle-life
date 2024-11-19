using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatedParticle : MonoBehaviour, IPointerClickHandler
{
    public static List<CreatedParticle> particles = new List<CreatedParticle>();
    public float weight;
    public Vector3 velocity = Vector3.zero;
    private void Awake()
    {
        particles.Add(this);
        if (GetComponent<Particle>())
            return;
        weight = Random.Range(-1000, 1000) / 1000f;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_Metallic", 0.8f);
        renderer.material.color = Color.red * ((weight + 1) / 2f);
    }
    private void OnMouseDrag()
    {
        var worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Config.fieldSize));
        gameObject.transform.position = worldPoint;
    }
    public void Release()
    {
        gameObject.AddComponent<Particle>();
        gameObject.GetComponent<Particle>().weight = weight;
        gameObject.GetComponent<Particle>().velocity = velocity;
        Destroy(this);
    }
    public static void ReleaseAll()
    {
        foreach (var particle in particles)
            particle.Release();
        particles.Clear();
        if(ParticleWindow.instance)
            Destroy(ParticleWindow.instance);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ParticleWindow.Create(this);
        }
    }
}
