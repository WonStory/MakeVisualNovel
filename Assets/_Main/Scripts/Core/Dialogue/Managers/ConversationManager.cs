using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COMMANDS;

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

        public Coroutine StartConversation(List<string> conversation)
        {
            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));

            return process;
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
                    //yield return WaitForUserInput(); //이게 대화마다 잠시 멈추게 해줌.
                }

                //run any command
                if (line.hasCommands)
                {
                    yield return Line_RunCommands(line);
                }
                if (line.hasCommands)
                {
                    //사용자 입력을 기다려야댐
                    yield return WaitForUserInput();
                }

            }


        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            if (line.hasSpeaker)//박스를 숨길지 말지 판단
            {
                dialogueSystem.ShowSpeakerName(line.speakerData.displayname);
            }

            //이제 대화를 빌드해보자
            yield return BuildLineSegments(line.dialogueData);

    
        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            List<DL_COMMAND_DATA.Command> commands = line.commandData.commands;

            foreach (DL_COMMAND_DATA.Command command in commands)
            {
                if (command.waitForCompletion)
                {
                    yield return CommandManager.Instance.Execute(command.name, command.arguments);
                }
                else
                {
                    CommandManager.Instance.Execute(command.name, command.arguments);
                }
                
            }
            
            yield return null;
        }

        IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line)
        {
            for (int i = 0; i < line.segments.Count; i++)
            {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];

                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);

                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }
        }

        IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            switch (segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                yield return WaitForUserInput(); //c나 a는 유저의 인풋을 기다린다.
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:    
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                default:
                    break;
            }
        }

        IEnumerator BuildDialogue(string dialogue, bool append = false)
        {//빌드 다이어로그
            if (!append)
            {
                architection.Build(dialogue);
            }
            else
            {
                architection.Append(dialogue);
            }
            //다이어로그 빌드까지 기다리기
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