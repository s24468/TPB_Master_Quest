using UnityEngine;

public class GameServicesBootstrap : MonoBehaviour
{
    private void Awake()
    {
        // Tworzysz instancję serwisu normalnie (bez singletona)
        Services.Register<IInstanceIdService>(new InstanceIdService());
    }

    private void OnDestroy()
    {
        // opcjonalnie (jak scena się kończy)
        if (Services.TryGet<IInstanceIdService>(out var ids))
            ids.Clear();

        Services.Clear();
    }
}