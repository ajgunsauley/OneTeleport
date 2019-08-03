using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndStateController : MonoBehaviour
{
    public Text endgameText;
    public bool levelComplete;

    // Start is called before the first frame update
    void Start()
    {
        levelComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        //We will need to edit how this works depending on how many levels we have.  Probably an input array
        if (Input.GetMouseButton(0) & levelComplete == true)
        {
            Application.LoadLevel("EndLevel");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Hero")
        {

            Destroy(other.gameObject);
            ShowMessage("You win!  Press Enter to continue...");

        }
        
    }

    void ShowMessage(string message)
    {
        endgameText.text = message;
        endgameText.enabled = true;
        levelComplete = true;
    }

}
