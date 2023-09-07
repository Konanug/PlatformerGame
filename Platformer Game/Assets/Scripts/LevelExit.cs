using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{

    [SerializeField] float loadDelay=1f;
    [SerializeField] AudioClip levelExitSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator OnTriggerEnter2D(Collider2D other){
        if(other.tag=="Player"){
            AudioSource.PlayClipAtPoint(levelExitSFX,gameObject.transform.position,10f);
            yield return new WaitForSecondsRealtime(loadDelay);
            int currentSceneIndex=SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex=currentSceneIndex+1;
            if(SceneManager.GetActiveScene().buildIndex==3){
                nextSceneIndex=5;
            }
            FindObjectOfType<ScenePersist>().ResetScenePersist();
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
