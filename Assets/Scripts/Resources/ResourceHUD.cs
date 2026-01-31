using UnityEngine;
using TMPro;

public class ResourceHUD : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;

    [Header("Text Fields")]
    [SerializeField] private TMP_Text oxygenText;
    [SerializeField] private TMP_Text waterText;
    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text energyText;

    private void OnEnable()
    {
        resourceManager.OnResourcesChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        resourceManager.OnResourcesChanged -= Refresh;
    }

    private void Refresh()
    {
        oxygenText.text = $"O2: {resourceManager.Get(ResourceType.Oxygen)}";
        waterText.text  = $"W: {resourceManager.Get(ResourceType.Water)}";
        foodText.text   = $"F: {resourceManager.Get(ResourceType.Food)}";
        energyText.text = $"E: {resourceManager.Get(ResourceType.Energy)}";
    }
}
