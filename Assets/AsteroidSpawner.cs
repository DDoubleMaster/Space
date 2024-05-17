using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // Asteroid Parameters
    [SerializeField] float spawnRadius = 10f;
    [SerializeField] float spawnInterval = 1f;
    [SerializeField] float spawnTimeout = 1f;
    // Asteroid Prefab
    [SerializeField] GameObject asteroidPrefab;

    private void Start()
    {
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

        // Create Asteroid
        Instantiate(asteroidPrefab, position, Quaternion.identity);
    }
}
