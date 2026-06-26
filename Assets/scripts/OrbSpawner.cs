using System.Collections.Generic;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    public static OrbSpawner Instance { get; private set; }

    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();
    [SerializeField] private int maxCanSpawn = 200;
    [SerializeField] private int shotsPerAdvance = 6;
    [SerializeField] private float rowHeight = 1.5f;
    [SerializeField] private int rowLength = 8;
    [SerializeField] private int initialRows = 5;
    [SerializeField] private float startY = 4f;
    [SerializeField] private float startX = -8f;
    [SerializeField] private float xSpacing = 2f;

    private int currentlySpawned = 0;
    private int shotsSinceLastAdvance = 0;
    private readonly List<GameObject> activeOrbs = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SpawnInitialRows();
    }

    public void HandleShotFired()
    {
        shotsSinceLastAdvance++;

        if (shotsSinceLastAdvance >= shotsPerAdvance)
        {
            AdvanceOrbsDown();
            SpawnTopRow();
            shotsSinceLastAdvance = 0;
        }
    }

    private void SpawnInitialRows()
    {
        for (int row = 0; row < initialRows; row++)
        {
            SpawnRowAtY(startY - row * rowHeight);
        }
    }

    private void SpawnTopRow()
    {
        SpawnRowAtY(startY);
    }

    private void SpawnRowAtY(float y)
    {
        if (prefabs == null || prefabs.Count == 0 || currentlySpawned >= maxCanSpawn)
        {
            return;
        }

        float rowStartX = startX - ((rowLength - 1) * xSpacing) * 0.5f;

        for (int i = 0; i < rowLength; i++)
        {
            if (currentlySpawned >= maxCanSpawn)
            {
                break;
            }

            Vector3 spawnPos = new Vector3(rowStartX + i * xSpacing, y, 0f);

            if (!SpotIsFree(spawnPos))
            {
                continue;
            }

            int randomIndex = Random.Range(0, prefabs.Count);
            GameObject orbPrefab = prefabs[randomIndex];

            if (orbPrefab == null)
            {
                continue;
            }

            GameObject orb = Instantiate(orbPrefab, spawnPos, Quaternion.identity);
            activeOrbs.Add(orb);
            currentlySpawned++;
        }
    }

    private void AdvanceOrbsDown()
    {
        for (int i = activeOrbs.Count - 1; i >= 0; i--)
        {
            if (activeOrbs[i] == null)
            {
                activeOrbs.RemoveAt(i);
                continue;
            }

            activeOrbs[i].transform.position += Vector3.down * rowHeight;
        }
    }

    private bool SpotIsFree(Vector3 pos)
    {
        float radius = 0.5f;
        Collider2D hit = Physics2D.OverlapCircle(pos, radius);
        return hit == null;
    }
}