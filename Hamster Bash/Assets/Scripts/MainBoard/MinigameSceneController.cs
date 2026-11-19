using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameSceneController : MonoBehaviour
{
    [SerializeField]
    private string[] minigameScenes;

    [SerializeField]
    private GameObject boardUI;
    [SerializeField]
    private GameObject boardCamera;

    private Action<bool> resultCallback;

    private string currentMinigame;

    public void StartRandomMinigame(int difficulty, Action<bool> callback)
    {
        if (minigameScenes.Length == 0)
        {
            Debug.LogError("No Minigame Scenes Assigned!");
            return;
        }

        resultCallback = callback;

        int randomIndex = UnityEngine.Random.Range(0, minigameScenes.Length);

        currentMinigame = minigameScenes[randomIndex];

        StartCoroutine(LoadMinigame(currentMinigame, difficulty));
    }

    private IEnumerator LoadMinigame(string sceneName, int difficulty)
    {
        boardUI.SetActive(false);
        boardCamera.SetActive(false);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        yield return operation;

        Scene minigameScene = SceneManager.GetSceneByName(sceneName);

        SceneManager.SetActiveScene(minigameScene);

        Debug.Log("Loaded Minigame: " +  sceneName);
        Debug.Log("Loaded Difficulty: " + difficulty);
    }

    public void FinishMinigame(bool won)
    {
        StartCoroutine(UnloadMinigame(won));
    }

    private IEnumerator UnloadMinigame(bool won)
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(currentMinigame);

        yield return operation;

        Scene boardScene = SceneManager.GetSceneByName("Main Board");
        SceneManager.SetActiveScene(boardScene);

        boardCamera.SetActive(true);
        boardUI.SetActive(true);

        resultCallback?.Invoke(won);
        resultCallback = null;
    }
}
