using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IcicleController : MonoBehaviour, ISwapResponder {
    //public UIController UI;
    public LayerMask rayFallingMask;
    public float gigglingTime = .2f;

    public GameObject destroyFX;

    [HideInInspector]
    public Collider2D droneCollider;

    private Rigidbody2D rbody;
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

        public override void OnDisable() {
            ic_.droneCollider.enabled = false;
        }

        override public void Update() {
            RaycastHit2D hit = Physics2D.Raycast(ic_.transform.position, Vector2.down, 100f, ic_.rayFallingMask);
            if (hit && hit.transform.name.StartsWith("Hero", System.StringComparison.Ordinal))
                ic_.stateManager_.Swap(new StateGiggling(ic_));
        }

        public override void OnCollisionEnter2D(Collision2D collision) {
            string name = collision.collider.name;
            if (name.StartsWith( "Drone", System.StringComparison.Ordinal) || name.StartsWith( "Crate", System.StringComparison.Ordinal))
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
            if (name.StartsWith( "Drone", System.StringComparison.Ordinal) || name.StartsWith( "Crate", System.StringComparison.Ordinal))
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
            if (other.name.StartsWith("Hero", System.StringComparison.Ordinal)) {
                other.GetComponent<HeroController>().Die(false);
            }
            // Hitting a crate/drone
            else if (other.name.StartsWith("Crate", System.StringComparison.Ordinal) || other.name.StartsWith("Drone", System.StringComparison.Ordinal))
                Destroy(other.gameObject);
            else if (other.name.StartsWith("Lava", System.StringComparison.Ordinal))
                ic_.Break(BreakCause.Melt);
            // We get rekt
            else
                ic_.Break();
        }
    }

    private class StateSwapped : IcicleState {
        public StateSwapped(IcicleController ic) : base(ic) { }

        private float breakDroneTimer;

        public override void OnStart() {
            breakDroneTimer = Time.time + .2f;
            ic_.rbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }

        public override void FixedUpdate() {
            if (ic_.rbody.velocity.y < -1f)
                ic_.stateManager_.Swap(new StateFalling(ic_));
        }

        public override void OnCollisionEnter2D(Collision2D collision) {
            Collider2D other = collision.collider;
            if (other.name.StartsWith( "Drone", System.StringComparison.Ordinal)) {
                ic_.Break();
                if (Time.time < breakDroneTimer)
                    Destroy(other.gameObject);
            } else if (other.name.StartsWith( "Crate", System.StringComparison.Ordinal)) {
                float icicleY = ic_.rbody.position.y;
                float crateY = other.attachedRigidbody.position.y;

                // Only destroy the crate if the icicle is above it!
                if (icicleY > crateY)
                    Destroy(other.gameObject);
            } else if (other.name.StartsWith( "Icicle", System.StringComparison.Ordinal)) {
                ic_.Break();
                other.GetComponent<IcicleController>().Break();
            }
        }

        public override void OnTriggerEnter2D(Collider2D other) {
            if (other.name.StartsWith( "Button", System.StringComparison.Ordinal))
                ic_.Break();
        }
    }

    private class StateBreak : IcicleState {
        private BreakCause cause_ = BreakCause.Break;
        public StateBreak(IcicleController ic, BreakCause cause = BreakCause.Break) : base(ic) {
            cause_ = cause;
        }

        public override void Swapped(GameObject hero) { }
        public override void OnCollisionEnter2D(Collision2D collision) { }

        public override void OnStart() {
            Instantiate(ic_.destroyFX, ic_.transform.position, Quaternion.identity)
                .GetComponent<IcicleDestroy>()
                .Play(cause_);

            Destroy(ic_.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        stateManager_ = gameObject.AddComponent<StateManager>();
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

    public void Break(BreakCause cause = BreakCause.Break) {
        stateManager_.Swap(new StateBreak(this, cause));
    }
}
