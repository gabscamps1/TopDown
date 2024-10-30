using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class DialogueManager : MonoBehaviour
{
    [Header("NPC INFO")]
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image targetImage; // Imagem exibida antes de cada sentença

    [Header("Dialogue Box")]
    public Animator animator;
    public Animator PortraitAnimation;
    public float timeDialogue;

    [SerializeField] private Sprite defaultBackground; // Defina o sprite padrão no inspetor

    public Image backgroundImage; // Imagem de fundo no Canvas

    public DialogueEffects dialogueEffects;

    private Queue<SentenceData> sentences; // Armazena as sentenças com imagens e nomes
    private bool isTyping = false;
    private string currentSentence;

    private static readonly Regex tagRegex = new Regex(@"<.*?>", RegexOptions.Compiled);
    private static readonly Regex commandRegex = new Regex(@"<call:(\w+)>", RegexOptions.Compiled);

    void Start()
    {
        sentences = new Queue<SentenceData>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueEffects?.ResetDialogPosition();
        dialogueEffects?.TriggerFadeOut();
        animator.SetBool("IsOpen", true);
        PortraitAnimation.SetBool("IsOpen", true);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (SentenceData sentenceData in dialogue.sentences)
        {
            sentences.Enqueue(sentenceData);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            dialogueText.text = currentSentence;
            isTyping = false;
        }
        else if (sentences.Count == 0)
        {
            EndDialogue();
        }
        else
        {
            SentenceData sentenceData = sentences.Dequeue();
            currentSentence = sentenceData.sentence;

            // Atualiza o nome do personagem e a imagem para a sentença atual
            UpdateName(sentenceData.characterName);
            UpdateImage(sentenceData.sentenceImage);
            UpdateBackgroundImage(sentenceData.backgroundImage);

            PortraitAnimation.SetBool("IsOpen", false);
            PortraitAnimation.SetBool("IsOpen", true);
            

            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentSentence));
        }
    }

    private void UpdateName(string characterName)
    {
        if (!string.IsNullOrEmpty(characterName))
        {
            nameText.text = characterName; // Usa o nome específico da sentença
        }
    }

    private void UpdateBackgroundImage(Sprite background)
    {
        if (backgroundImage.sprite == null)
        {
            backgroundImage.sprite = defaultBackground; // Define o sprite padrão
            backgroundImage.color = new Color(0f, 0f, 0f, 0f); // Torna a imagem visível
            print(backgroundImage);
        }
        else {
            

            /*backgroundImage.sprite = background;
            backgroundImage.color = new Color(1f, 1f, 1f, 1f); // Torna a imagem visível
            print("Com imagem fornecida");
            print(backgroundImage.sprite);*/
        }
       
    }

   

    /*private void UpdateBackgroundImage(Sprite background)
    {
        if (background != null)
        {
            backgroundImage.sprite = background;
            backgroundImage.color = new Color(0f, 0f, 0f, 1f); // Torna a imagem visível
            print("Com imagem fornecida");
        }
        else if (backgroundImage.sprite == null) // Caso ainda não haja um sprite
        {
            backgroundImage.sprite = defaultBackground; // Define o sprite padrão
            backgroundImage.color = new Color(0f, 0f, 0f, 1f); // Torna a imagem visível
            print("Sem imagem fornecida, usando padrão");
        }
    }*/


    private void UpdateImage(Sprite sentenceSprite)
    {
        if (targetImage != null)
        {
            if (sentenceSprite != null)
            {
                targetImage.sprite = sentenceSprite;
                targetImage.color = new Color(1f, 1f, 1f, 1f); // Torna a imagem visível
            }
            else
            {
                targetImage.sprite = null;
                targetImage.color = new Color(1f, 1f, 1f, 0f); // Torna a imagem invisível se não houver sprite
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        int charIndex = 0;
        string currentText = "";
        string openTags = "";
        isTyping = true;

        while (charIndex < sentence.Length)
        {
            Match commandMatch = commandRegex.Match(sentence.Substring(charIndex));
            if (commandMatch.Success)
            {
                string command = commandMatch.Groups[1].Value;
                ExecuteCommand(command);
                charIndex += commandMatch.Length;
                continue;
            }

            if (sentence[charIndex] == '<')
            {
                Match tagMatch = tagRegex.Match(sentence.Substring(charIndex));
                if (tagMatch.Success)
                {
                    currentText += tagMatch.Value;

                    if (!tagMatch.Value.Contains("/"))
                    {
                        openTags += tagMatch.Value;
                    }
                    else
                    {
                        int lastOpenTagIndex = openTags.LastIndexOf('<');
                        if (lastOpenTagIndex != -1)
                        {
                            openTags = openTags.Substring(0, lastOpenTagIndex);
                        }
                    }

                    charIndex += tagMatch.Length;
                }
            }
            else
            {
                currentText += sentence[charIndex];
                charIndex++;
                dialogueText.text = currentText + openTags;
                yield return new WaitForSeconds(timeDialogue);
            }
        }

        isTyping = false;
    }

    void ExecuteCommand(string command)
    {
        switch (command)
        {
            case "FlashScreen":
                GameObject.Find("DialogueManager").GetComponent<ScreenFlash>().FlashScreen();
                break;

            case "ScreenShake":
                GameObject.Find("DialogueManager").GetComponent<ScreenShake>().TriggerShake();
                break;

            /*case "ScreenShake":
                dialogueEffects?.TriggerShake();
                break;*/

            case "FadeIn":
                dialogueEffects?.TriggerFadeIn();
                break;

            case "FadeOut":
                dialogueEffects?.TriggerFadeOut();
                break;

            case "CenterDialog":
                dialogueEffects?.CenterDialog();
                break;

            case "MiddleDialog":
                dialogueEffects?.MiddleDialog();
                break;

            case "ResetDialog":
                dialogueEffects?.ResetDialogPosition();
                break;

            default:
                Debug.LogWarning("Comando não reconhecido: " + command);
                break;
        }
    }

    void EndDialogue()
    {
       
        Debug.Log("End of Conversation");
        animator.SetBool("IsOpen", false);
        PortraitAnimation.SetBool("IsOpen", false);
        //dialogueEffects?.ResetDialogPosition();
    }
}
