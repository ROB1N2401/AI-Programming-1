using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbilityNode : Node
{
    Agent _currentAgent;
    BaseAI _baseAIComponent;
    Ability _ability;

    public UseAbilityNode(Agent currentAgent_in)
    {
        _currentAgent = currentAgent_in;
        _baseAIComponent = _currentAgent.GameObjectHolder.GetComponent<BaseAI>();
        _ability = _baseAIComponent.GetAbility();
    }

    public override State Evaluate()
    {
        _ability.ActivateAbility();
        return State.RUNNING;
    }
}
