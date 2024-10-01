using System;
using UnityEngine;

public class AlienRay : MonoBehaviour
{

    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float offsetDistance = 50.0f; // Distance from the planet for the spawn position
    [SerializeField] private float lifespan = 10f;
    [SerializeField] private Transform planet;
  
    private Transform player;
    private bool hitPlanet = false;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (player != null)
        {
            Vector3 cameraPosition = Camera.main.transform.position; 
            Vector3 directionToPlanet = (planet.position - cameraPosition).normalized;

            Vector3 spawnPosition = cameraPosition + directionToPlanet * 10f;          
            transform.position = spawnPosition;


            transform.forward = (planet.position - transform.position).normalized;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) 
        {
            if (!hitPlanet)
            {

                MoveTowardPlanet();
            }
            else 
            {
                FollowPlayer();
            }


            lifespan -= Time.deltaTime;

            if (lifespan <= 0) 
            {
                gameObject.SetActive(false);
            }
        }

    }


    private void MoveTowardPlanet()
    {

        float step = speed * Time.deltaTime;

        // Controlla se il raggio ha colpito il pianeta
        if (Vector3.Distance(transform.position, planet.position) <= 0.5f) // Modifica il valore in base alla dimensione del pianeta
        {
            hitPlanet = true; // Raggio ha colpito il pianeta
            SnapToSurface(); // Snap alla superficie del pianeta
        }
        else
        {
            // Continua a muoversi verso il pianeta
            Vector3 directionToPlanet = (planet.position - transform.position).normalized;
            transform.position += directionToPlanet * step;
        }

    }

    private void SnapToSurface()
    {
        // Calcola la posizione sulla superficie del pianeta
        Vector3 planetCenter = planet.position;
        Vector3 directionFromCenter = (transform.position - planetCenter).normalized;

        // Calcola la distanza dal centro del pianeta per posizionarsi sulla superficie
        float planetRadius = planet.GetComponent<SphereCollider>().radius * planet.localScale.x; // Modifica se stai usando un MeshCollider
        Vector3 surfacePosition = planetCenter + directionFromCenter * planetRadius;

        transform.position = surfacePosition; // Posiziona il raggio sulla superficie
    }

    private void FollowPlayer()
    {
        // Follow the player after the ray has hit the planet
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        transform.position += directionToPlayer * speed * Time.deltaTime;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Planet"))
    //    {
    //        hitPlanet = true;
    //        Debug.Log("Ray hit the planet!");
    //    }
    //}




}
