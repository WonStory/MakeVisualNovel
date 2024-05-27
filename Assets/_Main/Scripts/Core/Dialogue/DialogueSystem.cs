using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        public DialogueContainer dialogueContainer = new DialogueContainer();
        private ConversationManager conversationManager;
        private TextArchitection architection;

        public static DialogueSystem instance;

        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPrompt_Next;

        public bool isRunningConversation => conversationManager.isRunning;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Initialize();
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }


        bool _initialized = false;
        private void Initialize()
        {
            if (_initialized)
                return;

            architection = new TextArchitection(dialogueContainer.dialogueText);
            conversationManager = new ConversationManager(architection);
        }

        public void OnUserPrompt_Next()
        {
            onUserPrompt_Next?.Invoke(); //멀티스레드 충돌을 방지하기위해서
        }

        public void ShowSpeakerName(string speakerName = "")
        {
            if (speakerName.ToLower() != "narrator")
                dialogueContainer.nameContainer.Show(speakerName);
            else
                HideSpeakerName();
        }
        
        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();


        public void Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() {$"{speaker} \"{dialogue}\""};
            Say(conversation);
        }

        public void Say(List<string> conversation)
        {
            conversationManager.StartConversation(conversation);
        }



    }
}