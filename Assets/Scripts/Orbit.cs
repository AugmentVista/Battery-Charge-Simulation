using System.Collections.Generic;
using UnityEngine;
public class Orbit : MonoBehaviour
{
    // List of collided objects
    private List<GameObject> collidedObjects = new List<GameObject>(); 

    private float speed = 20;

    private Vector2 target;

    private Rigidbody2D rb;
    private Rigidbody2D rbCollidedWith;

    private bool hasTarget;

    public float targetGoalRadius;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetInitialDirection();

        target = Vector2.zero;
        hasTarget = false;
    }

    private void Update()
    {
        if (hasTarget)
        {
            MoveTowardsTarget();

            if (Vector2.Distance(transform.position, target) <= targetGoalRadius)
            {
                ReverseDirection();
                hasTarget = false; // clear target when reached
            }
        }
        Debug.Log("Current Position: " + transform.position);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OrbitObject"))
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

    void OnGUI()
    {
        if (Input.GetMouseButtonDown(0)) // Get mouse position and set as target
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target = mousePos; // target becomes mouse location on click
            hasTarget = true;
        }
    }
}
