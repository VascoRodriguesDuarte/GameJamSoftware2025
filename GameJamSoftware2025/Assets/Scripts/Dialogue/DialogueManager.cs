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
    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private CanvasGroup image;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private float transitionTimeSpeed;

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
        fadeCanvas.SetActive(true);
        StartCoroutine(TitleDrop());
    }

    private IEnumerator TitleDrop()
    {
        // While the image is not fully visible, then it fades in.
        while(image.alpha != 1f)
        {
            image.alpha += transitionSpeed;
            AudioListener.volume -= transitionSpeed;

            if(image.alpha > 1f)
            {
                image.alpha = 1f;
            }
            if(AudioListener.volume < 0f)
            {
                AudioListener.volume = 0f;
            }

            yield return new WaitForSeconds(transitionTimeSpeed);
        }

        yield return new WaitForSeconds(3f);

        menuManager.StartGame(2);
    }
}