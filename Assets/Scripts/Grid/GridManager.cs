using UnityEngine;

namespace TypeII.Grid
{
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Size (cells)")]
        public int width = 30;
        public int height = 20;

        [Header("Cell Size (world units)")]
        public float cellSize = 1f;

        [Header("Grid Origin (world position)")]
        public Vector2 origin = Vector2.zero;

        // Occupancy map: true = something built here
        private bool[,] occupied;

        private void Awake()
        {
            occupied = new bool[width, height];
        }

        public bool IsInBounds(Vector2Int cell)
        {
            return cell.x >= 0 && cell.x < width && cell.y >= 0 && cell.y < height;
        }

        public bool IsOccupied(Vector2Int cell)
        {
            if (!IsInBounds(cell)) return true; // out of bounds counts as blocked
            return occupied[cell.x, cell.y];
        }

        public void SetOccupied(Vector2Int cell, bool value)
        {
            if (!IsInBounds(cell)) return;
            occupied[cell.x, cell.y] = value;
        }

        public Vector2Int WorldToCell(Vector2 worldPos)
        {
            Vector2 local = worldPos - origin;
            int x = Mathf.FloorToInt(local.x / cellSize);
            int y = Mathf.FloorToInt(local.y / cellSize);
            return new Vector2Int(x, y);
        }

        public Vector2 CellToWorldCenter(Vector2Int cell)
        {
            float x = origin.x + (cell.x + 0.5f) * cellSize;
            float y = origin.y + (cell.y + 0.5f) * cellSize;
            return new Vector2(x, y);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // Draw grid outline and lines in Scene view when selected
            Gizmos.color = Color.gray;

            Vector3 bottomLeft = new Vector3(origin.x, origin.y, 0);
            Vector3 topRight = new Vector3(origin.x + width * cellSize, origin.y + height * cellSize, 0);

            // Border
            Gizmos.DrawLine(bottomLeft, new Vector3(topRight.x, bottomLeft.y, 0));
            Gizmos.DrawLine(bottomLeft, new Vector3(bottomLeft.x, topRight.y, 0));
            Gizmos.DrawLine(new Vector3(topRight.x, bottomLeft.y, 0), topRight);
            Gizmos.DrawLine(new Vector3(bottomLeft.x, topRight.y, 0), topRight);

            // Vertical lines
            for (int x = 1; x < width; x++)
            {
                float wx = origin.x + x * cellSize;
                Gizmos.DrawLine(new Vector3(wx, origin.y, 0), new Vector3(wx, origin.y + height * cellSize, 0));
            }

            // Horizontal lines
            for (int y = 1; y < height; y++)
            {
                float wy = origin.y + y * cellSize;
                Gizmos.DrawLine(new Vector3(origin.x, wy, 0), new Vector3(origin.x + width * cellSize, wy, 0));
            }
        }
#endif
    }
}
