using UnityEngine;

public class TickSystem : MonoBehaviour
{
    [SerializeField] private float tickIntervalSeconds = 1f;

    public System.Action OnTick;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= tickIntervalSeconds)
        {
            _timer -= tickIntervalSeconds;
            OnTick?.Invoke();
        }
    }
}
