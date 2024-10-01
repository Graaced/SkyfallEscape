using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isPlayerDead { get; private set; } // state of the player

    // Start is called before the first frame update
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerDied()
    {
        isPlayerDead = true;
       
    }

    public void ResetGame()
    {
        isPlayerDead = false;
        
    }
}
