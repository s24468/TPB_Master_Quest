// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class IDHolder : MonoBehaviour
// {
//     public string UniqueID;
//     public static List<IDHolder> AllIDHolders = new List<IDHolder>();
//
//     private void Awake()
//     {
//         AllIDHolders.Add(this);
//     }
//
//     public static GameObject GetGameObjectWithID(string ID)
//     {
//         foreach (var i in AllIDHolders)
//         {
//             if (i.UniqueID == ID)
//             {
//                 return i.gameObject;
//             }
//         }
//         return null;
//     }
//     private void OnDestroy()
//     {
//         // usuń z listy, gdy GO/komponent jest niszczony
//         AllIDHolders.Remove(this);
//     }
//     public static void ClearIDHoldersList()
//     {
//         AllIDHolders.Clear();
//     }
// }

using UnityEngine;

public class IDHolder : MonoBehaviour
{
    [SerializeField] private string instanceId;

    public string InstanceId => instanceId;

    private IInstanceIdService _ids;

    // ✅ alias dla starego kodu
    public string UniqueID => instanceId;


    private void Awake()
    {
        _ids = Services.Get<IInstanceIdService>();

        // jeśli ID nie ustawione z zewnątrz — generujemy
        if (string.IsNullOrWhiteSpace(instanceId))
        {
            instanceId = _ids.NewId("card"); // prefix opcjonalny
        }

        _ids.Register(instanceId, gameObject);
    }

    // Wywołasz to zaraz po Instantiate, zanim Awake? -> nie, Awake już mogło polecieć.
    // Więc użyjemy SetBeforeRegister w wariancie poniżej.
    public void ForceSetIdAndReregister(string newId)
    {
        if (_ids == null) _ids = Services.Get<IInstanceIdService>();

        // usuń stare
        if (!string.IsNullOrWhiteSpace(instanceId))
            _ids.Unregister(instanceId);

        instanceId = newId;
        _ids.Register(instanceId, gameObject);
    }

    private void OnDestroy()
    {
        if (_ids == null && Services.TryGet<IInstanceIdService>(out var ids))
        {
            _ids = ids;
        }

        if (_ids != null)
        {
            _ids.Unregister(gameObject);
        }
    }

    public static GameObject Find(string id)
    {
        var ids = Services.Get<IInstanceIdService>();
        return ids.Resolve(id);
    }
}