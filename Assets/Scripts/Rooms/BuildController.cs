using TypeII.Grid;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridManager grid;
    [SerializeField] private Transform roomsRoot;
    [SerializeField] private RoomTile roomTilePrefab;

    [Header("Ghost Tint")]
    [SerializeField] private Color ghostValidTint = new Color(1f, 1f, 1f, 0.55f);
    [SerializeField] private Color ghostInvalidTint = new Color(1f, 0.2f, 0.2f, 0.55f);

    [Header("Arrival Room (auto spawn)")]
    [SerializeField] private bool spawnArrivalRoom = true;
    [SerializeField] private Vector2Int arrivalCell = new Vector2Int(0, 0);
    [SerializeField] private RoomDefinition arrivalDefinition;

    private bool _isBuilding;
    private RoomDefinition _selectedDefinition;

    private RoomTile _ghostTile;
    private SpriteRenderer _ghostRenderer;
    private RoomInstance _ghostInstance;

    private void Start()
    {
        if (spawnArrivalRoom && arrivalDefinition != null)
            SpawnRoom(arrivalCell, arrivalDefinition);
    }

    private void Update()
    {
        if (!_isBuilding) return;

        // Cancel should always work
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            CancelBuild();
            return;
        }

        // Always update ghost position (even if mouse is over UI)
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int cell = grid.WorldToCell(mouseWorld);

        bool canPlace = CanPlaceAt(cell);
        UpdateGhost(cell, canPlace);

        // But block PLACEMENT if clicking UI
        bool pointerOverUI = IsPointerOverBlockingUI();

        if (Input.GetMouseButtonDown(0))
        {
            if (pointerOverUI) return;  // <--- key line

            if (canPlace)
                SpawnRoom(cell, _selectedDefinition);
        }
    }

    public void StartBuild(RoomDefinition definition)
    {
        if (definition == null) return;

        _isBuilding = true;
        _selectedDefinition = definition;

        if (_ghostTile == null)
            CreateGhost();

        ApplyGhostDefinition();
    }

    public void CancelBuild()
    {
        _isBuilding = false;
        _selectedDefinition = null;

        if (_ghostTile != null)
            Destroy(_ghostTile.gameObject);

        _ghostTile = null;
        _ghostRenderer = null;
        _ghostInstance = null;
    }

    private bool CanPlaceAt(Vector2Int cell)
    {
        if (!grid.IsInBounds(cell)) return false;
        if (grid.IsOccupied(cell)) return false;
        return true;
    }

    private void CreateGhost()
    {
        _ghostTile = Instantiate(roomTilePrefab);
        _ghostTile.name = "GhostRoom";

        _ghostRenderer = _ghostTile.GetComponent<SpriteRenderer>();
        _ghostInstance = _ghostTile.GetComponent<RoomInstance>();

        ApplyGhostDefinition();
    }

    private void ApplyGhostDefinition()
    {
        if (_ghostInstance == null) return;

        _ghostInstance.definition = _selectedDefinition;
        _ghostInstance.ApplyVisuals();

        // Force ghost transparency (ApplyVisuals might set opaque color)
        if (_ghostRenderer != null)
            _ghostRenderer.color = ghostValidTint;
    }

    private void UpdateGhost(Vector2Int cell, bool canPlace)
    {
        if (_ghostTile == null) CreateGhost();

        _ghostTile.cell = cell;
        _ghostTile.transform.position = grid.CellToWorldCenter(cell);

        if (_ghostRenderer != null)
            _ghostRenderer.color = canPlace ? ghostValidTint : ghostInvalidTint;
    }

    private void SpawnRoom(Vector2Int cell, RoomDefinition definition)
    {
        var tile = Instantiate(roomTilePrefab, roomsRoot);
        tile.cell = cell;
        tile.transform.position = grid.CellToWorldCenter(cell);

        var inst = tile.GetComponent<RoomInstance>();
        inst.definition = definition;
        inst.ApplyVisuals();
        inst.SetOffline(false);

        tile.name = definition != null ? definition.displayName : "Room";
        grid.SetOccupied(cell, true);
    }

    private static bool IsPointerOverBlockingUI()
    {
        if (EventSystem.current == null) return false;

        var data = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        // Block only if the top UI under the cursor is actually interactable (Button, Slider, TMP_InputField, etc.)
        foreach (var r in results)
        {
            if (r.gameObject.GetComponentInParent<Selectable>() != null)
                return true;
        }

        return false;
}

}
