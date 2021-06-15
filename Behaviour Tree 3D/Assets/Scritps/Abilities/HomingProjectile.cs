using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HomingProjectile : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    Transform _target;
    float _speed = 10.0f;
    float _rotationSpeed = 720.0f;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _speed;
        _navMeshAgent.angularSpeed = _rotationSpeed;
        _navMeshAgent.acceleration = 50f;
        _navMeshAgent.avoidancePriority = 1;
        Material material = GetComponent<MeshRenderer>().material;
        material.color = new Color(186.0f / 255.0f, 0f, 255.0f / 255.0f);
    }

    private void Update()
    {
        _navMeshAgent.SetDestination(_target.position);
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    Debug.Log(_target.gameObject.name);
    //    Vector3 direction = (_target.position - transform.position).normalized;
    //    float rotateAmount = Vector3.Cross(direction, transform.forward).y;
    //    _rb.angularVelocity = new Vector3(0, rotateAmount * _rotationSpeed, 0);
    //    _rb.velocity = transform.forward * _speed;
    //}

    public void Initialize(Transform target_in, float speed_in, float rotationSpeed_in)
    {
        _target = target_in;
        _speed = speed_in;
        _rotationSpeed = rotationSpeed_in;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Agent"))
        {
            other.gameObject.GetComponent<BaseAI>().Cripple();
        }
        if (!other.gameObject.CompareTag("Dispenser"))
            Destroy(this.gameObject);
    }
}
