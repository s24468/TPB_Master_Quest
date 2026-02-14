using UnityEngine;

public interface IInstanceIdService
{
    string NewId(string prefix = null);

    bool Register(string id, GameObject go);
    void Unregister(string id);
    void Unregister(GameObject go);

    bool TryResolve(string id, out GameObject go);
    GameObject Resolve(string id);
    // ✅ “helpery” w serwisie do szukania
    GameObject Find(string id);
    bool TryFind(string id, out GameObject go);
    bool Contains(string id);
    void Clear();
}