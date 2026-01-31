using UnityEngine;

public class BuildToolbarUI : MonoBehaviour
{
    [SerializeField] private BuildController buildController;

    [Header("Room Definitions")]
    [SerializeField] private RoomDefinition oxygenRoom;
    [SerializeField] private RoomDefinition waterRoom;
    [SerializeField] private RoomDefinition foodRoom;
    [SerializeField] private RoomDefinition energyRoom;
    [SerializeField] private RoomDefinition schoolRoom;

    public void SelectOxygen() => buildController.StartBuild(oxygenRoom);
    public void SelectWater()  => buildController.StartBuild(waterRoom);
    public void SelectFood()   => buildController.StartBuild(foodRoom);
    public void SelectEnergy() => buildController.StartBuild(energyRoom);
    public void SelectSchool() => buildController.StartBuild(schoolRoom);

    public void Cancel() => buildController.CancelBuild();
}
