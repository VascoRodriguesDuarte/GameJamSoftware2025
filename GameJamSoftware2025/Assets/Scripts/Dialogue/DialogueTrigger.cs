using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
    public bool rightSide;
    public float textSpeed;
}
 
[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}
 
[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
 
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public float delayBeforeStart = 1.5f;
    [SerializeField] private PlayerInput playerInput;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    private void Start()
    {
        playerInput.SwitchCurrentActionMap("UI");
        StartCoroutine(DelayedTrigger());
    }

    private IEnumerator DelayedTrigger()
    {
        yield return new WaitForSeconds(delayBeforeStart);
        TriggerDialogue();
    }
}