using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IcicleController : MonoBehaviour
{
    //public UIController UI;
    public EndStateController endStateController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Hero")
        {
            Destroy(other.gameObject);
            endStateController.FailLevel();
        }
        Destroy(gameObject);

    }
}
