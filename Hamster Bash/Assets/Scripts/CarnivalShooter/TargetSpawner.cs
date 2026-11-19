using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;

    [SerializeField]
    private int targetsPerGroup;
    [SerializeField]
    private int targetValue;
    [SerializeField]
    private int targetTime;
    [SerializeField]
    private int minimumGoodTargets = 5;
    [SerializeField]
    [Range(0, 1f)]
    private float badTargetChance = 0.5f;

    private Coroutine spawningCoroutine;

    [Header("Tutorial Target Spawns")]
    public Transform badTargetSpawn;
    public Transform startGameSpawn;

    [Header("Stationary Target Spawns")]
    [SerializeField]
    private Transform stationarySpawnParent;
    private Transform[] stationarySpawnPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int count = stationarySpawnParent.childCount;
        stationarySpawnPoints = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            stationarySpawnPoints[i] = stationarySpawnParent.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginSpawning(int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                targetsPerGroup = 10;
                targetValue = 50;
                targetTime = 8;
                break;
            case 2:
                targetsPerGroup = 14;
                targetValue = 30;
                targetTime = 6;
                break;
            case 3:
                targetsPerGroup = 18;
                targetValue = 25;
                targetTime = 5;
                break;
        }
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
        }

        spawningCoroutine = StartCoroutine(SpawnTargetGroups());
    }

    private IEnumerator SpawnTargetGroups()
    {
        while (true)
        {
            SpawnStationaryTargets();

            yield return new WaitForSeconds(targetTime);

            yield return new WaitForSeconds(1f);
        }
    }

    // Spawns a group of stationary targets
    public void SpawnStationaryTargets()
    {
        if (stationarySpawnPoints == null || stationarySpawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned");
            return;
        }

        List<Transform> possibleSpawns = new List<Transform>(stationarySpawnPoints);

        int garanteeGoodTargets = Mathf.Clamp(minimumGoodTargets, 0, targetsPerGroup);

        for (int i = 0; i < targetsPerGroup; i++)
        {
            int randomIndex = Random.Range(0, possibleSpawns.Count);
            Transform spawnPoint = possibleSpawns[randomIndex];
            possibleSpawns.RemoveAt(randomIndex);

            GameObject newTarget = Instantiate(targetPrefab, spawnPoint.position, spawnPoint.rotation);
            Target targetScript = newTarget.GetComponent<Target>();
            targetScript.AdjustValues(targetValue, targetTime);

            Target.TargetTypes targetType;

            // Make sure a certain number of good targets are spawned
            if (i < garanteeGoodTargets)
            {
                targetType = Target.TargetTypes.Good;
            } 
            else
            {
                float randomValue = Random.value;

                if (randomValue < badTargetChance) // After garanteed good targets spawn, theres now chance to spawn bad target.
                {
                    targetType = Target.TargetTypes.Bad;
                } 
                else
                {
                    targetType = Target.TargetTypes.Good;
                }
            }
            // Activate target with given type
            targetScript.ActivateTarget(targetType);
        }

    }

    // CarnivalShooter will call this when mini game timer runs out
    public void StopSpawningTargets()
    {
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
            spawningCoroutine = null;
        }
    }
    // Spawns Bad Target for tutorial
    public void BadTutorialTarget()
    {
        GameObject badTutorialTarget = Instantiate(targetPrefab, badTargetSpawn.position, badTargetSpawn.rotation);
        Target targetScript = badTutorialTarget.GetComponent<Target>();
        targetScript.ActivateTarget(Target.TargetTypes.Bad);
        targetScript.AdjustValues(50, 999f);
    }
    // Spawns Start Game Target for tutorial
    public void StartGameTarget()
    {
        GameObject startGameTarget = Instantiate(targetPrefab, startGameSpawn.position, startGameSpawn.rotation);
        Target targetScript = startGameTarget.GetComponent<Target>();
        targetScript.ActivateTarget(Target.TargetTypes.StartGame);
    }
}
