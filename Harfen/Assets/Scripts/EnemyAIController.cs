using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{

    public EnemyScript self;
    public Transform target;
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
        self = this.GetComponent<EnemyScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //slowly arrive at position
    public Vector3 DynamicArrive()
    {
        //get the distance between target and agent
        Vector3 direction = GetDirectionVec();
        float distance = GetDistance();
        //distance check
        if (distance < slowRadiusL && distance > targetRadiusL)
        {
            //here is the condition we need to think about reduce speed
            float temp = distance - targetRadiusL;
            float targetSpeed = maxSpeed * (temp / (slowRadiusL - targetRadiusL));
            Vector3 targetVelocity = direction.normalized * targetSpeed;
            Vector3 linear = targetVelocity - self.velocity;
            linear /= timeToTarget;
            /*
             * Note: since linear /= timeToTarget will get the acceleration
             * we need to perform per time, we need to devide the linear with
             * another deltaTime in order to make sure the enough acceleration
             * is applied to the gameobject
             */
            linear /= Time.deltaTime;
            return linear;
        }

        return new Vector3(0, 0, 0);
    }

    public float Align(Vector3 targetVec)
    {
        //get direction to target
        Vector3 direction = targetVec - self.position;
        float rotation = Mathf.Atan2(direction.x, direction.z) - self.orientation;
        float targetRotation, steering, rotationDirection;

        //map result to (-pi, pi) interval
        rotation = MapOrientation(rotation);
        float rotationSize = Mathf.Abs(rotation);

        //check if we are there
        if (rotationSize < targetRadiusA) { return 0f; }

        //if oiutside slow radius use max rotation
        if (rotationSize > slowRadiusA) { targetRotation = maxRotation; }

        //otherwise use scaled rotation
        else { targetRotation = maxRotation * (rotationSize / slowRadiusL); }

        //final target rotation use speed and direction
        targetRotation *= rotation / rotationSize;

        //acceleration tries to get to the target rotation
        steering = targetRotation - self.rotation;

        //check if acceleration is too high
        float angularAccel = Mathf.Abs(steering);
        if (angularAccel > maxAngularAcceleration)
        {
            steering /= angularAccel;
            steering *= maxAngularAcceleration;
        }
        return steering;
    }

    //random wandering;
    public wanderSteering Wander()
    {
        //update orientation
        wanderOrientation += (Random.value - Random.value) * wanderRate;

        //get target orientation
        float targetOr = self.orientation + wanderOrientation;

        //get center of wander cirlce
        Vector3 position = self.position + wanderOffset * new Vector3(Mathf.Sin(self.orientation), 0, Mathf.Cos(self.orientation));

        //get target locations
        position += wanderRadius * new Vector3(Mathf.Sin(targetOr), 0, Mathf.Cos(targetOr));

        //create struct to hold output
        wanderSteering ret;

        //delegate to face
        ret.angular = Align(target.position);

        if ((target.position - self.position).magnitude < slowRadiusL)
        {
            ret.linear = DynamicArrive();
            return ret;
        }
        //else keep max acceleration
        else
        {
            ret.linear = maxAcceleration * new Vector3(Mathf.Sin(self.orientation), 0, Mathf.Cos(self.orientation));
            return ret;
        }
    }

    //this function returns the distance between agent and target
    private float GetDistance()
    {
        float result = (target.position - self.position).magnitude;
        return result;
    }

    //this function returns the vector from agent to target
    private Vector3 GetDirectionVec()
    {
        Vector3 result = target.position - self.position;
        return result;
    }

    private float MapOrientation(float i)
    {
        //we will map the value to (-pi, pi] interval
        if (i <= Mathf.PI && i > -Mathf.PI)
        {
            return i;
        }
        else if (i > Mathf.PI)
        {
            while (i > Mathf.PI)
            {
                i -= 2 * Mathf.PI;
            }
            return i;
        }
        else
        {
            while (i <= -Mathf.PI)
            {
                i += 2 * Mathf.PI;
            }
            return i;
        }
    }
}
