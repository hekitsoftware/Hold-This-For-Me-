using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class TalkManager : MonoBehaviour
{
    [Header("Ink & UI")]
    public TextAsset inkFile;
    public TextMeshProUGUI textBox;
    public InputActionReference Accept;
    public UnityEngine.UI.Button[] choiceButtons;
    public UnityEngine.UI.Image panel;

    [Header("References")]
    public PlayerMovement player;

    private Ink.Runtime.Story _story;
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
            _story = new Ink.Runtime.Story(inkFile.text);
        }
        textBox.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
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

    #region TALKING
    private void OnAcceptPressed(InputAction.CallbackContext context)
    {
        if (_story == null) return;

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            textBox.text = _story.currentText;
            isTyping = false;
            ShowChoices();
            return;
        }

        if (_story.currentChoices.Count == 0)
        {
            ContinueStory();
        }
    }

    public void ContinueStory()
    {
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
    #endregion

    #region TYPETEXT
    private IEnumerator TypeText(string line)
    {
        isTyping = true;
        textBox.text = "";

        int letterCount = 0;
        float pitchVariation = 0.05f;
        int letterSkipForSound = 3;

        foreach (char c in line)
        {
            textBox.text += c;
            letterCount++;

            if (talkID != null && talkID.typeID == typeID.NoVA && sideOcClip != null && audioSource != null)
            {
                if (letterCount % letterSkipForSound == 0)
                {
                    audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
                    audioSource.PlayOneShot(sideOcClip);
                }
            }

            yield return new WaitForSeconds(0.02f);
        }

        audioSource.pitch = 1f;
        isTyping = false;
        ShowChoices();
    }
    #endregion

    #region CHOICES
    private void ShowChoices()
    {
        List<Ink.Runtime.Choice> choices = _story.currentChoices;
        int index = 0;
        foreach (Ink.Runtime.Choice c in choices)
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

    public void FinishTalking()
    {
        panel.gameObject.SetActive(false);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
        }

        player.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.ZoomOut();
    }
    #endregion
}
