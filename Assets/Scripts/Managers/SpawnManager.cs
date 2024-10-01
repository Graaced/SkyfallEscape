using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //PLANET
    [SerializeField] private Transform planet;

    // ASTEROID
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private GameObject craterPrefab;
    [SerializeField] private float spawnAsteroidTime = 1.5f;
    [SerializeField] private float destroyCreaterTime = 1.5f;
    [SerializeField] private int asteroidsAmount = 20;

    private float spawnTimer;
    private int currentPoolIndex = 0;
    private bool isActive;


    //ALIEN RAY
    [SerializeField] private AlienRay rayPrefab;   
    [SerializeField] private float spawnRayTime = 2f;

    private float spawnRayTimer;


    // SCORE
    [SerializeField] private ScoreManager scoreManager;

 
    // LIST
    private List<Asteroid> asteroidsList = new List<Asteroid>();
    private List<Asteroid> asteroidsPool = new List<Asteroid>();

    private List<GameObject> activeCraters = new List<GameObject>();
    private List<float> craterTimers = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        // SET POOL
        for (int i = 0; i < asteroidsAmount; i++)
        {
            Asteroid asteroid = Instantiate(asteroidPrefab);
            asteroid.gameObject.SetActive(false);
            asteroidsPool.Add(asteroid);
        }

        spawnTimer = spawnAsteroidTime;


        // ALIEN RAY
        //spawnRayTimer = spawnRayTime;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.isPlayerDead) 
        {
            return; 
        }

        // ASTEROIDS
        spawnTimer -= Time.deltaTime;
        
        if (spawnTimer <= 0)
        {
            SpawnAsteroids();
            spawnTimer = spawnAsteroidTime;
        }

        if (scoreManager.isShrinking)
        {
            DestroyCraters();
        }
        else 
        {
            for (int i = activeCraters.Count - 1; i >= 0; i--)
            {
                craterTimers[i] -= Time.deltaTime;

                if (craterTimers[i] <= 0)
                {
                    Destroy(activeCraters[i]);
                    activeCraters.RemoveAt(i);
                    craterTimers.RemoveAt(i);
                }
            }
        }

        // ALIEN RAY
        //spawnRayTimer -= Time.deltaTime;

        //if (spawnRayTimer <= 0)
        //{
        //    SpawnAlienRay();
        //    spawnRayTimer = spawnRayTime;
        //}


    }

    private void SpawnAsteroids()
    {
        if (scoreManager.isShrinking) 
        {
            return;
        }
        
        if (asteroidsList.Count >= asteroidsAmount) return;

       // Spawn an asteroid from the pool
       Asteroid asteroid = GetAsteroidFromPool();

       if (asteroid != null)
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
        // cycle to find the inactive asteroid
        for (int i = 0; i < asteroidsPool.Count; i++)
        {
            
            if (!asteroidsPool[currentPoolIndex].isActive)
            {
                //Update index for next access
                Asteroid asteroid = asteroidsPool[currentPoolIndex];
                currentPoolIndex = (currentPoolIndex + 1) % asteroidsPool.Count;
                return asteroid;
            }
            currentPoolIndex = (currentPoolIndex + 1) % asteroidsPool.Count;
        }

        return null;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float spawnDistance = 100f;
        Vector3 direction = Random.onUnitSphere * spawnDistance;
        return planet.position + direction;
    }


    public void OnAsteroidHit(Asteroid asteroid, Collision collision)
    {
        // Position of the asteroid at the time of impact
        Vector3 hitPosition = asteroid.transform.position;

        // I use GetContact to get the surface normal
        Vector3 surfaceNormal = collision.GetContact(0).normal; // I need to get the first contact normal

        // I use a raycast to get the exact point on the surface
        RaycastHit hit;
        if (Physics.Raycast(hitPosition, -surfaceNormal, out hit))
        {
            // Impact point on the surface
            Vector3 craterPosition = hit.point; 

            Quaternion craterRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            GameObject crater = Instantiate(craterPrefab, craterPosition, craterRotation);
            activeCraters.Add(crater);
            craterTimers.Add(destroyCreaterTime);

            // diable asteroid
            asteroid.gameObject.SetActive(false);
            asteroid.isActive = false;
            asteroidsList.Remove(asteroid);
            asteroidsPool.Add(asteroid);
        }

    }

    private void SpawnAlienRay() 
    {
        if (rayPrefab != null) 
        {
            AlienRay alienRay = Instantiate(rayPrefab);
        }

    }


    private void DestroyCraters()
    {
        for (int i = activeCraters.Count - 1; i >= 0; i--)
        {
            Destroy(activeCraters[i]);
        }       
        activeCraters.Clear();
        craterTimers.Clear();
    }


   


}
