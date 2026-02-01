using UnityEngine;

public class BuildUIController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private BuildController buildController;
    [SerializeField] private CanvasGroup buildPanelGroup;

    [Header("Behavior")]
    [SerializeField] private bool startHidden = true;
    [SerializeField] private bool autoClosePanelOnRoomSelect = true;

    private void Awake()
    {
        if (buildPanelGroup == null)
            buildPanelGroup = GetComponent<CanvasGroup>();

        if (startHidden)
            SetPanelVisible(false);
        else
            SetPanelVisible(true);
    }

    private void Update()
    {
        // If panel is not visible, don't do anything
        if (!IsPanelVisible()) return;

        // Close panel on Esc or Right Click even if no room was selected
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            // If build mode is active, cancel it. If not active, CancelBuild() is still safe.
            if (buildController != null)
            buildController.CancelBuild();

            SetPanelVisible(false);
        }
    }

    private bool IsPanelVisible()
    {
        return buildPanelGroup != null && buildPanelGroup.alpha > 0.01f;
    }

    // Called by the big BUILD button
    public void ToggleBuildPanel()
    {
        bool isVisible = buildPanelGroup.alpha > 0.01f;
        SetPanelVisible(!isVisible);
    }

    // Called by room buttons (pass RoomDefinition asset in OnClick)
    public void SelectRoom(RoomDefinition definition)
    {
        if (definition == null) return;

        // Start build mode
        buildController.StartBuild(definition);

        // Optional: close the panel so it doesn't cover the grid
        if (autoClosePanelOnRoomSelect)
            SetPanelVisible(false);
    }

    // Called by Cancel button
    public void CancelBuildAndClose()
    {
        buildController.CancelBuild();
        SetPanelVisible(false);
    }

    // Optional: separate "close only"
    public void ClosePanel()
    {
        SetPanelVisible(false);
    }

    private void SetPanelVisible(bool visible)
    {
        buildPanelGroup.alpha = visible ? 1f : 0f;
        buildPanelGroup.interactable = visible;
        buildPanelGroup.blocksRaycasts = visible;
    }

    private void OnEnable()
    {
        buildController.OnBuildCanceled += HandleBuildCanceled;
    }

    private void OnDisable()
    {
        buildController.OnBuildCanceled -= HandleBuildCanceled;
    }

    private void HandleBuildCanceled()
    {
        SetPanelVisible(false);
    }

}
