using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateScript : MonoBehaviour
{
    public bool moving = false;
    void Update()
    {
        if (Input.GetKey("w"))
        {
            moving = true;
        } else
        {
            moving = false;
        }
    }
}
