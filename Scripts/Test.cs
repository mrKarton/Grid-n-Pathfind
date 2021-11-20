using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KartonDev.Grid;
using KartonDev.Pathfinding;

public class Test : MonoBehaviour
{
    Pathfinder pathfinder;
    void Start()
    {
        pathfinder = new Pathfinder(125, 50, new string[1] { "wall" });

    }

    int x, y;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinder.grid.GetXY(mousePos, out x, out y);

            List<PathNode> path = pathfinder.GetPath(transform.position, mousePos);
            if (path != null)
            {
                List<Vector3> vectorPath = pathfinder.NodeListToVector3(path);

                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(pathfinder.grid.GetPosition(path[i].X, path[i].Y), pathfinder.grid.GetPosition(path[i + 1].X, path[i + 1].Y), Color.green, 1000f);
                }

                StartCoroutine(GoByList(vectorPath));
            }
            else
            {
                Debug.LogWarning("Path not found");
            }
        }
    }

    public IEnumerator GoByList(List<Vector3> path)
    {
        foreach(Vector3 vector in path)
        {
            while(transform.position != vector)
            {
                transform.position = Vector3.MoveTowards(transform.position, vector, Time.deltaTime * 5);

                yield return new WaitForEndOfFrame();
            }
        }
    }

   
}
