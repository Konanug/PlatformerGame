using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{

    [SerializeField] int playerLives=3;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] int score=0;

    private void Awake() {
        int numGameSessions=FindObjectsOfType<GameSession>().Length;
        if(numGameSessions>1){
            Destroy(gameObject);
        }
        else{
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start(){
        livesText.text="Lives Remaining: "+ playerLives.ToString();
        scoreText.text="Bank: "+ score.ToString();
    }

    void Update(){
        if(SceneManager.GetActiveScene().buildIndex==0||SceneManager.GetActiveScene().buildIndex==4||SceneManager.GetActiveScene().buildIndex==5){
            Destroy(gameObject);
        }
    }

    public void AddScore(int pointsToAdd){
        score+=pointsToAdd;
        scoreText.text="Bank: "+ score.ToString();
    }

    public void ProcessPlayerDeath(){
        if(playerLives>1){
            playerLives--;
            int currentSceneIndex=SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
            livesText.text="Lives Remaining: "+ playerLives.ToString();
        }
        else{
            FindObjectOfType<ScenePersist>().ResetScenePersist();
            SceneManager.LoadScene("EndMenu");
            Destroy(gameObject);
        }
    }
}
