using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public GameObject player;

    //for enemy AI
    public float maxSpeed;
    public float orientation;
    public float rotation;
    public Vector3 position;
    public Vector3 velocity;

    private float angular;
    private Vector3 linear;

    private EnemyAIController ai;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        /*ai = this.GetComponent<EnemyAIController>();
        rb = this.GetComponent<Rigidbody>();
        position = rb.position;
        orientation = transform.eulerAngles.y;*/
        //this.GetComponent<Animation>().Play("Walk");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*//get new linear and angular velocity
        linear = ai.Wander().linear;
        angular = ai.Wander().angular;

        //update values from wander function
        orientation += rotation * Time.fixedDeltaTime;
        velocity += linear * Time.fixedDeltaTime;
        rotation += angular * Time.fixedDeltaTime;

        //don't exceed max velocity
        if (velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }

        //add forces to rb
        rb.AddForce(velocity - rb.velocity, ForceMode.VelocityChange);
        position = rb.position;
        rb.MoveRotation(Quaternion.Euler(new Vector3(0, Mathf.Rad2Deg * orientation, 0)));*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("hit by: " + collision.gameObject.tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(player.GetComponent<PlayerController>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sword Stab"))
        {
            Kill();
        }

    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
