using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State {
    public virtual void OnEnable() { }
    public virtual void OnStart() { }
    public virtual void OnPause() { }
    public virtual void OnDisable() { }

    public virtual void FixedUpdate() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }

    public virtual void OnCollisionEnter2D(Collision2D collision) { }
    public virtual void OnCollisionExit2D(Collision2D collision) { }

    public virtual void OnTriggerEnter2D(Collider2D other) { }
    public virtual void OnTriggerExit2D(Collider2D other) { }
}

public class StateManager : MonoBehaviour {
    private LinkedList<State> states_;

    private class DefaultState : State { }
    private static readonly DefaultState defaultState_ = new DefaultState();

    public StateManager() {
        states_ = new LinkedList<State>();
    }

    public State CurrentState() {
        return states_.Count > 0 ? states_.First.Value : defaultState_;
    }

    public void Push(State state) {
        Debug.Assert(state != null);

        // Pause current State
        CurrentState().OnPause();

        // Add and enable new State
        states_.AddFirst(state);
        state.OnEnable();
        state.OnStart();
    }

    public State Pop() {
        // Disable current State
        State current = CurrentState();
        current.OnPause();
        current.OnDisable();

        states_.RemoveFirst();

        // Restart next State
        CurrentState().OnStart();

        return current;
    }

    public State Swap(State state) {
        Debug.Assert(state != null);

        // Remove current State
        State current = CurrentState();
        current.OnPause();
        current.OnDisable();

        states_.RemoveFirst();

        // Add and enable new State
        states_.AddFirst(state);
        state.OnEnable();
        state.OnStart();

        // Debug.Log(current.GetType().ToString() + " => " + state.GetType().ToString());

        return current;
    }

    public void FixedUpdate() { CurrentState().FixedUpdate(); }
    public void Update() { CurrentState().Update(); }
    public void LateUpdate() { CurrentState().LateUpdate(); }

    public void OnCollisionEnter2D(Collision2D collision) {
        CurrentState().OnCollisionEnter2D(collision);
    }
    public void OnCollisionExit2D(Collision2D collision) {
        CurrentState().OnCollisionExit2D(collision);
    }

    public void OnTriggerEnter2D(Collider2D other) {
        CurrentState().OnTriggerEnter2D(other);
    }
    public void OnTriggerExit2D(Collider2D other) {
        CurrentState().OnTriggerExit2D(other);
    }
}