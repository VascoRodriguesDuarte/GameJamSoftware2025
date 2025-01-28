using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Class which manages the pause on the affected objects.
/// </summary>
public class PauseController : MonoBehaviour
{
    [SerializeField] private bool pausePhysics;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private List<Behaviour> componentsToPause;
    Dictionary<Behaviour, bool> prevBehaviorState;

    private Rigidbody2D rb2d;
    private Vector2 rb2dPrevVelocity;
    private float rb2dPrevAngularVelocity;
    private RigidbodyType2D rb2dPrevState;

    /// <summary>
    /// Private method called before the first frame.
    /// </summary>
    private void Start()
    {
        PauseManager.Register(Pause);

        prevBehaviorState = new Dictionary<Behaviour, bool>();
        rb2d = GetComponent<Rigidbody2D>();

        if(rb2d == null)
        {
            pausePhysics = false;
        }
    }

    /// <summary>
    /// Private method called when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        PauseManager.Unregister(Pause);
    }

    /// <summary>
    /// Public method which does the Pause logic.
    /// </summary>
    /// <param name="isPaused"> Value which determines if its going to pause
    /// or unpause.</param>
    public void Pause(bool isPaused, bool victory)
    {
        // If the game is getting paused.
        if(isPaused)
        {
            if(!victory)
            {
                pauseMenu.SetActive(true);
            }

            // Keeps the current state of the stopped component and stops them.
            foreach(var component in componentsToPause)
            {
                if(component.enabled)
                {
                    prevBehaviorState.Add(component, component.enabled);
                    component.enabled = false;
                }
            }

            // Also stops the physics if checked.
            if(pausePhysics)
            {
                rb2dPrevState = rb2d.bodyType;
                rb2dPrevVelocity = rb2d.linearVelocity;
                rb2dPrevAngularVelocity = rb2d.angularVelocity;
                rb2d.bodyType = RigidbodyType2D.Static;
            }
        }
        // If the game is already paused.
        else
        {
            if(pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
            }
            else if(optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);
            }
            else if(controlsMenu.activeSelf)
            {
                controlsMenu.SetActive(false);
            }

            // Starts the components again.
            foreach (var component in prevBehaviorState.Keys)
            {
                component.enabled = true;
            }

            prevBehaviorState.Clear();

            // Starts the physics too if they were stopped.
            if(pausePhysics)
            {
                rb2d.bodyType = rb2dPrevState;
                rb2d.linearVelocity = rb2dPrevVelocity;
                rb2d.angularVelocity = rb2dPrevAngularVelocity;
                rb2d.WakeUp();
            }
        }

    }

    /// <summary>
    /// Public method which adds every component to the list to stop the 
    /// components.
    /// </summary>
    [Button("Add all components")]
    public void AddAllComponents()
    {
        componentsToPause = new List<Behaviour>(GetComponents<Behaviour>());
    }
}