using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKiller : MonoBehaviour {

    private ParticleSystem particleSystem;

    void Start()
    {
        // Fetches particle System.
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (particleSystem)
        {
            if (!particleSystem.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
