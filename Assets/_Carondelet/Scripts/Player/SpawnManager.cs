using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform defaultSpawnPoint;

    IEnumerator Start()
    {
        // Wait one frame to ensure all scene objects are loaded
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) yield break;

        Transform targetSpawn = GetSpawnPoint();
        player.transform.SetPositionAndRotation(targetSpawn.position, targetSpawn.rotation);
    }

    private Transform GetSpawnPoint()
    {
        // Early exit if no door was used
        if (string.IsNullOrEmpty(DoorManager.Instance.LastDoorUsed))
            return defaultSpawnPoint;

        // Find all ACTIVE doors in the scene
        DoorSceneLoader[] doors = FindObjectsByType<DoorSceneLoader>(FindObjectsSortMode.InstanceID); // Include inactive
        foreach (DoorSceneLoader door in doors)
        {
            // Add null check and explicit string comparison
            if (door != null && door.doorID == DoorManager.Instance.LastDoorUsed)
            {
                return door.GetSpawnPoint();
            }
        }

        Debug.LogWarning($"Door {DoorManager.Instance.LastDoorUsed} not found! Using default spawn.");
        return defaultSpawnPoint;
    }
}