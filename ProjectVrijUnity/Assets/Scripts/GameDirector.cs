using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [Header("CRITTER CREATION")]
    public int critterSpawnAmount;
    public GameObject critterPrefab;
    void Start()
    {
        SummonCritters();
    }

    public void SummonCritters()
    {
        for (int c = 0; c < critterSpawnAmount; c++)
        {
            float spawnX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
            float spawnY = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);

            Vector2 spawnPosition = new Vector2(spawnX, spawnY);
            Instantiate(critterPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
