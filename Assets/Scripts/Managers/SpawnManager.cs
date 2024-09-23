using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private GameObject craterPrefab;
    [SerializeField] private Transform planet;

    [SerializeField] private float spawnAsteroidTime = 1.5f;
    [SerializeField] private float destroyCreaterTime = 1.5f;
    [SerializeField] private int asteroidsAmount = 20;

    private float spawnTimer;
    private int currentPoolIndex = 0;


    // LIST
    private List<Asteroid> asteroidsList = new List<Asteroid>();
    private List<Asteroid> asteroidsPool = new List<Asteroid>();

    private List<GameObject> activeCraters = new List<GameObject>();
    private List<float> craterTimers = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        // SET POOL
        for(int i = 0; i < asteroidsAmount; i++) 
        {
            Asteroid asteroid = Instantiate(asteroidPrefab);
            asteroid.gameObject.SetActive(false);
            asteroidsPool.Add(asteroid);
        }

        spawnTimer = spawnAsteroidTime;
 
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0) 
        {
            SpawnAsteroids();
            spawnTimer = spawnAsteroidTime;
        }

        for (int i = activeCraters.Count - 1; i >= 0; i--) 
        {
            craterTimers[i] -= Time.deltaTime;

            if(craterTimers[i] <= 0) 
            {
                Destroy(activeCraters[i]);
                activeCraters.RemoveAt(i);
                craterTimers.RemoveAt(i);
            }
        }


    }

    private void SpawnAsteroids() 
    {
        
        if (asteroidsList.Count >= asteroidsAmount) return;

        // Spawn an asteroid from the pool
        Asteroid asteroid = GetAsteroidFromPool();

        if(asteroid != null) 
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            asteroid.transform.position = spawnPosition;

            float randomSize = Random.Range(1f, 2f);
            asteroid.transform.localScale = Vector3.one * randomSize;

            asteroid.SetTarget(planet.position);
            asteroid.gameObject.SetActive(true);
            asteroidsList.Add(asteroid);
        }
    }

    private Asteroid GetAsteroidFromPool() 
    {
       
        // Usa un ciclo per trovare l'asteroide inattivo
        for (int i = 0; i < asteroidsPool.Count; i++)
        {
            // Calcola l'indice attuale nel pool
            int index = (currentPoolIndex + i) % asteroidsPool.Count;

            if (!asteroidsPool[index].gameObject.activeInHierarchy)
            {
                // Aggiorna l'indice per il prossimo accesso
                currentPoolIndex = (index + 1) % asteroidsPool.Count;
                return asteroidsPool[index];
            }
        }

        
        return null;
    }

    private Vector3 GetRandomSpawnPosition() 
    {
        
        Vector3 direction = Random.onUnitSphere * 100f;
        return planet.position + direction;
    }

    public void OnAsteroidHit(Asteroid asteroid) 
    {
        Vector3 hitPosition = asteroid.transform.position;

        Quaternion craterRotation = Quaternion.FromToRotation(Vector3.up, (planet.position - hitPosition).normalized);

        
        GameObject creator = Instantiate(craterPrefab, hitPosition, craterRotation);
        activeCraters.Add(creator);
        craterTimers.Add(destroyCreaterTime);          

        asteroid.gameObject.SetActive(false);  
        asteroidsList.Remove(asteroid);
        asteroidsPool.Add(asteroid);
        
    }
}
