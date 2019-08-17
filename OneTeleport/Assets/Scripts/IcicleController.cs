﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IcicleController : MonoBehaviour, ISwapResponder {
    //public UIController UI;
    public EndStateController endStateController;
    public LayerMask rayFallingMask;
    public float gigglingTime = .2f;

    private Rigidbody2D rbody;
    private float fallingTimer;
    private bool wasSwapped;
    private bool isFalling;

    private AudioSource breakSound;
    private Animator animator;

    private StateManager stateManager_;

    private class IcicleState : State, ISwapResponder {
        protected IcicleController ic_;
        public IcicleState(IcicleController ic) { ic_ = ic; }

        public virtual void Swapped(GameObject hero) {
            Vector2 hv = hero.GetComponent<Rigidbody2D>().velocity;
            if (hv.y < -0.01f)
                ic_.stateManager_.Swap(new StateFalling(ic_));
            else
                ic_.stateManager_.Swap(new StateSwapped(ic_));
        }
    }

    private class StateDetect : IcicleState {
        public StateDetect(IcicleController ic) : base(ic) { }

        override public void OnStart() {
            ic_.rbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        override public void Update() {
            RaycastHit2D hit = Physics2D.Raycast(ic_.transform.position, Vector2.down, 100f, ic_.rayFallingMask);
            if (hit && hit.transform.name == "Hero")
                ic_.stateManager_.Swap(new StateGiggling(ic_));
        }

        public override void OnCollisionEnter2D(Collision2D collision) {
            string name = collision.collider.name;
            if (name == "Drone" || name == "Crate")
                ic_.Break();
        }
    }

    private class StateGiggling : IcicleState {
        public StateGiggling(IcicleController ic) : base(ic) { }

        private float gigglingTimeOut;
        override public void OnStart() {
            gigglingTimeOut = Time.time + ic_.gigglingTime;
            ic_.animator.SetBool("IsGiggling", true);
        }

        public override void OnPause() {
            ic_.animator.SetBool("IsGiggling", false);
        }

        public override void Update() {
            if (Time.time > gigglingTimeOut)
                ic_.stateManager_.Swap(new StateFalling(ic_));
        }

        public override void OnCollisionEnter2D(Collision2D collision) {
            string name = collision.collider.name;
            if (name == "Drone" || name == "Crate")
                ic_.Break();
        }
    }

    private class StateFalling : IcicleState {
        public StateFalling(IcicleController ic) : base(ic) { }

        public override void Swapped(GameObject hero) {
            // We just keep falling!
            ic_.rbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }

        public override void OnStart() {
            ic_.rbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }

        public override void OnCollisionEnter2D(Collision2D collision) {
            Collider2D other = collision.collider;
            // Hitting poor chunky boy
            if (other.name == "Hero") {
                other.GetComponent<HeroController>().Die(false);
                ic_.endStateController.FailLevel();
            }
            // Hitting a crate/drone
            else if (other.name == "Crate" || other.name == "Drone")
                Destroy(other.gameObject);
            // We get rekt
            else
                ic_.Break();
        }
    }

    private class StateSwapped : IcicleState {
        public StateSwapped(IcicleController ic) : base(ic) { }

        public override void OnStart() {
            ic_.rbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }

        public override void FixedUpdate() {
            if (ic_.rbody.velocity.y < -1f)
                ic_.stateManager_.Swap(new StateFalling(ic_));
        }

        public override void OnCollisionEnter2D(Collision2D collision) {
            Collider2D other = collision.collider;
            if (other.name == "Drone")
                ic_.Break();
            else if (other.name == "Crate") {
                float icicleY = ic_.rbody.position.y;
                float crateY = other.attachedRigidbody.position.y;

                // Only destroy the crate if the icicle is above it!
                if (icicleY > crateY)
                    Destroy(other.gameObject);
            } else if (other.name == "Icicle") {
                ic_.Break();
                other.GetComponent<IcicleController>().Break();
            }
        }

        public override void OnTriggerEnter2D(Collider2D other) {
            if (other.name == "Button")
                ic_.Break();
        }
    }

    private class StateBreak : IcicleState {
        public StateBreak(IcicleController ic) : base(ic) { }

        public override void Swapped(GameObject hero) { }
        public override void OnCollisionEnter2D(Collision2D collision) { }

        public override void OnStart() {
            ic_.breakSound.Play();

            // Disable components that affect the game
            ic_.GetComponentInChildren<SpriteRenderer>().enabled = false;
            foreach (var col in ic_.GetComponentsInChildren<Collider2D>())
                col.enabled = false;

            Destroy(ic_.gameObject, 1f);
        }
    }

    // Start is called before the first frame update
    void Start() {
        rbody = GetComponent<Rigidbody2D>();
        breakSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        stateManager_ = new StateManager();
        stateManager_.Push(new StateDetect(this));
    }

    // Update is called once per frame
    void Update() {
        stateManager_.Update();
    }

    private void FixedUpdate() {
        stateManager_.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        stateManager_.OnCollisionEnter2D(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        stateManager_.OnTriggerEnter2D(collision);
    }

    public void Swapped(GameObject hero) {
        if (stateManager_.CurrentState() is IcicleState state)
            state.Swapped(hero);
    }

    public bool IsFalling() {
        return (stateManager_.CurrentState() as StateFalling) != null;
    }

    public void Fall() {
        stateManager_.Swap(new StateFalling(this));
    }

    public void Break() {
        stateManager_.Swap(new StateBreak(this));
    }
}
