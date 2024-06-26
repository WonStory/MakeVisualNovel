using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CHARACTERS;


namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField]private DialogueSystemConfigurationSO _config;
        public DialogueSystemConfigurationSO config => _config;

        public DialogueContainer dialogueContainer = new DialogueContainer();
        private ConversationManager conversationManager;
        private TextArchitection architection;

        public static DialogueSystem instance {get; private set;} //공개적이고 스태틱이라 언제든 바뀔 수 있었는데 이제 제한을 걸음, 대화시스템으로만 가능

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

        public void ApplySpeakerDataToDialogueContainer(string speakerName)
        {
            Character character = CharacterManager.instance.GetCharacter(speakerName);
            CharacterConfigData config = character != null ? character.config : CharacterManager.instance.GetCharacterConfig(speakerName);
        
            ApplySpeakerDataToDialogueContainer(config); //이렇게 분리해놓으면 데이터가 없을 때 이름으로 호출이 가능하고, 데이터가 있으면 직접 호출이 가능해진다.
        }

        public void ApplySpeakerDataToDialogueContainer(CharacterConfigData config)
        {
            dialogueContainer.SetDialogueColor(config.dialogueColor);
            dialogueContainer.SetDialogueFont(config.dialogueFont);
            dialogueContainer.nameContainer.SetNameColor(config.nameColor);
            dialogueContainer.nameContainer.SetNameFont(config.nameFont);
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


        public Coroutine Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() {$"{speaker} \"{dialogue}\""};
            return Say(conversation);
        }

        public Coroutine Say(List<string> conversation)
        {
            return conversationManager.StartConversation(conversation);
        }



    }
}