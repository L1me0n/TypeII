using UnityEngine;
using TypeII.Rooms;

namespace TypeII.UI
{
    public class BuildToolbarUI : MonoBehaviour
    {
        public BuildController buildController;

        public void SelectRoomA()
        {
            buildController.StartBuild(RoomType.RoomA);
        }

        public void SelectRoomB()
        {
            buildController.StartBuild(RoomType.RoomB);
        }

        public void Cancel()
        {
            buildController.CancelBuild();
        }
    }
}
