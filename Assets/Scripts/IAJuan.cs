using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAJuan : MonoBehaviour {

    [Tooltip("Triggers special animation for driving posture")]
    public bool driving;
    [Tooltip("True if the navigation should use navmesh or transform")]
    public bool useNavmesh;
    [Tooltip("The speed of the character")]
    public float vitesse;
    [Tooltip("List of every point on the dummy path")]
    public List<DummyPath> path = new List<DummyPath>();

    public bool dead = false, waiting = true;

    NavMeshAgent agent;
    Animator myAnimator;

    Transform currentTarget;
    int currentPath = 0;

    void Start()
    {
        WalkToPoint(path[currentPath].transform);
    }

    void OnEnable()
    {
        Start();
    }

    void Update()
    {
        // If dead, reduce the velocity of the skeleton rigidbodies to prevent wobbles
        if (dead)
        {
            this.enabled = false;
        }
        // Check if the dummy should walk
        if (!dead)
        {
            if (!waiting)
            {

                Walk();
                if (CheckIfArrived())
                {
                    WalkToNextPoint();
                }
            }
        }
    }

    // DIIIIIIE !
    public void Die()
    {
        dead = true;
    }

    // Set destination and start walking towards a point
    public void WalkToPoint(Transform point)
    {
        if (!dead)
        {
            currentTarget = point;
            waiting = false;
            if (useNavmesh)
            {
                // agent.SetDestination(point.position);

            }
        }
    }

    // Walk to next point in the path point list
    void WalkToNextPoint()
    {
        if (currentPath < path.Count - 1)
        {
            currentPath++;
        }
        else
        {
            currentPath = 0;
        }
        WalkToPoint(path[currentPath].transform);
    }

    public void StopWalking()
    {
        waiting = true;
        if (driving)
        {
            return;
        }
        myAnimator.CrossFade("Wait", 0.2f);
    }


    void Walk()
    {
        if (useNavmesh)
        {
            // agent.enabled = true;
            // agent.SetDestination(currentTarget.position);
            return;
        }
        else
        {
            // agent.enabled = false;
        }
        // Look at target
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.2f);
        Vector3 localForward = transform.worldToLocalMatrix.MultiplyVector(transform.forward);

        if (Random.Range(0f,1f)<0.15f)
            localForward = localForward + new Vector3(Random.Range(-0.3f, 0.3f), 0f, Random.Range(-0.3f, 0.3f));

        // Walk toward target
        transform.Translate(localForward * Time.deltaTime * vitesse);
    }

    private bool CheckIfArrived()
    {
        // If close enough from point, tell it's arrived
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.45f)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Worker")
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 15f)
            Die();
    }

}
