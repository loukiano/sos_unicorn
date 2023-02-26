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

        //setting proper color
        Player player = GetComponentInParent<Player>();
        var col = ps.colorOverLifetime;
        col.enabled = true;

        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(player.dashColor, 0.0f), new GradientColorKey(player.dashColor, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

        col.color = grad;


        //setting proper sprite
        var tex = ps.textureSheetAnimation;
        tex.enabled = true;
        tex.mode = ParticleSystemAnimationMode.Sprites;
        var plSpr = GetComponentInParent<SpriteRenderer>();
        tex.SetSprite(0, plSpr.sprite);


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
