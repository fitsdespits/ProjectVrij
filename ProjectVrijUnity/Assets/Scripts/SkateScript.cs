using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateScript : MonoBehaviour
{
    [Header("COMPONENTS")]
    public Rigidbody2D rb;

    [Header("MOVEMENT")]
    public bool isSkating = false;
    public float moveSpeed;
    public float moveSpeedIncrease;
    public float moveSpeedMax;
    public float standardMoveSpeed;

    [Header("ROTATION")]
    public float rotation;
    public float rotateSpeed;
    public float rotateSpeedDecrease;
    public float rotateSpeedMax;
    public float standardRotateSpeed;

    private void Start()
    {
        moveSpeed = standardMoveSpeed;
        rotateSpeed = standardRotateSpeed;
    }

    void Update()
    {
        //get rotation input.
        rotation = Input.GetAxis("Horizontal") * -rotateSpeed * Time.deltaTime;

        //get movement input.
        if (Input.GetKey("space"))
        {
            Move();
        } else
        {
            isSkating = false;
        }

        //increasing movespeed
        if (isSkating)
        {
            //increasing movementspeed when
            if(moveSpeed <= moveSpeedMax)
            {
                moveSpeed += moveSpeedIncrease;
                Debug.Log("++");
            }
        } else
        {
            if(moveSpeed >= standardMoveSpeed)
            {
                moveSpeed -= moveSpeedIncrease;
                Debug.Log("--");
            }
        }
    }

    private void LateUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(0f, 0f, rotation);
    }

    private void Move()
    {
        if (!isSkating)
        {
            isSkating = true;
        }
        rb.AddForce(transform.right * moveSpeed * Time.deltaTime);
    }
}
