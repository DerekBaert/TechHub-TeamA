using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public ParticleSystem particle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particle.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
