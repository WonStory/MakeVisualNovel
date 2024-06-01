using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using Unity.VisualScripting;
using UnityEngine;

namespace CHARACTERS
{
    public abstract class Character
    {
        public string name = "";
        public string displayname = "";
        public RectTransform root = null; //각 캐릭터마다 다른 특징을 보유해야된다.

        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        public Character(string name)
        {
            this.name = name;
            displayname = name;
        }

        public Coroutine Say(string dialogue) => Say(new List<string> {dialogue});
        public Coroutine Say(List<string> dialogue)
        {
            dialogueSystem.ShowSpeakerName(displayname);
            return dialogueSystem.Say(dialogue);
        }
        

        public enum CharacterType
        {
            Text,
            Sprite,
            SpriteSheet,
            Live2D,
            Model3D,
        }


    }
}

