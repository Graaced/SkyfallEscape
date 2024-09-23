using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // MOVEMENT
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 3f; // forward rotate speed


    [SerializeField] private float surfaceDistance = 1.5f;  

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

        rb.useGravity = false;
    }

    
    private void Update()
    {

    }

    private void FixedUpdate()
    {       
        AlignWithPlanetSurface();
        Move();
    }


    private void Move()
    {
        Vector2 input = playerController.GetMovement();


        Vector3 movementDirection = (transform.right * input.x) + (transform.forward * input.y);

        // if there is input, rotate the object in the direction it is moving.
        if (movementDirection.magnitude > 0.1f) // avoid rotating if there is no input
        {

            Quaternion targetRotation = Quaternion.LookRotation(movementDirection.normalized, transform.up);


            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }


        Vector3 forwardMovement = transform.forward * speed * Time.fixedDeltaTime;


        rb.MovePosition(rb.position + forwardMovement);
    }


    private void AlignWithPlanetSurface()
    {
        Vector3 directionToPlanet = (transform.position - planet.position).normalized;

        // Raycast to find the surface of the planet
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -directionToPlanet, out hit))
        {
            // Set the player's position slightly above the surface
            Vector3 targetPosition = hit.point + directionToPlanet * surfaceDistance;
            rb.MovePosition(targetPosition);

            // Align the car's up direction with the surface normal
            Vector3 surfaceNormal = hit.normal;

            // Adjust the car's forward direction to stay parallel with the surface
            Vector3 forwardAlongSurface = Vector3.Cross(transform.right, surfaceNormal).normalized;

            // Create a rotation that aligns the car's up direction with the surface normal
            // and keeps the car's forward direction parallel to the ground
            Quaternion targetRotation = Quaternion.LookRotation(forwardAlongSurface, surfaceNormal);

            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
