using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Ability
{
    ParticleSystem _particleSystem = null;
    Agent _agent = null;
    bool _isActive = false;
    float _timeElapsed = 0.0f;
    const float _duration = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        AiComponent = GetComponent<BaseAI>();
        _agent = AiComponent.GetAgent();
        _particleSystem = GetComponent<ParticleSystem>();
        var main = _particleSystem.main;
        main.startColor = new Color(0, 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive)
        {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed >= _duration && TargetAgent.GameObjectHolder.transform.childCount > 0)
            {
                _isActive = false;
                _timeElapsed -= _duration;
                var shield = TargetAgent.GameObjectHolder.transform.GetChild(0).gameObject;
                Destroy(shield);
            }
        }
    }

    void FixedUpdate()
    {
        if (_isActive && TargetAgent.GameObjectHolder.transform.childCount > 0)
        {
            var shield = TargetAgent.GameObjectHolder.transform.GetChild(0);
            shield.position = TargetAgent.GameObjectHolder.transform.position;
        }

    }

    public override void ActivateAbility()
    {
        if(!_isActive)
        {
            var main = _particleSystem.main;
            main.startColor = new Color(0, 1.0f, 1.0f);
            _particleSystem.Play();
            IsAvailable = false;
            _isActive = true;
            GameObject shield = Instantiate(Resources.Load("Shield", typeof(GameObject)), TargetAgent.GameObjectHolder.transform) as GameObject;
            shield.transform.SetParent(TargetAgent.GameObjectHolder.transform);
        }
    }
}
