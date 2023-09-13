using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Panel")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI dialogueName;
    [SerializeField] private Image face;

    [Header("In game")]
    [SerializeField] private GameObject notification;
    [SerializeField] private GameObject choise;

    [Header("In Script")]
    [SerializeField] private string dialogueType;
    [SerializeField] private string[] sentences;
    [SerializeField] private float wordSpeed;
    [SerializeField] private int index;

    public bool isDialoguePlaying { get; private set; }

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
        choise.SetActive(false);
        notification.SetActive(false);
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

        //if (index == sentences.Length - 1 && dialogueType == "Choise")
        //{
        //    choise.SetActive(true);
        //    isDialoguePlaying = false;
        //}
    }

    public void EnterDialogueMode(DialogueSO dialogue, ItemSO itemNPC)
    {
        isDialoguePlaying = true;
        dialoguePanel.SetActive(true);
        item = itemNPC;
        sentences = dialogue.sentences;
        dialogueName.text = dialogue.name;
        face.sprite = dialogue.face;
        dialogueType = dialogue.type.ToString();

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
            if (index == sentences.Length - 2 && dialogueType == "Choise")
            {
                choise.SetActive(true);
            }
            index++;
            StartCoroutine(Typing());
        }
        else if (index == sentences.Length - 1)
        {
            ExitDialogueMode();
        } 
    }

    public void OnClickChoise0()
    {
        notification.SetActive(true);
        ExitDialogueMode();
    }

    public void OnClickChoise1()
    {
        ExitDialogueMode();
    }

    private void GiveItem()
    {
        if (item != null)
        {
            InventoryManager.instance.AddItem(item);
        }
    }
}
