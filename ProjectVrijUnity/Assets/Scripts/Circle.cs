using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [Header("COMPONENTS")]
    public GameObject circle;
    public EdgeCollider2D edgeCollider2D;
    public LineRenderer lineRenderer;

    [Header("INITIALISATION")]
    public bool init = false;

    [Header("INFORMATION")]
    public List<Vector2> circlePositions;
    public List<GameObject> capturedCritters;

    [Header("FADING")]
    public float fadeCooldown;
    public int health;
    public bool fading = false;

    void Update()
    {
        //Check if line has turned into a circle
        if (circle.tag == "Circle" && !init)
        {
            CircleInitialise();
        }

        //Fading lines.
        if (circle.tag != "Circle" && !fading)
        {
            if (health <= 0)
            {
                Destroy(circle);
            }
            else
            {
                StartCoroutine(Fade());
            }
        }
    }

    private void CircleInitialise()
    {
        init = true;

        //Collecting information from DrawAction script
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        circlePositions = player.GetComponent<DrawAction>().paintPositions;

        //Locating largest X and Y points in the list and farthest distance
        float xMax = circlePositions[0].x;
        float xMin = circlePositions[0].x;
        float yMax = circlePositions[0].y;
        float yMin = circlePositions[0].y;
        for (int i = 0; i < circlePositions.Count; i++)
        {
            xMax = Mathf.Max(circlePositions[i].x, xMax);
            xMin = Mathf.Min(circlePositions[i].x, xMin);
            yMax = Mathf.Max(circlePositions[i].y, yMax);
            yMin = Mathf.Min(circlePositions[i].y, yMin);
        }

        float xFarthestDistance = xMax - xMin;
        float yFarthestDistance = yMax - yMin;
        float farthestDistance = Mathf.Max(xFarthestDistance, yFarthestDistance);
        farthestDistance = Mathf.Abs(farthestDistance);

        //Tagging critters inside box
        GameObject[] critters = GameObject.FindGameObjectsWithTag("UnlitCritter");
        LayerMask mask = LayerMask.GetMask("CircleMask");

        for (int c = 0; c < critters.Length; c++)
        {
            GameObject thisCritter = critters[c];
            if (edgeCollider2D.bounds.Contains(thisCritter.transform.position))
            {
                //Raycasting around critter
                int castScore = 0;
                Transform critterPos = thisCritter.transform;

                for (int r = 0; r <= 360; r += 1)
                {
                    //Rotating critter
                    critterPos.rotation = Quaternion.Euler(new Vector3(0, 0, r));

                    //Raycast
                    RaycastHit2D hit = Physics2D.Raycast(critterPos.position, critterPos.TransformDirection(Vector2.up), farthestDistance + 5, mask);

                    //Debug.DrawRay(critterPos.position, critterPos.TransformDirection(Vector2.up) * (farthestDistance + 5), Color.green, 5);

                    if (hit != false && hit.collider != null)
                    {
                        //Check if the hit collider is actually this circle's collider
                        if (hit.collider == edgeCollider2D)
                        {
                            castScore += 1;
                        }
                    }
                }

                if (castScore == 361)
                {
                    thisCritter.tag = "LitCritter";
                    capturedCritters.Add(thisCritter);
                }
            }
        }

        GameObject collider = this.transform.Find("Collider").gameObject;
        collider.layer = LayerMask.NameToLayer("CircleComplete");

        if (capturedCritters.Count > 1)
        {
            Debug.Log("The player captured " + capturedCritters.Count + " critters at " + player.transform.position + "!");
        }

        if (capturedCritters.Count == 1)
        {
            Debug.Log("The player captured " + capturedCritters.Count + " critter at " + player.transform.position + "!");
        }
    }

    IEnumerator Fade()
    {
        fading = true;
        health -= 1;

        yield return new WaitForSecondsRealtime(fadeCooldown);
        fading = false;
    }
}
