using System;
using System.Collections.Generic;
using UnityEngine;

public class InstanceIdService : IInstanceIdService
{
    private readonly Dictionary<string, GameObject> _byId = new();
    private readonly Dictionary<int, string> _byGoInstance = new();

    public string NewId(string prefix = null)
    {
        var raw = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("=", "")
            .Replace("+", "")
            .Replace("/", "");

        return string.IsNullOrWhiteSpace(prefix) ? raw : $"{prefix}_{raw}";
    }

    public bool Register(string id, GameObject go)
    {
        if (string.IsNullOrWhiteSpace(id) || go == null) return false;

        var goKey = go.GetInstanceID();
        if (_byGoInstance.TryGetValue(goKey, out var oldId))
        {
            _byGoInstance.Remove(goKey);
            _byId.Remove(oldId);
        }

        if (_byId.ContainsKey(id))
        {
            Debug.LogError($"[InstanceIdService] Duplicate ID detected: {id}");
            return false;
        }

        _byId[id] = go;
        _byGoInstance[goKey] = id;
        return true;
    }

    public void Unregister(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return;

        if (_byId.TryGetValue(id, out var go) && go != null)
            _byGoInstance.Remove(go.GetInstanceID());

        _byId.Remove(id);
    }

    public void Unregister(GameObject go)
    {
        if (go == null) return;

        var key = go.GetInstanceID();
        if (_byGoInstance.TryGetValue(key, out var id))
        {
            _byGoInstance.Remove(key);
            _byId.Remove(id);
        }
    }

    public bool TryResolve(string id, out GameObject go)
    {
        go = null;
        if (string.IsNullOrWhiteSpace(id)) return false;

        if (_byId.TryGetValue(id, out go))
        {
            // Unity fake-null (zniszczony obiekt)
            if (go == null)
            {
                _byId.Remove(id);
                return false;
            }
            return true;
        }

        return false;
    }

    public GameObject Resolve(string id) => TryResolve(id, out var go) ? go : null;

    // ✅ helpery “jak w tej klasie Id”
    public GameObject Find(string id) => Resolve(id);

    public bool TryFind(string id, out GameObject go) => TryResolve(id, out go);

    public bool Contains(string id) => !string.IsNullOrWhiteSpace(id) && _byId.ContainsKey(id);

    public void Clear()
    {
        _byId.Clear();
        _byGoInstance.Clear();
    }
}
