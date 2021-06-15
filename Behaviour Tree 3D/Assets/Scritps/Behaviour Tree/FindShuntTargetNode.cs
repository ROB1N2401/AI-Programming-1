using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindShuntTargetNode : Node
{
    Agent _currentAgent;
    Agent _targetAgent;
    BaseAI _baseAIComponent;
    Ability _ability;
    int _position;

    public FindShuntTargetNode(Agent currentAgent_in, int position_in)
    {
        _currentAgent = currentAgent_in;
        _targetAgent = null;
        _position = position_in;
        _baseAIComponent = _currentAgent.GameObjectHolder.GetComponent<BaseAI>();
        _ability = _baseAIComponent.GetAbility();
    }

    public override State Evaluate()
    {
        if (_currentAgent.Position <= _position)
        {
            return State.FAILURE; 
        }
        _targetAgent = GameObject.Find("Game Manager").GetComponent<Positioning>().GetAgentAtPosition(_position);
        _ability.SetTarget(_targetAgent);
        return State.RUNNING;
    }
}
