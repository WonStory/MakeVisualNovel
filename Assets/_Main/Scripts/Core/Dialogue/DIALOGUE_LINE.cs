using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DIALOGUE_LINE
    {


        public string speaker;
        public string dialogue;
        public string commands; //일단 단순하게 유지
 
        public DIALOGUE_LINE(string speaker, string dialogue, string commands)
        {
            this.speaker = speaker;
            this.dialogue = dialogue;
            this.commands = commands;
        }
 
 
 
 
 
 
    }

}
