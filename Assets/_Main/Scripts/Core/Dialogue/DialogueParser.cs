using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueParser
    {
        private const string commandRegexPattern = @"\w*[^\s]\("; //명령정규식 패턴 *은 여러 아규를 나타낸다 @로 나타내면 \를 하나만 해도 된다.


        public static DIALOGUE_LINE Parse(string rawline)
        {
            Debug.Log($"Parsing line - '{rawline}'");

            (string speaker, string dialogue, string commands) = RipContent(rawline);

            Debug.Log($"Speaker = '{speaker}'\nDialogue = '{dialogue}'\nCommands = '{commands}'");

            return new DIALOGUE_LINE(speaker, dialogue, commands);

        }

        private static (string, string, string) RipContent(string rawline)
        {
            string speaker = "", dialogue = "", commands = "";

            int dialogueStart = -1;
            int dialogueEnd = -1;
            bool isEscaped = false;

            for (int i = 0; i < rawline.Length; i++)
            {
                char current = rawline[i];
                if (current == '\\') // \이게 처음 나타났을 때 이스케이프가 발동한다
                {
                    isEscaped = !isEscaped;//반대로 바꿔주면서 대화부분을 찾는 것
                }
                else if (current == '"' && !isEscaped)
                {
                    if (dialogueStart == -1)
                    {
                        dialogueStart = i;
                    }
                    else if (dialogueEnd == -1)
                    {
                        dialogueEnd = i;
                    }
                }
                else
                {
                    isEscaped = false;
                }
            }

            //Debug.Log(rawline.Substring(dialogueStart + 1, (dialogueEnd - dialogueStart) - 1)); 인덱싱을 보여줌(2번째건 문자 개수 이므로 빼줘서 개수를 구한다)

            Regex commandRegex = new Regex(commandRegexPattern);
            Match match = commandRegex.Match(rawline);
            int commandStart = -1;
            if (match.Success)
            {
                commandStart = match.Index;//음수로 설정해 놓고 맞으면 인덱스로 갈아넣음

                if (dialogueStart == -1 && dialogueEnd == -1)
                    return ("", "", rawline.Trim());//스피커랑 대화는 빠져있으므로 ""로 나타냄
            }

            if (dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd)) //간단히 수학적비교를 통해 어딕 어디인지 파악함
            {
                speaker = rawline.Substring(0, dialogueStart).Trim();
                dialogue = rawline.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1).Replace("\\\"","\"");
                if (commandStart != -1)
                {
                    commands = rawline.Substring(commandStart).Trim();
                }
            }
            else if (commandStart != -1 && dialogueStart > commandStart)
            {
                commands = rawline;
            }
            else
            {
                speaker = rawline;
            }

            return (speaker, dialogue, commands);
        }









    }

    
}
