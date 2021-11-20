using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KartonDev.Grid;

namespace KartonDev.Pathfinding
{ 
    public class Pathfinder
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        public Grid<PathNode> grid;
        public List<PathNode> openList;
        public List<PathNode> closedList;

        public string[] unwalkableTags;
        public Pathfinder(int width, int height, string[] unwalkable)
        {
            grid = new Grid<PathNode>(width, height, 0.2f, false, Vector3.zero);
            unwalkableTags = unwalkable;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid.SetNode(x, y, new PathNode(x, y, true, grid));
                }
            }
        }

        public Pathfinder(int width, int height)
        {
            grid = new Grid<PathNode>(width, height, 1f, true, Vector3.zero);
            unwalkableTags = new string[0];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid.SetNode(x, y, new PathNode(x, y, true, grid));
                }
            }
        }

        public List<PathNode> GetPath(Vector2Int start, Vector2Int end)
        {
            UpdateWalkable();

            PathNode startNode = grid.GetNode(start.x, start.y);
            PathNode endNode = grid.GetNode(end.x, end.y);

            openList = new List<PathNode> { startNode };
            closedList = new List<PathNode>();

            for (int x = 0; x < grid.width; x++)
            {
                for (int y = 0;  y < grid.height; y++)
                {
                    PathNode node = grid.GetNode(x, y);
                    node.gCost = int.MaxValue;
                    node.CalculateFCost();
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistance(startNode, endNode);

            startNode.CalculateFCost();

            while(openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCost(openList);
                if(currentNode == endNode)
                {
                    return CalculatePath(currentNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach(PathNode neighbour in PathNode.GetNeigbours(grid, currentNode.ToVector2Int()))
                {
                    if (closedList.Contains(neighbour)) continue;
                    if (!neighbour.isWalkable)
                    {
                        closedList.Add(neighbour);
                        continue;
                    }
                    int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neighbour);
                    if(tentativeGCost < neighbour.gCost)
                    {
                        neighbour.cameFromNode = currentNode;
                        neighbour.gCost = tentativeGCost;
                        neighbour.hCost = CalculateDistance(neighbour, endNode);
                        neighbour.CalculateFCost();

                        if(!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }

            //if out of nodes in open list. Just return something. It always return from cicle
            return null;
        }

        public List<PathNode> GetPath(Vector3 start, Vector3 end)
        {
            int x, y;

            grid.GetXY(start, out x, out y);
            Vector2Int startPos = new Vector2Int(x, y);
            grid.GetXY(end, out x, out y);
            Vector2Int endPos = new Vector2Int(x, y);

            return GetPath(startPos, endPos);
        }

        public List<Vector3> NodeListToVector3(List<PathNode> list)
        {
            List<Vector3> path = new List<Vector3>();
            
            foreach (PathNode node in list)
            {
                path.Add(grid.GetPosition(node.X, node.Y));
            }

            return path;
        }

        public List<PathNode> CalculatePath(PathNode end)
        {
            List<PathNode> path = new List<PathNode> { end };
            PathNode currentNode = end;

            while(currentNode.cameFromNode != null)
            {
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }

            path.Reverse();

            return path;
        }

        public int CalculateDistance(PathNode start, PathNode end)
        {
            int xDist = Mathf.Abs(start.X - end.X);
            int yDist = Mathf.Abs(start.Y - end.Y);
            int remaining = Mathf.Abs(xDist - yDist);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDist, yDist) + MOVE_DIAGONAL_COST * remaining;
        }

        public PathNode GetLowestFCost(List<PathNode> list)
        {
            PathNode lowest = list[0];
            foreach(PathNode node in list)
            {
                if(node.fCost < lowest.fCost)
                {
                    lowest = node;
                }
            }

            return lowest;
        }

        public void UpdateWalkable()
        {
            for (int x = 0; x < grid.width; x++)
            {
                for (int y = 0; y < grid.height; y++)
                {
                    Vector3 pos = grid.GetPosition(x, y);

                    RaycastHit2D hit = Physics2D.BoxCast(pos, new Vector2(1, 1), 0, Vector2.zero);

                    if(hit)
                    {
                        if(System.Array.IndexOf(unwalkableTags, hit.collider.tag) != -1)
                        {
                            grid.GetNode(x, y).isWalkable = false;
                        }
                        else
                        {
                            grid.GetNode(x, y).isWalkable = true;
                        }
                    }
                    else
                    {
                        grid.GetNode(x, y).isWalkable = true;
                    }
                    
                }
            }
        }
    }
}
