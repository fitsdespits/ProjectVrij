using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("COMPONENTS")]
    public Rigidbody2D rb;
    public ParticleSystem skateDust1;
    public ParticleSystem skateDust2;

    [Header("MOVEMENT")]
    public bool isSkating = false;
    public bool startBoost = false;
    public float moveSpeed;
    public float moveSpeedIncrease;
    public float moveSpeedDecrease;
    public float moveSpeedMaximum;

    [Header("ROTATION")]
    public bool isRotating = false;
    public float rotation;
    public float rotateSpeed;
    public float rotateSpeedIncrease;
    public float rotateSpeedDecrease;
    public float rotateSpeedMaximum;
    public float rotateSpeedMinimum;
    public float rotationBoost;

    [Header("THRUSTING")]
    public bool isThrusting = false;
    public int thrustSide = 0;
    public float thrustCooldown;

    public void Start()
    {
        //RESETTING VALUES
        moveSpeed = 0;
    }

    public void Update()
    {
        //VELOCITY
        if (!isRotating)
        {
            rb.AddForce(transform.right * moveSpeed * Time.deltaTime);
        } else
        {
            rb.AddForce(transform.right * (moveSpeed * rotationBoost) * Time.deltaTime);
        }

        //ROTATION INPUT
        rotation = Input.GetAxis("Horizontal") * -rotateSpeed * Time.deltaTime;

        if(rotation != 0)
        {
            isRotating = true;
        } else
        {
            isRotating = false;
        }

        //MOVEMENT INPUT
        if (Input.GetKey("space"))
        {
            isSkating = true;

            if (!startBoost)
            {
                startBoost = true;
                if(moveSpeed < moveSpeedMaximum / 3)
                {
                    moveSpeed = moveSpeedMaximum / 3;
                }
            }
        } else
        {
            isSkating = false;
            startBoost = false;
        }

        Move();

        //THRUSTING
        if (isSkating && !isThrusting)
        {
            StartCoroutine(Thrust());
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
            if(moveSpeed < moveSpeedMaximum)
            {
                moveSpeed += moveSpeedIncrease * Time.deltaTime;
            }

            if(rotateSpeed < rotateSpeedMaximum)
            {
                rotateSpeed += rotateSpeedIncrease * Time.deltaTime;
            }
   
        } else
        {
            //MOVEMENT DECREASE
            if (moveSpeed > 0)
            {
                moveSpeed -= moveSpeedDecrease * Time.deltaTime;
            }

            if(rotateSpeed > rotateSpeedMinimum)
            {
                rotateSpeed -= rotateSpeedDecrease * Time.deltaTime;
            }
        }

        if(moveSpeed < 0)
        {
            moveSpeed = 0;
        }
    }

    IEnumerator Thrust()
    {
        isThrusting = true;
        SkateDust();
        yield return new WaitForSecondsRealtime(thrustCooldown);
        isThrusting = false;
    }

    private void Rotate()
    {
        transform.Rotate(0f, 0f, rotation);
    }

    private void SkateDust()
    {
        if(thrustSide == 1)
        {
            thrustSide = 0;
            skateDust1.Play();
        } else
        {
            if(thrustSide == 0)
            {
                thrustSide = 1;
                skateDust2.Play();
            }
        }
    }
}
