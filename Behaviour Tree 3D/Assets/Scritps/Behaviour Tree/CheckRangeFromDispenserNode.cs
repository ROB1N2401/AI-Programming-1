using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CheckRangeFromDispenserNode : Node
{
    Agent _currentAgent;
    float _rangeThreshold;

    public CheckRangeFromDispenserNode(Agent currentAgent_in)
    {
        _currentAgent = currentAgent_in;
        _rangeThreshold = 2.0f;
    }

    public override State Evaluate()
    {
        var dispensers = Object.FindObjectsOfType<Dispenser>();
        for(int i = 0; i < dispensers.Length; i++)
        {
            var distance = dispensers[i].transform.position - _currentAgent.GameObjectHolder.transform.position;
            if(distance.sqrMagnitude <= (_rangeThreshold * _rangeThreshold))
            {
                return State.SUCCESS;
            }
        }
        return State.FAILURE;
    }
}
