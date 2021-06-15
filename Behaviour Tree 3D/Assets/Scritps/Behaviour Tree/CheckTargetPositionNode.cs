using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetPositionNode : Node
{
    Agent _currentAgent;
    Agent _targetAgent;

    public CheckTargetPositionNode(Agent currentAgent_in)
    {
        _currentAgent = currentAgent_in;
        _targetAgent = null;
    }

    public override State Evaluate()
    {
        switch(_currentAgent.AgentType)
        {
            case Type.LEADER:
                _targetAgent = _currentAgent;
                break;
            case Type.INTERCEPTOR:
                var leaders = GameObject.FindObjectsOfType<LeaderAI>();
                for(int i = 0; i < leaders.Length; i++)
                {
                    if(leaders[i].GetAgent().AgentTeam == _currentAgent.AgentTeam)
                    {
                        _targetAgent = leaders[i].GetAgent();
                        break;
                    }
                }
                if (_targetAgent == null)
                    Debug.LogError("CheckTargetPositionNode.cs: failed to acquire team's Leader");
                break;
            default:
                Debug.LogError("CheckTargetPositionNode.cs: failed to acquire target agent");
                break;
        }

        if (_targetAgent.Position == 1)
        {
            return State.SUCCESS;
        }
        else return State.FAILURE;
    }
}
