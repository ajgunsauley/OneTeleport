using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

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
        //if icicle is not moving, drone kills it and then flips direction
        if (other.name == "Icicle")
        {
            var icicleSpeed = other.GetComponent<Rigidbody2D>().velocity;
            if (icicleSpeed.y == 0)
            {
                Destroy(other.gameObject);
                RotateDrone();
            }
        }

    }

    //nothing lives here yet
    void RotateDrone()
    {

    }
}
