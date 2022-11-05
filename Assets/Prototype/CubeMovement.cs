using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3>();
    public CubeMovement nextCube;
    public LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.transform.SetParent(null);

        points.Add(transform.position);
        points.Add(FindObjectOfType<RandomSpawn>().transform.position);
        
        UpdateLine();
    }

    public void Update()
    {
        if (!already_moved && Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue);
            points.Insert(points.Count - 1, hit.point);
            UpdateLine();
        }
    }

    bool already_moved = false;
    bool isPaused = false;
    public void StartMove()
    {
        if (!enabled)
            return;
        if (already_moved)
            return;

        already_moved = true;
        StartCoroutine(MovementRoutine());
    }
    public void PauseMove()
    {
        isPaused = !isPaused;
    }
    IEnumerator MovementRoutine()
    {
        bool isMove = true;
        int current_point = 1;
        
        while (isMove)
        {
            while (isPaused)
                yield return new WaitForEndOfFrame();

            yield return new WaitForEndOfFrame();
            
            transform.position += (points[current_point] - transform.position).normalized * 0.1f;
            if (Vector3.Distance(transform.position, points[current_point]) <= 0.1f)
                current_point += 1;
            if (current_point >= points.Count)
                isMove = false;
        }

        if (nextCube != null)
            nextCube.enabled = true;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    private void UpdateLine()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.Select(x => (x - lineRenderer.transform.position) + new Vector3(0, 0.5f, 0)).ToArray());
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (nextCube != null && nextCube.enabled)
            Gizmos.color = Color.red;

        for (int i = 0; i < points.Count - 1; i++)
            Gizmos.DrawLine(points[i], points[i + 1]);
    }
}
