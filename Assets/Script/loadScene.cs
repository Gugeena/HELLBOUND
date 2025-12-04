using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour
{
    public static int SceneToLoad;
    private AsyncOperation loadAsync;

    void Start()
    {
        Functions.setUpView();
        loadAsync = SceneManager.LoadSceneAsync(SceneToLoad);
        loadAsync.allowSceneActivation = false;

        StartCoroutine(load());
    }

    IEnumerator load()
    {
        yield return new WaitForSeconds(1f);
        loadAsync.allowSceneActivation = true;
    }
}
