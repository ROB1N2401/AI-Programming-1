using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shunt : Ability
{
    ParticleSystem _particleSystem = null;
    NavMeshAgent _navMeshAgent = null;
    Agent _agent = null;
    const float _speed = 16.0f;
    const float _rotationSpeed = 200.0f;

    // Start is called before the first frame update
    void Start()
    {
        AiComponent = GetComponent<BaseAI>();
        _agent = AiComponent.GetAgent();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _particleSystem = GetComponent<ParticleSystem>();

        var main = _particleSystem.main;    
        main.startColor = new Color(255.0f / 255.0f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActivateAbility()
    {
        IsAvailable = false;
        var main = _particleSystem.main;
        main.startColor = new Color(255.0f / 255.0f, 0, 0);
        _particleSystem.Play();
        GameObject projectile = Instantiate(Resources.Load("Projectile", typeof(GameObject))) as GameObject;
        projectile.transform.position = this.transform.position + (_navMeshAgent.velocity.normalized * 1f);
        projectile.transform.rotation = this.transform.rotation;
        projectile.AddComponent<HomingProjectile>();
        var missile = projectile.GetComponent<HomingProjectile>();
        missile.Initialize(TargetAgent.GameObjectHolder.transform, _speed, _rotationSpeed);
    }
}
