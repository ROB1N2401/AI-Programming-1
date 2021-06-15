using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Nitro))]
public class LeaderAI : BaseAI
{
    protected override void GetAbilities()
    {
        this.ability = GetComponent<Nitro>();
    }

    protected override void ConstructBehaviourTree()
    {
        CheckTargetPositionNode leadershipCheck = new CheckTargetPositionNode(GetAgent());
        CheckAvailabilityNode abilityAvailability = new CheckAvailabilityNode(GetAbility());
        CheckRangeFromDispenserNode checkRangeFromDispenser = new CheckRangeFromDispenserNode(GetAgent());
        UseAbilityNode useAbility = new UseAbilityNode(GetAgent());
        Inverter invertedLeadershipCheck = new Inverter(leadershipCheck);

        Sequence disposeOfAbility = new Sequence(new List<Node> { abilityAvailability, checkRangeFromDispenser, useAbility });
        Sequence tryToCatchUp = new Sequence(new List<Node> { invertedLeadershipCheck, abilityAvailability, useAbility });

        topNode = new Selector(new List<Node> { tryToCatchUp, disposeOfAbility });
    }
}
