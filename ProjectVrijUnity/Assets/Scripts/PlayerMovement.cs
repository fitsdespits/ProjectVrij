using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("COMPONENTS")]
    public Rigidbody2D rb;

    [Header("MOVEMENT")]
    public bool isSkating = false;
    public float moveSpeed;
    public float moveSpeedIncrease;
    public float moveSpeedDecrease;
    public float moveSpeedMaximum;

    [Header("ROTATION")]
    public float rotation;
    public float rotateSpeed;
    public float rotateSpeedIncrease;
    public float rotateSpeedDecrease;
    public float rotateSpeedMaximum;
    public float rotateSpeedMinimum;
    public float standardRotateSpeed;

    [Header("THRUSTING")]
    public bool isThrusting = false;
    public float standardThrustTime;
    public float thrustForce;
    public float thrustDrop;
    public float thrustMaximum;

    public void Start()
    {
        //RESETTING VALUES
        moveSpeed = 0;
        rotateSpeed = standardRotateSpeed;
    }

    public void Update()
    {
        //VELOCITY
        rb.AddForce(transform.right * (moveSpeed + thrustForce) * Time.deltaTime);

        //ROTATION INPUT
        rotation = Input.GetAxis("Horizontal") * -rotateSpeed * Time.deltaTime;

        //MOVEMENT INPUT
        if (Input.GetKey("space"))
        {
            isSkating = true;
        } else
        {
            isSkating = false;
        }

        Move();

        //THRUST
        if (!isThrusting && isSkating)
        {
            StartCoroutine(Thrusting());
        }
    }
    private void LateUpdate()
    {
        Rotate();
    }

    private void Move()
    {
        if (isSkating)
        {
            //MOVEMENT INCREASE
            if(moveSpeed <= moveSpeedMaximum)
            {
                moveSpeed += moveSpeedIncrease * Time.deltaTime;
            }

            if(rotateSpeed >= rotateSpeedMinimum)
            {
                rotateSpeed -= rotateSpeedDecrease * Time.deltaTime;
            }
   
        } else
        {
            //MOVEMENT DECREASE
            if (moveSpeed >= 0)
            {
                moveSpeed -= moveSpeedDecrease * Time.deltaTime;
            }

            if(rotateSpeed <= rotateSpeedMaximum)
            {
                rotateSpeed += rotateSpeedIncrease * Time.deltaTime;
            }
        }

        if(moveSpeed < 0)
        {
            moveSpeed = 0;
        }
    }

    private void Rotate()
    {
        transform.Rotate(0f, 0f, rotation);
    }

    IEnumerator Thrusting()
    {
        isThrusting = true;

        Debug.Log("THRUST");

        yield return new WaitForSecondsRealtime(standardThrustTime + moveSpeed/180);

        isThrusting = false;
    }
}
