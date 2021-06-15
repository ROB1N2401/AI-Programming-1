using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent 
{
    BaseAI _aiComponent;
    GameObject _gameObjectHolder = null;
    readonly Team _agentTeam;
    readonly Type _agentType;
    readonly string _name;
    readonly float _acceleration;
    readonly float _angularSpeed;
    readonly float _speed;
    readonly int _pointsMultiplier;
    
    public int Position;

    public BaseAI AIComponent { get => _aiComponent; set => _aiComponent = value; }
    public GameObject GameObjectHolder { get => _gameObjectHolder; set => _gameObjectHolder = value; }
    public Team AgentTeam => _agentTeam;
    public Type AgentType => _agentType;
    public int PointsMultiplier => _pointsMultiplier;
    public float Acceleration => _acceleration;
    public float AngularSpeed => _angularSpeed;
    public float Speed => _speed;

    public Agent(Team agentTeam_in, Type agentType_in)
    {
        _name = "";
        _agentTeam = agentTeam_in;
        _agentType = agentType_in;
        Position = 0;
        Vector3 pos = new Vector3(-5f, 0.5f, -9f);

        switch (_agentType)
        {
            case Type.LEADER:
                _gameObjectHolder = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _aiComponent = _gameObjectHolder.AddComponent<LeaderAI>();
                _angularSpeed = 180;
                _acceleration = 10;
                _speed = 6f;
                _pointsMultiplier = 4;
                _name += "Leader ";
                break;
            case Type.DEFENDER:
                _gameObjectHolder = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                _aiComponent = _gameObjectHolder.AddComponent<DefenderAI>();
                _angularSpeed = 180;
                _acceleration = 10;
                _speed = 5f;
                _pointsMultiplier = 2;
                _name += "Defender ";
                pos += Vector3.forward;
                break;
            case Type.INTERCEPTOR:
                _gameObjectHolder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                _aiComponent = _gameObjectHolder.AddComponent<InterceptorAI>();
                _angularSpeed = 120;
                _acceleration = 8;
                _speed = 5.5f;
                _pointsMultiplier = 3;
                _name += "Interceptor ";
                pos += (Vector3.forward * 2);
                break;
            default:
                Debug.LogError(agentType_in + " Set to faulty value");
                break;
        }
        
        Material material = _gameObjectHolder.GetComponent<MeshRenderer>().material;
        ParticleSystem particleSystem = _gameObjectHolder.AddComponent<ParticleSystem>();
        particleSystem.Stop();
        ParticleSystemRenderer particleSystemRenderer = _gameObjectHolder.GetComponent<ParticleSystemRenderer>();
        particleSystemRenderer.material = Resources.Load("Particles", typeof(Material)) as Material;
        particleSystemRenderer.renderMode = ParticleSystemRenderMode.HorizontalBillboard;
        var main = particleSystem.main;
        main.startSize = 2f;
        main.playOnAwake = false;
        main.loop = false;
        main.startSpeed = 5.0f;
        main.duration = 1.0f;
        main.gravityModifier = 1;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        var shape = particleSystem.shape;
        shape.rotation = new Vector3(-90, 0, 0);


        switch (_agentTeam)
        {
            case Team.BLUE:
                material.color = Color.blue;
                _name += "Blue";
                break;
            case Team.RED:
                material.color = Color.red;
                pos += Vector3.right;
                _name += "Red";
                break;
            case Team.GREEN:
                material.color = Color.green;
                pos += (Vector3.right * 2);
                _name += "Green";
                break;
            default:
                break;
        }

        _gameObjectHolder.name = _name;
        _gameObjectHolder.tag = "Agent";
        _gameObjectHolder.transform.position = pos;
        _gameObjectHolder.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        NavMeshAgent navMesh = _gameObjectHolder.AddComponent<NavMeshAgent>();
        navMesh.angularSpeed = _angularSpeed;
        navMesh.acceleration = _acceleration;
        navMesh.speed = _speed;
        switch (_agentType)
        {
            case Type.LEADER:
                navMesh.avoidancePriority = 10;
                break;
            case Type.DEFENDER:
                navMesh.avoidancePriority = 30;
                break;
            case Type.INTERCEPTOR:
                navMesh.avoidancePriority = 20;
                break;
            default:
                Debug.LogError(agentType_in + " Set to faulty value");
                break;
        }

        _aiComponent.SetAgent(this);
    }
}
