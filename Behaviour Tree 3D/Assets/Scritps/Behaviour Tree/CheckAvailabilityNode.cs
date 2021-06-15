using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAvailabilityNode : Node
{
    Ability _ability = null;

    public CheckAvailabilityNode(Ability ability_in)
    {
        _ability = ability_in;
    }

    public override State Evaluate()
    {
        if (_ability.IsAvailable)
        {
            return State.SUCCESS;
        }
        else return State.FAILURE;
    }
}
