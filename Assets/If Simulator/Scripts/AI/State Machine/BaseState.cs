using UnityEngine;

/// <summary>
/// The base class for all states. Use OnEnable and OnDisable instead of Start and OnDestroy.
/// </summary>
public abstract class BaseState : MonoBehaviour
{
    protected StateMachine Manager { get; private set; }
    protected object[] Args { get; private set; }

    private void Awake()
    {
        enabled = false; // Disable the state by default.
    }

    public void Enter(StateMachine manager, params object[] args)
    {
        Manager = manager;
        Args = args;
        enabled = true;
    }

    public void Exit()
    {
        enabled = false;
    }
}