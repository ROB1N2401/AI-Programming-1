using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    protected Node node;

    public Inverter(Node node_in)
    {
        node = node_in;
    }

    public override State Evaluate()
    {
        switch(node.Evaluate())
        {
            case State.RUNNING:
                break;
            case State.FAILURE:
                nodeState = State.SUCCESS;
                return nodeState;
            case State.SUCCESS:
                nodeState = State.FAILURE;
                break;
            default:
                break;
        }

        return nodeState;
    }
}
