using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class Testing_Architect : MonoBehaviour
    {

        DialogueSystem ds;
        TextArchitection architect; //여기서 정의안함

        string[] lines = new string[5]
        {
            "hihihihihihihihihihihihihihihihihihihihihi",
            "hellohellohellohellohellohellohellohellohellohellohello",
            "byebyebyebyebyebyebyebye",
            "goodgoodgoodgoodgoodgoodgoodgoodgood",
            "awsomeawsomeawsomeawsomeawsomeawsomeawsomeawsomeawsomeawsome"
        };

        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.instance;
            architect = new TextArchitection(ds.dialogueContainer.dialogueText);
            architect.buildMethod = TextArchitection.BuildMethod.typewriter;
        }

        // Update is called once per frame
        void Update()
        {
            string longline = "ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (architect.isBuilding)
                {
                    if (!architect.hurryUp)
                    {
                        architect.hurryUp = true;
                    }
                    else
                    {
                        architect.ForceComplete();
                    }
                }
                else
                    architect.Build(longline);
                //architect.Build(lines[Random.Range(0, lines.Length)]);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                architect.Append(longline);
                //architect.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}