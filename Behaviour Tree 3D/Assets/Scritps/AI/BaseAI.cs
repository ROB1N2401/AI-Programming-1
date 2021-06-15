using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Team
{
    RED,
    GREEN,
    BLUE
}

public enum Type
{
    LEADER,
    DEFENDER,
    INTERCEPTOR
}

public abstract class BaseAI : MonoBehaviour
{
    Camera _cam;
    Transform _finishPoint;
    NavMeshAgent _meshAgent;
    NavMeshPath _meshPath;
    Agent _agent;
    bool _isCrippled = false;
    float _timeElapsed = 0.0f;
    float _crippleTimeElapsed = 0.0f;
    const float _duration = 0.5f;

    protected Node topNode;
    protected Ability ability;

    void Start()
    {
        _cam = Camera.main;
        _meshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        _meshPath = new NavMeshPath();

        _finishPoint = GameObject.Find("Destination Point").GetComponent<Transform>();

        _meshAgent.SetDestination(_finishPoint.position);
        _meshAgent.CalculatePath(_meshAgent.destination, _meshPath);

        GetAbilities();
        ConstructBehaviourTree();
        //Debug.Log(meshPath.corners.Length);

        //Destroy(this.GetComponent<BoxCollider>());
    }

    void Update()
    {
        UpdateCripple();
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed >= _duration)
        {
            _timeElapsed -= _duration;
            topNode.Evaluate();
        }
    }

    public Agent GetAgent()
    {
        return _agent;
    }
    public void SetAgent(Agent agent_in)
    {
        _agent = agent_in;
    }

    public Ability GetAbility()
    {
        return ability;
    }


    public Node GetTopNode()
    {
        return topNode;
    }

    public void SetTopNode(Node topNode_in)
    {
        topNode = topNode_in;
    }


    public void StopAgent()
    {
        GameObject.Find("Game Manager").GetComponent<Results>().CountIn(_agent);
        _meshAgent.isStopped = true;
    }

    public bool CheckIfFinished()
    {
        return _meshAgent.isStopped;
    }

    public float CalculateRemainingDistance()
    {
        float distance = 0.0f;

        if (_meshAgent == null)
            Debug.LogError("BaseAI.cs: _meshAgent has not been received");

        _meshAgent.CalculatePath(_meshAgent.destination, _meshPath);

        for (int i = 0; i < _meshPath.corners.Length - 1; i++)
        {
            distance += Vector3.Distance(_meshPath.corners[i], _meshPath.corners[i + 1]);
        }

        //Debug.Log("Distance is " + distance);

        return distance;
    }

    public void UpdateCripple()
    {
        if (_isCrippled)
        {
            _crippleTimeElapsed += Time.deltaTime;
            if (_crippleTimeElapsed >= _duration)
            {
                _crippleTimeElapsed -= _duration;
                Uncripple();
            }
        }
    }

    public void Cripple()
    {
        _isCrippled = true;

        var main = GetComponent<ParticleSystem>().main;
        main.startColor = new Color(0, 0, 0);
        GetComponent<ParticleSystem>().Play();

        _meshAgent.speed = _agent.Speed / 3.0f;
        _meshAgent.angularSpeed = _agent.AngularSpeed / 3.0f;
        _meshAgent.acceleration = _agent.Acceleration / 3.0f;
    }

    public void Uncripple()
    {
        _isCrippled = false;
        _meshAgent.speed = _agent.Speed;
        _meshAgent.angularSpeed = _agent.AngularSpeed;
        _meshAgent.acceleration = _agent.Acceleration;
    }

    protected abstract void GetAbilities();
    protected abstract void ConstructBehaviourTree();
}
