using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{

    public EnemyScript self;
    public Vector3 target;
    public float slowRadiusL;
    public float targetRadiusL;
    public float slowRadiusA;
    public float targetRadiusA;
    public float timeToTarget;
    public float maxSpeed;
    public float maxAcceleration;
    public float maxAngularAcceleration;
    public float maxRotation;

    // For wander function
    public float wanderOffset;
    public float wanderRadius;
    public float wanderRate;
    private float wanderOrientation;

    public struct wanderSteering
    {
        public float angular;
        public Vector3 linear;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void fixedUpdate()
    {

    }
}
