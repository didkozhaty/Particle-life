using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using System;

public class Particle : MonoBehaviour
{
    public static List<GameObject> particles = new List<GameObject>();
    private static float fieldOff
    {
        get { return Config.fieldSize / 2; }
    }
    public Vector3 velocity = Vector3.zero;
    public float weight;
    private void Awake()
    {
        particles.Add(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (Config.isLaunched)
        {
            Vector3 givenVelocity = Vector3.zero;
            for (int i = 0; i < particles.Count; i++)
            {
                if (particles[i] == gameObject)
                    continue;
                Vector3 offset = particles[i].transform.position - transform.position;
                Vector3 dir = CalculateGravity(offset);
                if (offset.x > 0 && offset.y > 0) offset += new Vector3(-Config.fieldSize, -Config.fieldSize, 0);
                else if (offset.x < 0 && offset.y > 0) offset += new Vector3(Config.fieldSize, -Config.fieldSize, 0);
                else if (offset.x > 0 && offset.y < 0) offset += new Vector3(-Config.fieldSize, Config.fieldSize, 0);
                else if (offset.x < 0 && offset.y < 0) offset += new Vector3(Config.fieldSize, Config.fieldSize, 0);
                dir += CalculateGravity(offset);
                givenVelocity += dir * particles[i].GetComponent<Particle>().weight;

            }
            velocity += givenVelocity * Config.scaleVelocity;
            transform.position += velocity * Time.deltaTime * Config.scaleVelocity;
            Vector3 fieldPosition = transform.position + new Vector3(fieldOff, fieldOff, 0);
            if(isOutOfField())
            {
                if (transform.position.x > fieldOff) transform.position -= new Vector3(Config.fieldSize, 0, 0) * (float)Math.Floor(Math.Abs(Convert.ToDouble(fieldPosition.x / Config.fieldSize)));
                if (transform.position.y > fieldOff)
                {
                    transform.position -= new Vector3(0, Config.fieldSize, 0) * (float)Math.Floor(Math.Abs(Convert.ToDouble(fieldPosition.y / Config.fieldSize)));
                }
                if (transform.position.x < -fieldOff) transform.position += new Vector3(Config.fieldSize, 0, 0) * (float)Math.Ceiling(Math.Abs(Convert.ToDouble(fieldPosition.x / Config.fieldSize)));
                if (transform.position.y < -fieldOff) transform.position += new Vector3(0, Config.fieldSize, 0) * (float)Math.Ceiling(Math.Abs(Convert.ToDouble(fieldPosition.y / Config.fieldSize)));
            }
        }
    }
    public void UnRelease()
    {
        gameObject.AddComponent<CreatedParticle>();
        gameObject.GetComponent<CreatedParticle>().weight = weight;
        gameObject.GetComponent<CreatedParticle>().velocity = velocity;
        particles.Remove(this.gameObject);
        Destroy(this);
    }
    public static void UnReleaseAll()
    {
        while (particles.Count > 0)
        {
            particles[0].GetComponent<Particle>().UnRelease();
        }
    }
    private Vector3 CalculateGravity(Vector3 dir)
    {
        double distance = Math.Sqrt(Math.Pow(dir.x, 2) + Math.Pow(dir.y, 2));
        dir = dir.normalized / (float)distance;
        return dir;
    }
    private bool isOutOfField()
    {
        return transform.position.x > fieldOff || transform.position.y > fieldOff || transform.position.x < -fieldOff || transform.position.y < -fieldOff;
    }
}
