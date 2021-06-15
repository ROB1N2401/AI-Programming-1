using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindShieldTargetNode : Node
{
    Agent _currentAgent;
    Agent _targetAgent;
    BaseAI _baseAIComponent;
    Ability _ability;
    Type _targetType;

    public FindShieldTargetNode(Agent currentAgent_in, Type targetType_in)
    {
        _currentAgent = currentAgent_in;
        _targetAgent = null;
        _baseAIComponent = _currentAgent.GameObjectHolder.GetComponent<BaseAI>();
        _ability = _baseAIComponent.GetAbility();
        _targetType = targetType_in;
    }

    public override State Evaluate()
    {
        switch (_targetType)
        {
            case Type.LEADER:
                var leaders = Object.FindObjectsOfType<LeaderAI>();
                for (int i = 0; i < leaders.Length; i++)
                {
                    if (leaders[i].GetAgent().AgentTeam == _currentAgent.AgentTeam)
                    {
                        _targetAgent = leaders[i].GetAgent();
                        break;
                    }
                }
                if (_targetAgent == null)
                    Debug.LogError("CheckRangeFromProjectileNode.cs: failed to acquire team's Leader");
                break;
            case Type.INTERCEPTOR:
                var interceptors = Object.FindObjectsOfType<InterceptorAI>();
                for (int i = 0; i < interceptors.Length; i++)
                {
                    if (interceptors[i].GetAgent().AgentTeam == _currentAgent.AgentTeam)
                    {
                        _targetAgent = interceptors[i].GetAgent();
                        break;
                    }
                }
                if (_targetAgent == null)
                    Debug.LogError("CheckRangeFromProjectileNode.cs: failed to acquire team's Interceptor");
                break;
            default:
                Debug.LogError("CheckRangeFromProjectileNode.cs: failed to acquire targetAgent");
                break;
        }
        _ability.SetTarget(_targetAgent);
        return State.RUNNING;
    }
}
