using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    protected List<Node> nodes;

    public Sequence(List<Node> nodes_in)
    {
        nodes = nodes_in;
    }

    public override State Evaluate()
    {
        bool anyNodeIsRunning = false;
        foreach(var node in nodes)
        {
            switch(node.Evaluate())
            {
                case State.RUNNING:
                    anyNodeIsRunning = true;
                    break;
                case State.FAILURE:
                    nodeState = State.FAILURE;
                    return nodeState;
                case State.SUCCESS:
                    break;
                default:
                    break;
            }
        }
        if (anyNodeIsRunning)
            nodeState = State.RUNNING;
        else nodeState = State.SUCCESS;

        return nodeState;
    }
}
