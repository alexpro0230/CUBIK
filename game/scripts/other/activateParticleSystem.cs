using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateParticleSystem : MonoBehaviour
{
    public ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateParticles()
    {
        ps.Play();
    }
}
