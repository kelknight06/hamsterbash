
using System;
using System.Collections.Generic;
using UnityEngine;

public class GateSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject leftGatePrefab;
    public GameObject rightGatePrefab;

    [Header("Lane Positions")]
    public float trackCenterX = -253.67f;
    public float leftLaneX = -3f;
    public float rightLaneX = 3f;
    public float gateY = 0f;

    [Header("Spawn Settings")]
    public float spawnDistance = 25f;
    public float pairSpacing = 15f;

    [Header("Gate Limits")]
    public int initialGatePairs = 3;
    public int maxGatePairs = 5;

    

    private float nextSpawnZ;

    private class GatePair
    {
        public GameObject left;
        public GameObject right;
        public float spawnZ;
    }

    private List<GatePair> activePairs =
        new List<GatePair>();

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("GateSpawner: Player reference missing!");
            return;
        }

        if (leftGatePrefab == null || rightGatePrefab == null)
        {
            Debug.LogError("GateSpawner: Gate prefabs missing!");
            return;
        }

        nextSpawnZ = player.position.z + spawnDistance;

        Debug.Log("GateSpawner initialized!");
        Debug.Log("Player Z: " + player.position.z);
        Debug.Log("First spawn Z: " + nextSpawnZ);

        for (int i = 0; i < initialGatePairs; i++)
        {
            SpawnGatePair();
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        if (MathGameManager.Instance == null)
        {
            return;
        }

        if (MathGameManager.Instance.GameFinished)
            return;

        RemoveOldGates();

        if (player.position.z + spawnDistance >= nextSpawnZ)
        {
            if (activePairs.Count < maxGatePairs)
            {
                SpawnGatePair();
            }
        }
    }

    private void SpawnGatePair()
    {
        // Get the Z position where the gates should spawn
        float spawnZ = nextSpawnZ;

        // Get the player's current X position
        float centerX = trackCenterX;

        // Calculate the positions of both gates
        Vector3 leftPosition = new Vector3(
            centerX + leftLaneX,
            gateY,
            spawnZ
        );

        Vector3 rightPosition = new Vector3(
            centerX + rightLaneX,
            gateY,
            spawnZ
        );
        Debug.Log("Left Gate: " + leftPosition);
        Debug.Log("Right Gate: " + rightPosition);

        // Create both gates
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


    // Get MathGate scripts
    MathGate leftScript = left.GetComponent<MathGate>();
        MathGate rightScript = right.GetComponent<MathGate>();

        if (leftScript == null || rightScript == null)
        {
            Debug.LogError("MathGate component missing from a gate prefab!");
            Destroy(left);
            Destroy(right);
            return;
        }

        // Register the gate pair
        GatePair pair = new GatePair
        {
            left = left,
            right = right,
            spawnZ = spawnZ
        };

        activePairs.Add(pair);

        // Generate the question and assign answers
        MathGameManager.Instance.SetupGatePair(leftScript, rightScript);

        Debug.Log("Spawned gate pair at Z: " + spawnZ);

        // Prepare the next spawn position
        nextSpawnZ += pairSpacing;
    }

    private void RemoveOldGates()
    {
        for (int i = activePairs.Count - 1; i >= 0; i--)
        {
            GatePair pair = activePairs[i];

            if (pair.left == null || pair.right == null)
            {
                activePairs.RemoveAt(i);
                continue;
            }

            if (player.position.z > pair.spawnZ + 10f)
            {
                Destroy(pair.left);
                Destroy(pair.right);

                activePairs.RemoveAt(i);
            }
        }
    }
}


