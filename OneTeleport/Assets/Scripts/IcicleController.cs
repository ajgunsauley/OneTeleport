using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IcicleController : MonoBehaviour
{
    //public UIController UI;
    public EndStateController endStateController;
    public LayerMask rayFallingMask;
    public float gigglingTime = .2f;
    public float fallingGravity = 4f;

    private Rigidbody2D rbody;
    private bool isFalling;
    private float fallingTimer;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        isFalling = false;
        fallingTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling == false) {
            if (fallingTimer == 0f) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position + 1f * Vector3.down, Vector2.down, 100f, rayFallingMask);
                if (hit && hit.transform.name == "Hero")
                        fallingTimer = Time.time + gigglingTime;
            } else if (Time.time >= fallingTimer) {
                isFalling = true;
                rbody.gravityScale = fallingGravity;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Hero")
        {
            Destroy(other.gameObject);
            endStateController.FailLevel();
        }

        if (other.name == "Crate" || other.name == "Drone")
            Destroy(other.gameObject);

        if (rbody.velocity.y != 0f)
        {
            Destroy(gameObject);
        }
    }
}
