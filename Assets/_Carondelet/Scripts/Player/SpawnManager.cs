using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform defaultSpawnPoint;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //Transform targetSpawn = GetSpawnPoint();

        //player.transform.position = targetSpawn.position;
        //player.transform.rotation = targetSpawn.rotation;
    }

    //private Transform GetSpawnPoint()
    //{
    //    if (string.IsNullOrEmpty(DoorManager.Instance.LastDoorUsed))
    //        return defaultSpawnPoint;
    //
    //    foreach (SceneLoader door in FindObjectsByType<SceneLoader>())
    //    {
    //        if (door.DoorID == GameManager.Instance.LastDoorUsed)
    //            return door.GetSpawnPoint();
    //    }
    //
    //    return defaultSpawnPoint;
    //}
}