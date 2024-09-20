using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private Transform player;   
    [SerializeField] private Transform planet;  
    [SerializeField] private Vector3 offset;     
    [SerializeField] private Vector3 rotationOffset; 
    [SerializeField] private float followSpeed = 0.1f; 
    [SerializeField] private float rotationSpeed = 5f;  


    private Vector3 velocity = Vector3.zero;  

    private float radius;  // Distance between the center of the planet and the camera

    void Start()
    {
        if (planet == null)
        {
            
            GameObject planetObject = GameObject.FindGameObjectWithTag("Planet");
            if (planetObject != null)
            {
                planet = planetObject.transform;
            }
        }

        // Calculate the radius (distance) from the camera to the center of the planet
        radius = Vector3.Distance(transform.position, planet.position);
    }

    void LateUpdate()
    {
        FollowPlayer();      
        AlignCameraWithPlayer();
    }

    

    private void FollowPlayer()
    {
        
        Vector3 targetPosition = player.position + player.TransformDirection(offset);

        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSpeed);
    }

    
    private void AlignCameraWithPlayer()
    {
        // Function to rotate around the player to give a dynamic effect

        Quaternion targetRotation = Quaternion.LookRotation(player.forward, player.up) * Quaternion.Euler(rotationOffset);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }







}
