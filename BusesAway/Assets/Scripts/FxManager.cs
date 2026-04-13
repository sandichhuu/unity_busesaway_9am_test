using AYellowpaper.SerializedCollections;
using UnityEngine;

[System.Serializable]
public class FxManager
{
    [SerializeField, SerializedDictionary("Key", "Fx")] private SerializedDictionary<string, GameObject> particleFxDict;

    public void Emit(string key, Vector3 position)
    {
        Object.Instantiate(this.particleFxDict[key], position, Quaternion.identity);
    }
}