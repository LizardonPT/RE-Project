using UnityEngine;

public class CoalSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject coalPrefab;
    [SerializeField] private Transform spawnPoint;

    public void SpawnPrefab()
    {
        if (coalPrefab == null)
        {
            Debug.LogWarning("Prefab não atribuído!");
            return;
        }

        Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion rotation = spawnPoint != null ? spawnPoint.rotation : transform.rotation;

        Instantiate(coalPrefab, position, rotation);
    }
}
