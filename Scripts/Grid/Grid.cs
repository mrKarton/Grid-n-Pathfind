using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KartonDev.Grid
{
    public class Grid<TNodeType>
    {
        public int width, height;
        public float cellSize;

        public TNodeType[,] grid;

        public bool visualize;

        private Vector3 originPosition;
        public Grid(int width, int height, float cellSize, bool visualize, Vector3 originPosition)
        {
            grid = new TNodeType[width, height];

            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;
            this.visualize = visualize;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = default(TNodeType);
                    if(visualize)
                    {
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    }
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return (new Vector3(x, y) * cellSize) + originPosition;
        }

        public Vector3 GetPosition(int x, int y)
        {
            return GetWorldPosition(x, y) + new Vector3(cellSize / 2, cellSize / 2);
        }

        public TNodeType GetNode(int x, int y)
        {
            return grid[x, y];
        }
        public void SetNode(int x, int y, TNodeType value)
        {
            grid[x, y] = value;
        }

        public void GetXY(Vector3 position, out int x, out int y)
        {
            int xPos = Mathf.FloorToInt((position.x - originPosition.x) / cellSize),
                yPos = Mathf.FloorToInt((position.y - originPosition.y) / cellSize);


            x = xPos;
            y = yPos;
        }

        public void Mark(int x, int y, Color color)
        {
            if (x < width && y < height && x >= 0 && y >= 0)
            {
                if (visualize)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y) * cellSize, color, 200f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1) * cellSize, color, 200f);
                    Debug.DrawLine(GetWorldPosition(x, y + 1), GetWorldPosition(x + 1, y + 1) * cellSize, color, 200f);
                    Debug.DrawLine(GetWorldPosition(x + 1, y), GetWorldPosition(x + 1, y + 1) * cellSize, color, 200f);
                }
            }
        }

        public void Mark(int x, int y)
        {
            Mark(x, y, Color.red);
        }

        public void UnmarkAll()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (visualize)
                    {
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y) * cellSize, Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1) * cellSize, Color.white, 100f);
                    }
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }
}