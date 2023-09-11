using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI dialogueName;
    [SerializeField] private Image face;

    [SerializeField] private string[] sentences;
    [SerializeField] private float wordSpeed;
    [SerializeField] private int index;

    private bool isDialoguePlaying;

    private ItemSO item;

    public static DialogueManager instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        index = 0;
        isDialoguePlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!isDialoguePlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (dialogueText.text == sentences[index])
            {
                wordSpeed = 0.1f;
                ContinueStory();
            }
            else
            {
                wordSpeed = 0.01f;
            }
        } 
    }

    public void EnterDialogueMode(DialogueSO dialogue, ItemSO itemNPC)
    {
        isDialoguePlaying = true;
        dialoguePanel.SetActive(true);
        item = itemNPC;
        sentences = dialogue.sentences;
        dialogueName.text = dialogue.name;
        face.sprite = dialogue.face;

        StartCoroutine(Typing());
    }

    IEnumerator Typing()
    {
        dialogueText.text = "";
        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private void ExitDialogueMode()
    {
        isDialoguePlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        index = 0;
    }

    private void ContinueStory()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(Typing());
        } 
        else
        {
            //GiveItem();
            ExitDialogueMode();
        }
    }

    private void GiveItem()
    {
        if (item != null)
        {
            InventoryManager.instance.AddItem(item);
        }
    }
}
