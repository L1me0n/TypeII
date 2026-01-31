using UnityEngine;
using TypeII.Grid;

namespace TypeII.Rooms
{
    public class BuildController : MonoBehaviour
    {
        [Header("Scene refs")]
        public GridManager grid;
        public Transform roomsRoot;

        [Header("Prefabs")]
        public RoomTile roomTilePrefab;

        [Header("Initial Rooms")]
        public bool spawnArrivalRoom = true;
        public Vector2Int arrivalCell = new Vector2Int(0, 0);

        [Header("Ghost visuals")]
        public Color validColor = new Color(1f, 1f, 1f, 0.6f);
        public Color invalidColor = new Color(1f, 0.3f, 0.3f, 0.6f);

        private bool isBuilding;
        private RoomType selectedType;

        private RoomTile ghostTile;
        private SpriteRenderer ghostRenderer;

        private void Start()
    {
        if (!spawnArrivalRoom) return;

        // Safety checks so Unity doesn't explode silently
        if (grid == null)
        {
            Debug.LogError("BuildController: GridManager reference is missing!");
            return;
        }

        if (roomsRoot == null)
        {
            Debug.LogError("BuildController: RoomsRoot reference is missing!");
            return;
        }

        if (roomTilePrefab == null)
        {
            Debug.LogError("BuildController: RoomTile prefab reference is missing!");
            return;
        }

        // If that cell is already occupied, don't place again
        if (!grid.IsInBounds(arrivalCell))
        {
            Debug.LogError($"Arrival cell {arrivalCell} is out of bounds. Adjust grid origin/size or arrivalCell.");
            return;
        }

        if (grid.IsOccupied(arrivalCell))
        {
            Debug.LogWarning($"Arrival cell {arrivalCell} is already occupied, skipping spawn.");
            return;
        }

        // Place the initial arrival room
        RoomTile tile = Instantiate(roomTilePrefab, roomsRoot);
        tile.roomType = RoomType.Arrival;
        tile.cell = arrivalCell;

        var sr = tile.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(0.6f, 0.9f, 1f, 1f); // light cyan
        }


        Vector2 world = grid.CellToWorldCenter(arrivalCell);
        tile.transform.position = new Vector3(world.x, world.y, 0);

        // Mark occupancy so player cannot build there
        grid.SetOccupied(arrivalCell, true);

        tile.gameObject.name = "ArrivalRoom";
    }


        private void Update()
        {
            if (!isBuilding) 
            {
                // Still allow cancel keys even when not building? Optional.
                return;
            }

            // Cancel build
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                CancelBuild();
                return;
            }

            Vector2Int cell = GetMouseCell();
            bool valid = CanPlaceAt(cell);

            UpdateGhost(cell, valid);

            if (valid && Input.GetMouseButtonDown(0))
            {
                Place(cell);
            }
        }

        public void StartBuild(RoomType type)
        {
            selectedType = type;
            isBuilding = true;

            EnsureGhost();
        }

        public void CancelBuild()
        {
            isBuilding = false;

            if (ghostTile != null)
            {
                Destroy(ghostTile.gameObject);
                ghostTile = null;
                ghostRenderer = null;
            }
        }

        private void EnsureGhost()
        {
            if (ghostTile != null) return;

            ghostTile = Instantiate(roomTilePrefab);
            ghostTile.name = "GhostTile";
            ghostTile.roomType = selectedType;

            ghostRenderer = ghostTile.GetComponent<SpriteRenderer>();
            if (ghostRenderer != null)
            {
                ghostRenderer.sortingOrder = 1000; // keep on top
            }

            // Ghost should not be parented to RoomsRoot because it's not a real placed room
            ghostTile.transform.SetParent(null);
        }

        private void UpdateGhost(Vector2Int cell, bool valid)
        {
            if (ghostTile == null) EnsureGhost();

            ghostTile.roomType = selectedType;
            ghostTile.cell = cell;

            Vector2 world = grid.CellToWorldCenter(cell);
            ghostTile.transform.position = new Vector3(world.x, world.y, 0);

            if (ghostRenderer != null)
                ghostRenderer.color = valid ? validColor : invalidColor;
        }

        private Vector2Int GetMouseCell()
        {
            Vector3 mouse = Input.mousePosition;
            Vector3 world = Camera.main.ScreenToWorldPoint(mouse);
            return grid.WorldToCell(world);
        }

        private bool CanPlaceAt(Vector2Int cell)
        {
            if (!grid.IsInBounds(cell)) return false;
            if (grid.IsOccupied(cell)) return false;

            return true;
        }

        private void Place(Vector2Int cell)
        {
            // Create real tile
            RoomTile tile = Instantiate(roomTilePrefab, roomsRoot);
            tile.roomType = selectedType;
            tile.cell = cell;

            Vector2 world = grid.CellToWorldCenter(cell);
            tile.transform.position = new Vector3(world.x, world.y, 0);

            // Mark occupancy
            grid.SetOccupied(cell, true);
        }
    }
}
