using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TalkManager : MonoBehaviour
{
    [Header("Path")]
    public PathTRIGGER pathTrig;

    [Header("Ink & UI")]
    public TextAsset inkFile;
    public TextMeshProUGUI textBox;
    public InputActionReference Accept;
    public UnityEngine.UI.Button[] choiceButtons;
    public UnityEngine.UI.Image panel;

    [Header("References")]
    public PlayerMovement player;

    private Story _story;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sideOcClip;
    public talkID talkID;

    private void OnEnable()
    {
        Accept.action.performed += OnAcceptPressed;
    }

    private void OnDisable()
    {
        Accept.action.performed -= OnAcceptPressed;
    }

    private void Start()
    {
        if (inkFile != null)
        {
            _story = new Story(inkFile.text);
        }
        textBox.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
    }

    #region LoadReferences
    public void LoadNewInk(TextAsset newInkFile)
    {
        inkFile = newInkFile;
        _story = new Story(inkFile.text);
        panel.gameObject.SetActive(true);
        ContinueStory();
    }

    public void LoadNonVaClip(AudioClip audioClip)
    {
        sideOcClip = audioClip;
    }

    public void LoadTalkID(talkID talkid)
    {
        talkID = talkid;
    }

    public void LoadSpeakerSource(AudioSource source)
    {
        audioSource = source;
    }
    #endregion

    private void OnAcceptPressed(InputAction.CallbackContext context)
    {
        if (_story == null) return;

        // If text is still typing, skip to full text immediately
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            textBox.text = _story.currentText;
            isTyping = false;
            ShowChoices();
            return;
        }

        // Only continue if there are NO choices currently
        if (_story.currentChoices.Count == 0)
        {
            ContinueStory();
        }
    }
    private void ContinueStory()
    {
        // Hide all choice buttons first
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }

        if (_story.canContinue)
        {
            player.canMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            textBox.gameObject.SetActive(true);
            panel.gameObject.SetActive(true);

            string line = _story.Continue();
            typingCoroutine = StartCoroutine(TypeText(line));
        }
        else
        {
            FinishTalking();
        }
    }

    private IEnumerator TypeText(string line)
    {
        isTyping = true;
        textBox.text = "";

        int letterCount = 0;
        float pitchVariation = 0.05f; // Optional: small random pitch variation
        int letterSkipForSound = 3;   // Only play sound every 3 letters

        foreach (char c in line)
        {
            textBox.text += c;
            letterCount++;

            if (talkID != null && talkID.typeID == typeID.NoVA && sideOcClip != null && audioSource != null)
            {
                if (letterCount % letterSkipForSound == 0)
                {
                    audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation); // optional
                    audioSource.PlayOneShot(sideOcClip);
                }
            }

            yield return new WaitForSeconds(0.02f); // Typing speed
        }

        audioSource.pitch = 1f; // Reset pitch
        isTyping = false;
        ShowChoices();
    }



    private void ShowChoices()
    {
        List<Choice> choices = _story.currentChoices;
        int index = 0;
        foreach (Choice c in choices)
        {
            choiceButtons[index].GetComponentInChildren<TextMeshProUGUI>().text = c.text;
            choiceButtons[index].gameObject.SetActive(true);
            index++;
        }
        for (int i = index; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
        }
    }

    public void SetChoice(int choiceIndex)
    {
        _story.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private void FinishTalking()
    {
        panel.gameObject.SetActive(false);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
        }

        player.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

public class InkExternalfunctions : MonoBehaviour
{
    public TalkManager talk;

    public void Bind(Story story, Animator anim)
    {
        story.BindExternalFunction("showPath", () => {
            talk.pathTrig.ShowPath();
        });
    }
}
