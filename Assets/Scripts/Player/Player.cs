using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // MOVEMENT
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 3f; // forward rotate speed
    [SerializeField] private float gravityStrength = 9.8f;
    
    private float radius; // Raggio del movimento circolare

    // PLANET
    [SerializeField] private Transform planet;


    // CONTROLLER
    private PlayerController playerController;

    // RIGIDBODY
    private Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        // RB
        rb = GetComponent<Rigidbody>();

        // CONTROLLER
        playerController = GetComponent<PlayerController>();

        // PLANET
        GameObject planetObject = GameObject.FindGameObjectWithTag("Planet");
        if (planetObject != null)
        {
            planet = planetObject.transform;

        }
   
        radius = Vector3.Distance(transform.position, planet.position);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        Move();
    }


    private void Move()
    {      
        Vector2 input = playerController.GetMovement();

        
        Vector3 movementDirection = (transform.right * input.x) + (transform.forward * input.y);

        // Se c'è input, ruota l'oggetto nella direzione in cui si sta muovendo
        if (movementDirection.magnitude > 0.1f) // Evita di ruotare se non c'è input
        {
            
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection.normalized, transform.up);

            
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        
        Vector3 forwardMovement = transform.forward * speed * Time.fixedDeltaTime;

        
        rb.MovePosition(rb.position + forwardMovement);
    }

    private void ApplyGravity()
    {

        Vector3 gravityDirection = (planet.position - transform.position).normalized;

        // Simulates "gravity" by pushing the car toward the center of the planet
        Vector3 gravityForce = gravityDirection * gravityStrength;


        rb.AddForce(gravityForce, ForceMode.Acceleration);

        // Align the machine so that it always "points" perpendicular to the surface
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

}
