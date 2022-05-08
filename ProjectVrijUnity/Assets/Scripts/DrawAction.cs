using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAction : MonoBehaviour
{
    [Header("COMPONENTS")]
    public GameObject linePrefab;
    public GameObject currentLine;
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider2D;

    [Header("CURRRENT POSITIONS")]
    public List<Vector2> paintPositions;

    [Header("PAINT SPACING")]
    public float paintSpacing;

    [Header("OTHER")]
    public bool drawing = false;
    public Transform playerPosition;
    public Material lineFinished;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateLine();
            drawing = true;
        }

        if (drawing)
        {
            Vector2 temptPaintPos = playerPosition.position;
            if (Vector2.Distance(temptPaintPos, paintPositions[paintPositions.Count - 1]) > paintSpacing)
            {
                UpdateLine(temptPaintPos);
            }
        }

        if (!Input.GetMouseButton(0) && !Input.GetKey("s"))
        {
            drawing = false;
        }
    }

    void CreateLine()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        edgeCollider2D = currentLine.GetComponentInChildren<EdgeCollider2D>();
        paintPositions.Clear();
        paintPositions.Add(playerPosition.position);
        paintPositions.Add(playerPosition.position);
        lineRenderer.SetPosition(0, paintPositions[0]);
        lineRenderer.SetPosition(1, paintPositions[1]);
        edgeCollider2D.points = paintPositions.ToArray();
    }

    void UpdateLine(Vector2 newPaintPos)
    {
        paintPositions.Add(newPaintPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPaintPos);
        edgeCollider2D.points = paintPositions.ToArray();
        
        //Detecting circles
        DetectCircle();
    }

    void DetectCircle()
    {
        //Search list for position within range of player
        for (int i = 1; i < paintPositions.Count - 1; i++)
        {
            if(PaintPostionInRange(paintPositions[i], paintPositions[paintPositions.Count - 1]))
            {
                drawing = false;
                lineRenderer.material = lineFinished;

                //Tag line as circle
                currentLine.tag = "Circle";
                GameObject collider = currentLine.transform.Find("Collider").gameObject;
                collider.tag = "CircleCollider";

                //setting correct layer
                collider.layer = LayerMask.NameToLayer("CircleMask");

                //Resetting edges
                edgeCollider2D.Reset();

                //Cleanup
                cleanUpPaintPostionBefore(i);

                break;
            }
        }
    }

    bool PaintPostionInRange(Vector2 position1, Vector2 postion2)
    {
        float range = paintSpacing / 1.45f;

        if(Mathf.Abs(position1.x - postion2.x) < range && Mathf.Abs(position1.y - postion2.y) < range)
        {
            return true;
        }

        return false;
    }

    void cleanUpPaintPostionBefore(int index)
    {
        //Empty linerenderer.
        lineRenderer.positionCount = 0;
        List<Vector2> edgeColliderPoints = new List<Vector2>();

        for (int i = index; i < paintPositions.Count; i++)
        {
            lineRenderer.positionCount++;
            if (i == paintPositions.Count - 1)
            {
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, paintPositions[index]);
                edgeColliderPoints.Add(paintPositions[index]);
            }
            else
            {
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, paintPositions[i]);
                edgeColliderPoints.Add(paintPositions[i]);
            }
        }

        edgeCollider2D.points = edgeColliderPoints.ToArray();
    }
}
