using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public string sceneName;
    public float timeToChangeScene;
    public float timeToReloadScene;

    public void ChangeScene()
    {
        StartCoroutine(TimeToDelayChangeScene(sceneName));
    }
    public void ChangeSceneByName(string name)
    {
        StartCoroutine(TimeToDelayChangeScene(name));
    }
    public void ReloadScene()
    {
        StartCoroutine(TimeToDelayReloadScene());
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    #region  Coroutines
    IEnumerator TimeToDelayChangeScene(string name)
    {
        yield return new WaitForSeconds(timeToChangeScene);
        SceneManager.LoadScene(name);
    }
    IEnumerator TimeToDelayReloadScene()
    {
        yield return new WaitForSeconds(timeToReloadScene);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}
