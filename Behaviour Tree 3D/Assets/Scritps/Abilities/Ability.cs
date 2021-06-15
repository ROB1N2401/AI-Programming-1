using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{
    Agent _targetAgent;
    BaseAI _aiComponent;
    bool _isAvailable = false;

    public BaseAI AiComponent { get => _aiComponent; set => _aiComponent = value; }
    public bool IsAvailable { get => _isAvailable; set => _isAvailable = value; }
    public Agent TargetAgent { get => _targetAgent; set => _targetAgent = value; }

    public void SetTarget(Agent target_in)
    {
        _targetAgent = target_in;
    }

    public abstract void ActivateAbility();
}
