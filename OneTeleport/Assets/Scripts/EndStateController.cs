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
    public bool playNextTrack = true;
    public bool autoNextLevel = false;

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
            NextLevel();
        }

        if (Input.GetButtonDown("Fire1") & levelFailed == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKey(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void NextLevel() {
        // Clear music for the next level
        InfiniteMusic music = GameObject.FindGameObjectWithTag("Music").GetComponent<InfiniteMusic>();
        if (playNextTrack && music) music.PlayNext();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //enter the gate directly to win!
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.StartsWith("Hero", System.StringComparison.Ordinal))
        {
            PlayerPrefs.SetString("UnlockedLevel", SceneManager.GetActiveScene().name);
            if (autoNextLevel) {
                NextLevel();
            } else {
                HeroController hc = other.GetComponent<HeroController>();
                if (hc != null) hc.Win();
                ShowMessage(endgameText, "Level complete!  Click to continue.... \nУровень пройден! Нажмите для продолжения....");
                levelComplete = true;
            }
        }
        
    }

    //for all the various ways to lose, reference this method
    public void FailLevel()
    {
        levelFailed = true;
        ShowMessage(endgameText, "You lose!  Click to restart.... \nТы проиграл! Нажмите, чтобы перезагрузить ....");
    }

    //edit UI endgame message
    public void ShowMessage(Text inputText, string message)
    {
        inputText.text = message;
        inputText.enabled = true;
    }

}
