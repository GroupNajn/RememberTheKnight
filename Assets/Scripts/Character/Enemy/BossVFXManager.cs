using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;


[RequireComponent(typeof(BehaviorGraphAgent))]
public class BossVFXManager : MonoBehaviour
{
    private static WaitForSeconds waitForGroundCleanup = new WaitForSeconds(5);
    [SerializeField] Transform signalSlot;
    [SerializeField] ParticleSystem signalEffect;
    [SerializeField] ParticleSystem executeSignalEffect;
    [SerializeField] ParticleSystem groundEffect;
    [SerializeField] ParticleSystem executeGroundEffect;

    [SerializeField] public Transform airOriginLeft;
    [SerializeField] public Transform airOriginRight;
    [SerializeField] ParticleSystem airEffect;
    [SerializeField] ParticleSystem executeAirEffect;


    ParticleSystem currentSignalEffect;
    ParticleSystem currentGroundEffect;
    ParticleSystem currentAirEffect;


    ParticleSystem cachedSignalEffect;

    BehaviorGraphAgent behaviourAgent;
    BlackboardVariable<Phase> bossPhase;
    BlackboardVariable<GameObject> target;
    Queue<ParticleSystem> extraGroundParticles;
    void Start()
    {
        behaviourAgent = GetComponent<BehaviorGraphAgent>();
        if (behaviourAgent.BlackboardReference.GetVariable("Phase", out bossPhase))
        {
            bossPhase.OnValueChanged += SwitchParticleSystems;
        }
        if (behaviourAgent.BlackboardReference.GetVariable("Target", out target)) { }

        currentSignalEffect = signalEffect;
        currentGroundEffect = groundEffect;
        currentAirEffect = airEffect;
        extraGroundParticles = new();
    }

    void OnDestroy()
    {
        bossPhase.OnValueChanged -= SwitchParticleSystems;
    }

    public void PlaySignalEffect()
    {
        if (!cachedSignalEffect) cachedSignalEffect = Instantiate(currentSignalEffect, signalSlot);
        cachedSignalEffect.Play();
    }

    public void PlayGroundEffect()
    {
        ParticleSystem extraGroundEffect = Instantiate(currentGroundEffect, target.Value.transform.position, Quaternion.identity);
        extraGroundEffect.Play();
        Destroy(extraGroundEffect.gameObject, 5f);
    }

    public void PlayAirEffectLeft()
    {
        ParticleSystem extraAirEffect = Instantiate(currentAirEffect, airOriginLeft.position, Quaternion.identity);
        extraAirEffect.Play();
        Destroy(extraAirEffect.gameObject, 5f);
        RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.bossBlueBubbleAttackEvent, extraAirEffect.gameObject);

    }

    public void PlayAirEffectRight()
    {
        ParticleSystem extraAirEffect = Instantiate(currentAirEffect, airOriginRight.position, Quaternion.identity);
        extraAirEffect.Play();
        Destroy(extraAirEffect.gameObject, 5f);
        RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.bossBlueBubbleAttackEvent, extraAirEffect.gameObject);

    }


    void SwitchParticleSystems()
    {
        Destroy(cachedSignalEffect);
        switch (bossPhase.Value)
        {
            case Phase.Initial:
            case Phase.Intermission:
                currentSignalEffect = signalEffect;
                currentGroundEffect = groundEffect;
                currentAirEffect = airEffect;
                break;
            case Phase.Execute:
                currentSignalEffect = executeSignalEffect;
                currentGroundEffect = executeGroundEffect;
                currentAirEffect = executeAirEffect;
                break;
        }
        RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.bossPhaseSwitchevent, gameObject);
    }
}
