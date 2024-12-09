using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // Asteroid Parameters
    [SerializeField] float spawnRadius = 10f;
    [SerializeField] float spawnInterval = 1f;
    [SerializeField] float spawnTimeout = 1f;
    // Asteroid Prefab
    [SerializeField] Asteroid[] asteroidsPrefab;
    [SerializeField] Dictionary<int, Asteroid> asteroids = new();

    private void Start()
    {
        TaskManager taskManager = GameObject.Find("TaskManager").GetComponent<TaskManager>();
        foreach(Asteroid asteroid in asteroidsPrefab)
        {
            asteroid.taskManager = taskManager;
            asteroids.Add(asteroid.chance, asteroid);
        }

        // Call to SpawnAsteroid method after SpawnTimeout every SpawnInterval
        InvokeRepeating("SpawnAsteroid", time: spawnTimeout, repeatRate: spawnInterval);
    }

	private void Update()
	{
        Vector3 pos = transform.position / 100;
        Vector3 noise;
        noise.x = Mathf.PerlinNoise(pos.x, pos.y);
		noise.y = Mathf.PerlinNoise(pos.y, pos.z);
		noise.z = Mathf.PerlinNoise(pos.z, pos.x);

        if (noise.magnitude > 1 && IsInvoking("SpawnAsteroid"))
            CancelInvoke("SpawnAsteroid");
        else if (noise.magnitude < 1 && !IsInvoking("SpawnAsteroid"))
			InvokeRepeating("SpawnAsteroid", time: spawnTimeout, repeatRate: spawnInterval);
	}

	private void SpawnAsteroid()
    {
        // Generate random position
        Vector2 point = Random.insideUnitSphere * spawnRadius;
        Vector3 position = transform.position + transform.forward * spawnRadius + new Vector3(point.x, 0f, point.y);


        int randomChance = Random.Range(0, 100);
        int currentChance = 0;
        foreach (int chance in asteroids.Keys)
        {
            currentChance += chance;
            if (randomChance < chance)
                Instantiate(asteroids[chance], position, Quaternion.identity);
        }
    }
}
