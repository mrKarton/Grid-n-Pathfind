using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KartonDev.Grid;

namespace KartonDev.Pathfinding
{
    public class PathNode
    {
        private int x, y;
        public int X { get { return this.x; } }
        public int Y { get { return this.y; } }


        public int gCost, hCost, fCost;
        public Grid<PathNode> grid;
        public bool isWalkable; 

        public PathNode cameFromNode;

        public PathNode(int x, int y, bool isWalkable,Grid<PathNode> grid)
        {
            this.x = x;
            this.y = y;
            this.grid = grid;
            this.isWalkable = isWalkable;
            cameFromNode = null;

        }
        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        public Vector2Int ToVector2Int()
        {
            return new Vector2Int(X, Y);
        }

        public static List<PathNode> GetNeigbours(Grid<PathNode> grid, Vector2Int position)
        {
            List<PathNode> neighbours = new List<PathNode>();

            if (position.y - 1 >= 0) neighbours.Add(grid.GetNode(position.x, position.y - 1));
            if (position.y + 1 < grid.height) neighbours.Add(grid.GetNode(position.x, position.y + 1));

            if (position.x + 1 < grid.width)
            {
                neighbours.Add(grid.GetNode(position.x + 1, position.y));

                if (position.y - 1 >= 0) neighbours.Add(grid.GetNode(position.x + 1, position.y - 1));
                if (position.y + 1 < grid.height) neighbours.Add(grid.GetNode(position.x + 1, position.y + 1));

                
            }
            if(position.x - 1 >= 0)
            {
                neighbours.Add(grid.GetNode(position.x - 1, position.y));

                if (position.y - 1 >= 0) neighbours.Add(grid.GetNode(position.x - 1, position.y - 1));
                if (position.y + 1 < grid.height) neighbours.Add(grid.GetNode(position.x - 1, position.y + 1));
            }

            

            return neighbours;
        }
    }
}