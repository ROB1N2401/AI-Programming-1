using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRangeFromProjectileNode : Node
{
    Agent _currentAgent;
    Agent _targetAgent;
    Type _targetType;
    float _rangeThreshold;

    public CheckRangeFromProjectileNode(Agent currentAgent_in, Type targetType_in)
    {
        _currentAgent = currentAgent_in;
        _targetAgent = null;
        _targetType = targetType_in;
        _rangeThreshold = 2.0f;
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

        var projectiles = Object.FindObjectsOfType<HomingProjectile>();
        if (projectiles.Length == 0)
            return State.FAILURE;
        else for (int i = 0; i < projectiles.Length; i++)
        {
            var distance = projectiles[i].transform.position - _targetAgent.GameObjectHolder.transform.position;
            if (distance.sqrMagnitude <= (_rangeThreshold * _rangeThreshold))
            {
                return State.SUCCESS;
            }
        }
        return State.FAILURE;
    }
}
