using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nitro : Ability
{
    ParticleSystem _particleSystem = null;
    NavMeshAgent _navMeshAgent = null;
    Agent _agent = null;
    bool _isActive = false;
    float _timeElapsed = 0.0f;
    const float _duration = 1.5f;
    const float _angularMultiplier = 1.5f;
    const float _accelerationMultiplier = 2f;
    const float _speedMultiplier = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        AiComponent = GetComponent<BaseAI>();
        _agent = AiComponent.GetAgent();
        TargetAgent = null;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _particleSystem = GetComponent<ParticleSystem>();
        var main = _particleSystem.main;
        main.duration = _duration;
        main.startColor = new Color(136.0f / 255.0f, 255.0f / 255.0f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(_isActive)
        {
            _timeElapsed += Time.deltaTime;
            if(_timeElapsed >= _duration)
            {
                _timeElapsed -= _duration;
                DeactivateNitro();
            }
        }
    }

    public override void ActivateAbility()
    {
        IsAvailable = false;
        _isActive = true;
        var main = _particleSystem.main;
        main.startColor = new Color(136.0f / 255.0f, 255.0f / 255.0f, 0);
        _particleSystem.Play();
        _navMeshAgent.speed = _agent.Speed * _speedMultiplier;
        _navMeshAgent.angularSpeed = _agent.AngularSpeed * _angularMultiplier;
        _navMeshAgent.acceleration = _agent.Acceleration * _accelerationMultiplier;

    }

    void DeactivateNitro()
    {
        _isActive = false;
        _navMeshAgent.speed = _agent.Speed;
        _navMeshAgent.angularSpeed = _agent.AngularSpeed;
        _navMeshAgent.acceleration = _agent.Acceleration;
    }

}
