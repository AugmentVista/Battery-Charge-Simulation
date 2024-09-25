using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    // List of collided objects
    private List<GameObject> collidedObjects = new List<GameObject>();

    private float speed = 40.0f;

    private Vector2 target;
    public Vector2 startingPosition;

    private Rigidbody2D rb;
    private Rigidbody2D rbCollidedWith;

    private bool timerFinished;
    private bool hasTarget;
    private bool isFrozen = false;

    public float targetGoalRadius;

    // Static list to hold starting positions across all instances
    private static List<Vector2> availablePositions = new List<Vector2>();


    private void Start()
    {
        startingPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        SetInitialDirection();

        target = Vector2.zero;
        hasTarget = false;

        availablePositions.Add(startingPosition);

        StartCoroutine(ManagePositions());
    }

    private void SetInitialDirection()
    {
        int randomDirection = Random.Range(0, 4);
        switch (randomDirection)
        {
            case 0:
                rb.velocity = Vector2.left * speed;
                break;
            case 1:
                rb.velocity = Vector2.right * speed;
                break;
            case 2:
                rb.velocity = Vector2.up * speed;
                break;
            case 3:
                rb.velocity = Vector2.down * speed;
                break;
        }
    }

    private void Update()
    {
        if (hasTarget && !isFrozen)
        {
            MoveTowardsTarget();

            if (Vector2.Distance(transform.position, target) <= targetGoalRadius)
            {
                ReverseDirection();
                hasTarget = false; // clear target when reached
            }
        }
        Debug.Log("Current Position: " + transform.position);
        FreezeCheck();
    }

    void OnGUI()
    {
        if (Input.GetMouseButtonDown(0)) // Get mouse position and set as target
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target = mousePos; // target becomes mouse location on click
            hasTarget = true;
        }
    }
    private IEnumerator ManagePositions()
    {
        yield return new WaitForSeconds(5);
        timerFinished = true;
    }

    private void FreezeCheck()
    {
        if (!isFrozen && availablePositions.Count > 0 && timerFinished)
        {
            Vector2 currentPosition = transform.position;

            foreach (Vector2 position in availablePositions)
            {
                if (Vector2.Distance(currentPosition, position) < 0.05f) // Doesn't need to be exact just very close
                {
                    rb.velocity = Vector2.zero;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY; // Freeze position
                    isFrozen = true; 
                    availablePositions.Remove(position); // Remove to prevent conflict
                    break; 
                }
            }
        }
    }



    private void MoveTowardsTarget()
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void ReverseDirection()
    {
        rb.velocity = -rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OrbitObject") && !isFrozen)
        {
            GameObject collidedObject = collision.gameObject;

            // Check isn't in the list
            if (!collidedObjects.Contains(collidedObject))
            {
                collidedObjects.Add(collidedObject);
            }

            // Check if there are 3 or more objects in the list
            if (collidedObjects.Count >= 3 && !hasTarget)
            {
                // Choose a random object in list to be target
                int randomIndex = Random.Range(0, collidedObjects.Count);
                target = collidedObjects[randomIndex].transform.position;
                hasTarget = true;
            }

            Rigidbody2D rbCollidedWith = collidedObject.GetComponent<Rigidbody2D>();

            if (rbCollidedWith != null)
            {
                int randomEffect = Random.Range(0, 4);
                switch (randomEffect)
                {
                    case 0:
                        rbCollidedWith.velocity = Vector2.left * speed;
                        break;
                    case 1:
                        rbCollidedWith.velocity = Vector2.right * speed;
                        break;
                    case 2:
                        rbCollidedWith.velocity = Vector2.up * speed;
                        break;
                    case 3:
                        rbCollidedWith.velocity = Vector2.down * speed;
                        break;
                }
            }
        }
    }
}