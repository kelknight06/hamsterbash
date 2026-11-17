
using UnityEngine;
using System.Collections.Generic;

public class GateSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject leftGatePrefab;
    public GameObject rightGatePrefab;

    [Header("Spawn Settings")]
    public float spawnDistance = 25f;
    public float gateSpacing = 6f;
    public int initialGatePairs = 3;
    public int maxGatePairs = 5;

    private float nextSpawnZ;
    private List<GameObject> spawnedGates =
        new List<GameObject>();

    private void Start()
    {
        nextSpawnZ = player.position.z + spawnDistance;

        // Spawn the starting gate pairs.
        for (int i = 0; i < initialGatePairs; i++)
        {
            SpawnGatePair();
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        // Spawn more gates as the player moves forward.
        if (player.position.z + spawnDistance >= nextSpawnZ)
        {
            SpawnGatePair();
        }

        RemoveOldGates();
    }

    private void SpawnGatePair()
    {
        if (MathGameManager.Instance == null)
            return;

        Vector3 leftPosition = new Vector3(
            -gateSpacing / 2f,
            0,
            nextSpawnZ
        );

        Vector3 rightPosition = new Vector3(
            gateSpacing / 2f,
            0,
            nextSpawnZ
        );

        GameObject left = Instantiate(
            leftGatePrefab,
            leftPosition,
            Quaternion.identity
        );

        GameObject right = Instantiate(
            rightGatePrefab,
            rightPosition,
            Quaternion.identity
        );

        MathGate leftScript = left.GetComponent<MathGate>();
        MathGate rightScript = right.GetComponent<MathGate>();

        spawnedGates.Add(left);
        spawnedGates.Add(right);

        // Generate a new question for this pair.
        MathGameManager.Instance.SetupGatePair(
            leftScript,
            rightScript
        );

        nextSpawnZ += gateSpacing * 2f;
    }

    private void RemoveOldGates()
    {
        for (int i = spawnedGates.Count - 1; i >= 0; i--)
        {
            if (spawnedGates[i] == null)
            {
                spawnedGates.RemoveAt(i);
                continue;
            }

            if (spawnedGates[i].transform.position.z <
                player.position.z - 10f)
            {
                Destroy(spawnedGates[i]);
                spawnedGates.RemoveAt(i);
            }
        }
    }
}
