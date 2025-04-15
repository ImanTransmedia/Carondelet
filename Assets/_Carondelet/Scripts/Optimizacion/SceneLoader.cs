using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SceneLoader : MonoBehaviour
{
    private string nameSceneLoad;

    private void Start()
    {
        nameSceneLoad = gameObject.name;
    }

    public void StartAsyncLoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    public IEnumerator SceneLoad()
    {
        string doorName = nameSceneLoad;
        AsyncOperation operation = SceneManager.LoadSceneAsync(doorName);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Main Player")
        {
            Debug.Log("Colision!");
            StartCoroutine(SceneLoad());
        }
    }
}
