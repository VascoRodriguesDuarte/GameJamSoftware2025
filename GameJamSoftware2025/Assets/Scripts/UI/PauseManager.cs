using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public delegate void PauseFunction(bool isPaused);
    private static PauseManager instance;
    private bool pause;
    private event PauseFunction Pause;

    /// <summary>
    /// Private method called when the GameObject is initialized.
    /// </summary>
    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    /// <summary>
    /// Public method which activates when the pause starts.
    /// </summary>
    public void OnPause()
    {
        //Pause
        pause = !pause;

        if (Pause != null)
        {
            Pause(pause);
        }
    }

    /// <summary>
    /// Public method calling the _Registered method when called
    /// </summary>
    /// <param name="pauseDelegate">Value containing the event of the 
    /// registering object</param>
    public static void Register(PauseFunction pauseDelegate)
    {
        instance._Register(pauseDelegate);
    }

    /// <summary>
    /// Private method which registers an object to the Pause Manager
    /// </summary>
    /// <param name="pauseDelegate">Value containing the event of the 
    /// registering object</param>
    private void _Register(PauseFunction pauseDelegate)
    {
        Pause += pauseDelegate;
    }

    /// <summary>
    /// Public method calling the _Unregistered method when called
    /// </summary>
    /// <param name="pauseDelegate">Value containing the event of the 
    /// unregistering object</param>
    public static void Unregister(PauseFunction pauseDelegate)
    {
        instance._Unregister(pauseDelegate);
    }

    /// <summary>
    /// Private method which unregisters an object to the Pause Manager
    /// </summary>
    /// <param name="pauseDelegate">Value containing the event of the 
    /// unregistering object</param>
    private void _Unregister(PauseFunction pauseDelegate)
    {
        Pause -= pauseDelegate;
    }
}
