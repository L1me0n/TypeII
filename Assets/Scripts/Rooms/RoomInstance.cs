using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RoomInstance : MonoBehaviour
{
    public RoomDefinition definition;

    [Header("Runtime State (debug)")]
    public bool isOffline;

    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        ApplyVisuals();
    }

    public void ApplyVisuals()
    {
        if (_sr == null) _sr = GetComponent<SpriteRenderer>();

        if (definition == null)
        {
            _sr.color = Color.magenta; // screams "missing data"
            return;
        }

        // Normal look
        _sr.color = definition.roomColor;
    }

    public void SetOffline(bool offline)
    {
        isOffline = offline;

        // Simple visual: darken when offline
        if (_sr == null) _sr = GetComponent<SpriteRenderer>();

        if (definition == null) return;

        _sr.color = offline ? (definition.roomColor * 0.45f) : definition.roomColor;
    }
}
