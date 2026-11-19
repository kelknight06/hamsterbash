using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// This is Hamster Bash's main Game Manager.
// It is in charge of handling the flow of the entire game.
public class GameManager : MonoBehaviour
{
    [Header("Game Status")]
    [SerializeField]
    private int difficulty;
    [SerializeField]
    private int seedPoints;

    [Header("Seed Points")]
    [SerializeField]
    private TextMeshProUGUI seedPointsText;

    [SerializeField]
    [Tooltip("Link Player Here")]
    private PlayerBoardMovement playerBoardMovement;

    [Header("Dice Information")]
    [SerializeField]
    [Tooltip("Link Dice Block Prefab Here")]
    private GameObject diceBlockPrefab;
    [Tooltip("Transfrom Oject Where Dice Box Spawns\nIdeally this should be attached to Camera.")]
    public Transform diceSpawnPoint;

    [Header("Minigame Management")]
    [SerializeField]
    [Tooltip("Link the Minigame Scene Controller Here")]
    private MinigameSceneController minigameSceneController;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        difficulty = 1;
        seedPoints = 0;

        AddSeedPoints(0);
        CreateDice();
    }

    // --------UI Updating---------
    private void AddSeedPoints(int points)
    {
        seedPoints += points;
        seedPointsText.text = "Seed Points: " + seedPoints;
    }


    // --------Dice Rolling--------
    [ContextMenu("SpawnDice")]
    private void CreateDice()
    {
        StartCoroutine(SpawnDice());
    }
    private IEnumerator SpawnDice()
    {
        yield return new WaitForSeconds(2);

        GameObject diceObject = Instantiate(diceBlockPrefab, diceSpawnPoint.position, diceSpawnPoint.rotation);
    }

    public void MovePlayer(int spacesToMove)
    {
        playerBoardMovement.MovePlayer(spacesToMove);
    }

    public void RequestPathChoice(IReadOnlyList<BoardSpace> options)
    {
        // Display selection UI here
    }

    public void SelectPath(BoardSpace selectedSoace)
    {
        playerBoardMovement.ChoosePath(selectedSoace);
    }

    public void OnPlayerLanded(BoardSpace landedSpace)
    {
        switch (landedSpace.spaceType)
        {
            case SpaceType.Normal:
                Debug.Log("Landed on Normal Space");
                // Handle normal space behavior
                break;

            case SpaceType.Minigame:
                Debug.Log("Landed on Minigame Space");
                PlayMinigame();
                break;

            case SpaceType.Special:
                Debug.Log("Landed on Special Space");
                // Handle special space behavior
                break;
        }
    }


    // ------Minigame Management--------
    [ContextMenu("Test Play Minigame")]
    public void PlayMinigame()
    {
        StartCoroutine(LoadMiniGame());
    }
    private IEnumerator LoadMiniGame()
    {
        yield return new WaitForSeconds(3f);

        minigameSceneController.StartRandomMinigame(difficulty, OnMinigameFinished);
    }

    private void OnMinigameFinished(bool won)
    {
        Debug.Log("GameManager received minigame result: " + won);

        if (won)
        {
            // Whatever happens when player wins minigame
            AddSeedPoints(20);

            CreateDice();
        }
        else
        {
            // Whatever happens when player loses minigame

            PlayMinigame();
        }
    }
}