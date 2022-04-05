using UnityEngine;

[CreateAssetMenu(fileName = "GovnaData", menuName = "Test govna", order = 1)]
public class SpawnManagerScriptableObject : ScriptableObject
{
    public string prefabName;

    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;
}