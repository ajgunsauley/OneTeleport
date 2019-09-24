using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
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
        if (!other.name.StartsWith("Icicle", System.StringComparison.Ordinal))
        {
            Destroy(other.gameObject);
        }
        if (other.name.StartsWith("Icicle", System.StringComparison.Ordinal))
        {
            IcicleController ic = other.GetComponent<IcicleController>();
            if (ic.IsFalling())
                ic.Break();
        }
        if (other.name.StartsWith("Hero", System.StringComparison.Ordinal))
        {
            other.GetComponent<HeroController>().Die(true);
            endStateController.FailLevel();
        }

    }

}
