using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
 
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
 
    public Image characterIconLeft;
    public Image characterIconRight;
    public TextMeshProUGUI characterNameLeft;
    public TextMeshProUGUI characterNameRight;
    public TextMeshProUGUI dialogueArea;

    public MainMenuManager menuManager;
 
    private Queue<DialogueLine> lines;
    
    public bool isDialogueActive = false;
 
    public float typingSpeed = 0.2f;
 
    public Animator animator;
 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
 
        lines = new Queue<DialogueLine>();
    }
 
    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
 
        lines.Clear();
 
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }
 
        DisplayNextDialogueLine();
    }
 
    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }
 
        DialogueLine currentLine = lines.Dequeue();
 
        if(currentLine.character.rightSide)
        {
            characterIconRight.sprite = currentLine.character.icon;
            characterNameRight.text = currentLine.character.name;
        }
        else
        {
            characterIconLeft.sprite = currentLine.character.icon;
            characterNameLeft.text = currentLine.character.name;
        }
        typingSpeed = currentLine.character.textSpeed;
 
        StopAllCoroutines();
 
        StartCoroutine(TypeSentence(currentLine));
    }

    public void OnSubmit()
    {
        DisplayNextDialogueLine();
    }
 
    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
 
    void EndDialogue()
    {
        menuManager.StartGame(2);
    }
}