using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;

        private Coroutine process = null;
        public bool isRunning => process != null;

        private TextArchitection architection = null;
        private bool userPrompt = false;

        public ConversationManager(TextArchitection architection)
        {
            this.architection = architection;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        public void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        public void StartConversation(List<string> conversation)
        {
            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
        }

        public void StopConversation()
        {
            if (!isRunning)
            {
                return;//실행 중이 아닐때 아무것도 반환안함
            }

            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
            for (int i = 0; i < conversation.Count; i++)
            {
                //dont show any blank lines or try to run any logic on them.
                if (string.IsNullOrWhiteSpace(conversation[i])) //빈무자열은 건너뜀
                    continue;
                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);

                //show dialogue
                if (line.hasDialogue)
                {
                    yield return Line_RunDialogue(line);
                }

                //run any command
                if (line.hasCommands)
                {
                    yield return Line_RunCommands(line);
                }
            }


        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            if (line.hasSpeaker)//박스를 숨길지 말지 판단
            {
                dialogueSystem.ShowSpeakerName(line.speaker);
            }
            else
            {
                dialogueSystem.HideSpeakerName();
            }

            //이제 대화를 빌드해보자
            yield return BuildDialogue(line.dialogue);

            //사용자 입력을 기다려야댐
            yield return WaitForUserInput();
        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            Debug.Log(line.commands);
            yield return null;
        }

        IEnumerator BuildDialogue(string dialogue)
        {
            architection.Build(dialogue);

            while (architection.isBuilding)
            {
                if (userPrompt)
                {
                    if (!architection.hurryUp)
                    {
                        architection.hurryUp = true;
                    }
                    else
                    {
                        architection.ForceComplete();
                    }

                    userPrompt = false;
                }
                yield return null;

            }
        }

        IEnumerator WaitForUserInput()
        {
            while (!userPrompt)
                yield return null;

            userPrompt = false;
        }

    }
}