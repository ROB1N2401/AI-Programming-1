using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Node
{
    protected State nodeState;

    //public State NodeState { get { return nodeState; } }
    public abstract State Evaluate();
}

public enum State
{
    RUNNING,
    SUCCESS,
    FAILURE
}