using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shield))]
public class DefenderAI : BaseAI
{
    protected override void GetAbilities()
    {
        this.ability = GetComponent<Shield>();
    }

    protected override void ConstructBehaviourTree()
    {
        CheckAvailabilityNode abilityAvailability = new CheckAvailabilityNode(GetAbility());
        CheckRangeFromProjectileNode checkLeadersRangeFromProjectile = new CheckRangeFromProjectileNode(GetAgent(), Type.LEADER);
        CheckRangeFromProjectileNode checkInterceptorsRangeFromProjectile = new CheckRangeFromProjectileNode(GetAgent(), Type.INTERCEPTOR);
        FindShieldTargetNode findAllyLeader = new FindShieldTargetNode(GetAgent(), Type.LEADER);
        FindShieldTargetNode findAllyInterceptor = new FindShieldTargetNode(GetAgent(), Type.INTERCEPTOR);
        CheckRangeFromDispenserNode checkRangeFromDispenser = new CheckRangeFromDispenserNode(GetAgent());
        UseAbilityNode useAbility = new UseAbilityNode(GetAgent());

        Sequence TryOnLeader = new Sequence(new List<Node> { abilityAvailability, checkRangeFromDispenser, findAllyLeader, useAbility });
        Sequence checkOnInterceptor = new Sequence(new List<Node> { checkInterceptorsRangeFromProjectile, abilityAvailability, checkRangeFromDispenser, findAllyInterceptor, useAbility });

        Sequence checkOnLeader = new Sequence(new List<Node> { checkLeadersRangeFromProjectile, abilityAvailability, findAllyLeader, useAbility });
        Selector disposeOfAbility = new Selector(new List<Node> { checkOnInterceptor, TryOnLeader });

        topNode = new Selector(new List<Node> { checkOnLeader, disposeOfAbility });
    }
}
