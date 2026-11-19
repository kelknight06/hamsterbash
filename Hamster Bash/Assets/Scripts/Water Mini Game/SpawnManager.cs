using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class SpawnManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public GameObject gameOver;
    public GameObject gameWin;
    public GameObject titleScreen;
    public GameObject player;
    public GameObject[] goodGems;
    public GameObject[] badGems;
    private float goodGemChance = 70f;
    public GameObject leftImage;
    public GameObject rightImage;
    public GameObject middleImage;
    public GameObject buildScreen;
    
    public List<RectTransform> toyPieces;
    public List<RectTransform> spawnLocations;

    public InputAction spawnAction;
    public  Button buildButton;
    public float timeValue = 60;

    private float spawnRangeX = 35;
    private float spawnPosZ = 40;
    private float startDelay = 1.0f;
    private float spawnRate = 1.0f;
    public float currentTimeRate = 1.0f;
    public bool isGameActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame(int difficulty)
    {
        //currentDifficulty = difficulty;
        isGameActive = true;
        if (difficulty == 1)
        {
            goodGemChance = 80f;
        }
        else if (difficulty == 2)
        {
            goodGemChance = 50f;
        }
        else if (difficulty == 3)
        {
            goodGemChance = 25f;
        }
        
        InvokeRepeating("SpawnRandomGem", startDelay, spawnRate);
        spawnAction.Enable();
        titleScreen.SetActive(false);
        player.SetActive(true);

        if (buildButton != null)
        {
            buildButton.onClick.RemoveAllListeners(); 
            buildButton.onClick.AddListener(RandomizePics);
        }

    }

    void SpawnRandomGem()
    {
        if (!isGameActive)
        {
            return;
        }
        float roll = Random.Range(0f, 100f);
        GameObject[] choicePool;
        if (roll < goodGemChance && goodGems.Length > 0)
        {
            choicePool = goodGems;
        }
        else if (badGems.Length > 0) {
            choicePool = badGems;
        }
        else
        {
            return;
        }
          int gemIndex = Random.Range(0, choicePool.Length);
          Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 2, Random.Range(-spawnPosZ, spawnPosZ));
          Instantiate(choicePool[gemIndex], spawnPos, choicePool[gemIndex].transform.rotation);

    }
    public void IncreaseTimeRate(float time)
    {
        currentTimeRate += time;
    }

    public void Update()
    {
        if (isGameActive == true)
        {
            TimeLeft(currentTimeRate);
            CheckPieces();
        }
        if (timeValue < 0)
        {
            GameOver();
        }
    }
  
    public float TimeLeft(float timeRate)
    {

        timeValue -= Time.deltaTime * timeRate;
        timeText.text = "Time: " + Mathf.Round(timeValue);
        return timeValue;

     
    }
    public void CheckPieces()
    {
        if (leftImage.activeSelf && rightImage.activeSelf && middleImage.activeSelf)
        {
            buildScreen.SetActive(true);
            CancelInvoke("SpawnRandomGem");
            player.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            spawnAction.Disable();
        }
    }
  
    public void RandomizePics()
    {
        if (spawnLocations.Count < toyPieces.Count)
        {
            Debug.LogError("not enough locations for all toy pieces.");
        }
        List<RectTransform> availableSpots = new List<RectTransform>(spawnLocations);
        for (int i = 0; i < availableSpots.Count; i++)
        {
            RectTransform temp = availableSpots[i];
            int randomIndex = Random.Range(i, availableSpots.Count);
            availableSpots[i] = availableSpots[randomIndex];
            availableSpots[randomIndex] = temp;
        }
        for (int i = 0; i < toyPieces.Count; i++)
        {
            toyPieces[i].anchoredPosition = availableSpots[i].anchoredPosition;
        }
        CheckWin();
    }
    public void CheckWin()
    {
        for (int i = 0; i < toyPieces.Count; i++)
        {
            if (Vector2.Distance(toyPieces[i].anchoredPosition, spawnLocations[i].anchoredPosition) > 0.1f)
            {
                Debug.Log("Not the correct order, keep trying.");
                return;
            }
        }
        Debug.Log("You built the toy!");
        isGameActive = false;
        leftImage.SetActive(false);
        rightImage.SetActive(false);
        middleImage.SetActive(false);
        gameWin.gameObject.SetActive(true);
        buildScreen.gameObject.SetActive(false);

    }

    public void GameOver()
    {
        buildScreen.SetActive(false);
        leftImage.SetActive(false);
        rightImage.SetActive(false);
        middleImage.SetActive(false);
        gameOver.gameObject.SetActive(true);
        isGameActive = false;
        CancelInvoke("SpawnRandomGem");
        player.SetActive(false);
        spawnAction.Disable();
        
    }
}
