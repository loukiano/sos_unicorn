using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AfterImage : MonoBehaviour
{
    public float fadeDur;
    

    private ParticleSystem ps;
    private GameObject parent;
    private ParticleSystem.Particle[] particles;

    private Dashable dash;


    // Start is called before the first frame update
    void Start()
    {

        InitializeIfNeeded();

        parent = transform.parent.gameObject;

        var main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        dash = GetComponentInParent<Dashable>();
    }

    void Update()
    {
        // only play emitter when we're dashing
        if (!dash.isDashing)
        {
            if (ps.isPlaying)
            {
                ps.Stop();
            }
        } else if (!ps.isPlaying)
        {
            ps.Play();
        }
    }

    void InitializeIfNeeded()
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();

        if (particles == null || particles.Length < ps.main.maxParticles)
            particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    

}
