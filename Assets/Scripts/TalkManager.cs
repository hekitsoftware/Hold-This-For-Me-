using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.UIElements;

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

    private Story _story;

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

    public void LoadNewInk(TextAsset newInkFile)
    {
        inkFile = newInkFile;
        _story = new Story(inkFile.text);
        panel.gameObject.SetActive(true);
        ContinueStory();
    }

    private void OnAcceptPressed(InputAction.CallbackContext context)
    {
        if (_story == null) return;

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
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

            textBox.gameObject.SetActive(true);
            panel.gameObject.SetActive(true);
            textBox.text = _story.Continue();
            ShowChoices();
        }
        else
        {
            FinishTalking();
        }
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
        for (int i = index; i < 2; i++)
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
        for(int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive( false);
        }

        player.canMove = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }
}
