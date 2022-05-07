using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public GameObject circle;
    public EdgeCollider2D edgeCollider2D;
    public LineRenderer lineRenderer;

    private bool init = false;

    //Fading
    public float fadeCooldown;
    public int health;
    public bool fading = false;

    //Collected information
    public List<Vector2> circlePositions;

    void Update()
    {
        //Check if line has turned into a circle
        if(circle.tag == "Circle" && !init)
        {
            CircleInitialise();
        }

        //Fading lines.
        if (circle.tag != "Circle" && !fading)
        {
            if(health <= 0)
            {
                Destroy(circle);
            } else
            {
                //StartCoroutine(Fade());
            }
        }
    }

    private void CircleInitialise()
    {
        init = true;

        //Collecting information from DrawAction script
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        circlePositions = player.GetComponent<DrawAction>().paintPositions;

        //Locating largest X and Y points in the list
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
        Debug.Log("X: " + xMax + ", " + xMin);
        Debug.Log("Y: " + yMax + ", " + yMin);

        //Tagging critters inside box
        GameObject[] critters = GameObject.FindGameObjectsWithTag("UnlitCritter");
        for (int c = 0; c < critters.Length; c++)
        {
            GameObject thisCritter = critters[c];
            if (edgeCollider2D.bounds.Contains(thisCritter.transform.position))
            {
                Debug.Log(thisCritter);
            }
        }
    }

    IEnumerator Fade()
    {
        fading = true;
        health -= 1;

        //Colorfade
        int a = 50;
        Color color = new Color(52, 33, 100, a);
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetColor("_BaseColor", color);
        lineRenderer.material = mat;




        yield return new WaitForSecondsRealtime(fadeCooldown);
        fading = false;
    }
}
