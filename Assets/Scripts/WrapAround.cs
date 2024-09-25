using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAround : MonoBehaviour
{
    private bool isWrappingX = false;
    private bool isWrappingY = false;
    private Rigidbody2D rb;
    public Camera gameCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        HandleScreenWrap(); // FixedUpdate over Update because Rigidbodies are used
    }

    void HandleScreenWrap()
    {
        Vector3 thisObjectsPosition = transform.position;

        // Parameters of phsyical camera size
        float camHeight = gameCamera.orthographicSize * 2f;
        float camWidth = camHeight * gameCamera.aspect;
        float leftBound = gameCamera.transform.position.x - camWidth / 2;
        float rightBound = gameCamera.transform.position.x + camWidth / 2;
        float floorBound = gameCamera.transform.position.y - camHeight / 2;
        float topBound = gameCamera.transform.position.y + camHeight / 2;

        // Check if the object is outside the camera's bounds
        if (thisObjectsPosition.x > rightBound)
        {
            thisObjectsPosition.x = leftBound; // Wrap around to the left
            isWrappingX = true;
        }
        else if (thisObjectsPosition.x < leftBound)
        {
            thisObjectsPosition.x = rightBound; // Wrap around to the right
            isWrappingX = true;
        }

        if (thisObjectsPosition.y > topBound)
        {
            thisObjectsPosition.y = floorBound; // Wrap around to the floor
            isWrappingY = true;
        }
        else if (thisObjectsPosition.y < floorBound)
        {
            thisObjectsPosition.y = topBound; // Wrap around to the top
            isWrappingY = true;
        }

        if (isWrappingX || isWrappingY)
        {
            transform.position = thisObjectsPosition;

            isWrappingX = false;
            isWrappingY = false;
        }
    }
}