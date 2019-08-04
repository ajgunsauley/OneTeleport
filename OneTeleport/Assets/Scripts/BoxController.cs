using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    private Transform m_currDrone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "DroneRoof")
        {
            m_currDrone = coll.gameObject.transform;
            transform.SetParent(m_currDrone);
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.name == "DroneRoof")
        {
            m_currDrone = null;
            transform.parent = null;
        }

    }
}
