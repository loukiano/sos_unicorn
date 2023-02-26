using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

[RequireComponent(typeof(ParticleSystem))]
public class BloodSuck : MonoBehaviour
{
    public float suckMag;
    public float suckAccel;
    public float invincDur;

    private ParticleSystem ps;
    private GameObject player;
    private ParticleSystem.Particle[] particles;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("blood suck start called!");

        InitializeIfNeeded();

        player = GameObject.FindGameObjectWithTag("Player");

        var main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        

        ps.Play();
    }
    
    // Update is called once per frame
    void LateUpdate()
    {

        if (ps.time > invincDur)
        {
            AddTrigger();
        }

        if (ps.isPlaying)
        {
            int numLivingParticles = ps.GetParticles(particles);

            for (int i = 0; i < numLivingParticles; i++)
            {
                Vector3 dirToPlayer = player.transform.position - particles[i].position;
                dirToPlayer.Normalize();

                Vector3 goalVel = Vector3.Project(particles[i].velocity, dirToPlayer) + (dirToPlayer * suckMag);
                suckAccel = ps.time / ps.main.duration;

                particles[i].velocity = Vector3.Lerp(particles[i].velocity, goalVel, suckAccel);
            }

            ps.SetParticles(particles, numLivingParticles);
        }
    }

    void AddTrigger()
    {
        var trigger = ps.trigger;
        trigger.AddCollider(player.GetComponent<BoxCollider2D>());
    }

    void OnParticleTrigger()
    {

        // particles
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

        // get
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        // iterate
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            p.startColor = new Color32(0, 0, 0, 0); // invisible on contact with trigger
            enter[i] = p;
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
