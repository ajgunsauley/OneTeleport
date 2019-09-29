using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndStateController : MonoBehaviour
{
    public Text endgameText;
    public bool levelComplete;
    public bool levelFailed;

    // Start is called before the first frame update
    void Start()
    {
        levelComplete = false;
        levelFailed = false;
    }

    // Update is called once per frame
    void Update()
    {
        //We will need to edit how this works depending on how many levels we have.  Probably an input array
        if (Input.GetButtonDown("Fire1") & levelComplete == true)
        {
            // Clear music for the next level
			InfiniteMusic music = GameObject.FindGameObjectWithTag("Music").GetComponent<InfiniteMusic>();
            if (music) music.PlayNext();

			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (Input.GetButtonDown("Fire1") & levelFailed == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKey(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //enter the gate directly to win!
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.StartsWith("Hero", System.StringComparison.Ordinal))
        {

            other.GetComponent<HeroController>().Win();
            ShowMessage(endgameText, "You win!  Click to continue...");
            levelComplete = true;

        }
        
    }

    //for all the various ways to lose, reference this method
    public void FailLevel()
    {
        levelFailed = true;
        ShowMessage(endgameText, "You lose!  Click to restart...");
    }

    //edit UI endgame message
    public void ShowMessage(Text inputText, string message)
    {
        inputText.text = message;
        inputText.enabled = true;
    }

}
