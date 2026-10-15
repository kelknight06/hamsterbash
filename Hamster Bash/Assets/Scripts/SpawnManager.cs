using UnityEngine;
using UnityEngine.InputSystem;
public class SpawnManager : MonoBehaviour
{
    public GameObject[] gemPrefabs;
    public InputAction spawnAction;
    private float spawnRangeX = 5;
    private float spawnPosZ = 4;
    private float startDelay = 3;
    private float spawnInterval = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnRandomGem", startDelay, spawnInterval);
        spawnAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnRandomGem()
    {
        //randomly generate animal index and spawn position
        int gemIndex = Random.Range(0, gemPrefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 1, Random.Range(-spawnPosZ,spawnPosZ));
        Instantiate(gemPrefabs[gemIndex], spawnPos, gemPrefabs[gemIndex].transform.rotation);

    }
}
