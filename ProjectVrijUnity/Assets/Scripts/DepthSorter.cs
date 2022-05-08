using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class DepthSorter : MonoBehaviour
{
    private void Update()
    {
        this.GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y * -10);
    }
}
