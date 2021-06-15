using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shunt))]
public class InterceptorAI : BaseAI
{
    protected override void GetAbilities()
    {
        this.ability = GetComponent<Shunt>();
    }

    protected override void ConstructBehaviourTree()
    {
        CheckTargetPositionNode leadershipCheck = new CheckTargetPositionNode(GetAgent());
        CheckAvailabilityNode abilityAvailability = new CheckAvailabilityNode(GetAbility());
        CheckRangeFromDispenserNode checkRangeFromDispenser = new CheckRangeFromDispenserNode(GetAgent());
        UseAbilityNode useAbility = new UseAbilityNode(GetAgent());
        Inverter invertedLeadershipCheck = new Inverter(leadershipCheck);
        FindShuntTargetNode findLeadingTarget = new FindShuntTargetNode(GetAgent(), 1);
        FindShuntTargetNode findSecondaryTarget = new FindShuntTargetNode(GetAgent(), 2);

        Sequence disposeOfAbility = new Sequence(new List<Node> { abilityAvailability, checkRangeFromDispenser, findSecondaryTarget, useAbility });
        Sequence helpLeader = new Sequence(new List<Node> { invertedLeadershipCheck, abilityAvailability, findLeadingTarget, useAbility });

        topNode = new Selector(new List<Node> { helpLeader, disposeOfAbility });
    }
}
