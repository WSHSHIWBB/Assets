using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    private GameObject selfParticle;

    private void Awake()
    {
        selfParticle = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!selfParticle.activeSelf && other.name== "MatchStic"||other.name== "MatchStic(Clone)")
        {
            var particle = other.GetComponentInChildren<ParticleSystem>();
            if (particle&& selfParticle&& particle.isPlaying)
            {
                selfParticle.SetActive(true);
            }
        }
        else if(selfParticle.activeSelf && other.name== "JiuJingDenGai")
        {
            selfParticle.SetActive(false);
        }
    }

    

}
