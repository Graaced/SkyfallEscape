using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // TRANSFORM
    [SerializeField] private Transform planet;
    [SerializeField] private float scaleDecreaseAmount = 10f;
    [SerializeField] private float minScale = 20f;
    [SerializeField] private float shrinkDuration = 2f;  //how long does the shrinking of the planet last 
    private Vector3 targetScale;
   
    public bool isShrinking = false;
    private float shrinkTimer = 0f;

    // UI 
    [SerializeField] private TextMeshProUGUI timerText;  // Text UI component reference

    // TIMERS
    private float scoreTime = 10f;
    private float scoreTimer;

    private bool isPlayerDead = false;

    // Start is called before the first frame update
    void Start()
    {
        scoreTimer = scoreTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShrinking && !isPlayerDead) 
        {
            scoreTimer -= Time.deltaTime;

            UpdateTimerUI(scoreTimer);// update canva timer
            //timerText.text = "Timer: " + Mathf.Floor(scoreTimer).ToString();

            if (scoreTimer < 0)
            {               
                //scoreTimer = scoreTime;
                ShrinkPlanet();
            }
        }

        // Update the scale gradually if the planet is shrinking
        if (isShrinking)
        {
            planet.localScale = Vector3.Lerp(planet.localScale, targetScale, Time.deltaTime);

            // scale has been reached, stop reduction
            if (Vector3.Distance(planet.localScale, targetScale) < 0.01f)
            {
                planet.localScale = targetScale;
                isShrinking = false;
                scoreTimer = scoreTime;
            }
        }


    }


    
    private void ShrinkPlanet()
    {
        if (planet != null)
        {
            
            float newScale = Mathf.Max(planet.localScale.x - scaleDecreaseAmount, minScale);

            // Apply the new scale only if it is not already at minimum
            targetScale = new Vector3(newScale, newScale, newScale);
            isShrinking = true;
        }
    }

    private void UpdateTimerUI(float time) 
    {
        if(timerText != null) 
        {
            timerText.text = " D = " + time.ToString("F2");
        }
    }


    public void PlayerDied()  
    {
        isPlayerDead = true;
    }
}
