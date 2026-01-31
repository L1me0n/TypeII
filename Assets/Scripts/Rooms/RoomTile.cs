using UnityEngine;

namespace TypeII.Rooms
{
    public class RoomTile : MonoBehaviour
    {
        public RoomType roomType;

        // The cell this tile occupies (for now 1x1)
        public Vector2Int cell;
    }
}
