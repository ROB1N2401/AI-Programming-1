using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    protected List<Node> nodes;

    public Selector(List<Node> nodes_in)
    {
        nodes = nodes_in;
    }

    public override State Evaluate()
    {
        foreach(var node in nodes)
        {
            switch(node.Evaluate())
            {
                case State.RUNNING:
                    nodeState = State.RUNNING;
                    return nodeState;
                case State.FAILURE:
                    break;
                case State.SUCCESS:
                    nodeState = State.SUCCESS;
                    return nodeState;
                default:
                    break;
            }
        }
        nodeState = State.FAILURE;

        return nodeState;
    }
}
