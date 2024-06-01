using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using TMPro;
using UnityEngine;

namespace CHARACTERS
{
    [System.Serializable]
    public class CharacterConfigData
    {
        public string name;
        public string alias; //이건 캐릭터 명이 길수도 있으니 별칭으로써 존재
        public Character.CharacterType characterType;

        public Color nameColor;
        public Color DialogueColor;

        public TMP_FontAsset nameFont;
        public TMP_FontAsset DialogueFont;

        public CharacterConfigData Copy() //원본데이터를 유지하고 복사하여서 복사한걸로 활용하기 위해
        {
            CharacterConfigData result = new CharacterConfigData();
            
            result.name = name;
            result.alias = alias;//복사본을 만드는 중
            result.characterType = characterType;
            result.nameFont = nameFont;
            result.DialogueFont = DialogueFont;
            result.nameColor = new Color(nameColor.r, nameColor.g, nameColor.b, nameColor.a);
            result.DialogueColor = new Color(DialogueColor.r, DialogueColor.g, DialogueColor.b, DialogueColor.a);



            return result;
        }

        private static Color defaultColor => DialogueSystem.instance.config.defaultTextColor;
        private static TMP_FontAsset defaultFont => DialogueSystem.instance.config.defaultFont;

        public static CharacterConfigData Default
        {
            get
            {
                CharacterConfigData result = new CharacterConfigData();
            
                result.name = "";
                result.alias = "";//복사본을 만드는 중
                result.characterType = Character.CharacterType.Text;

                result.nameFont = defaultFont;
                result.DialogueFont = defaultFont;

                result.nameColor = new Color(defaultColor.r,defaultColor.g,defaultColor.b,defaultColor.a);
                result.DialogueColor = new Color(defaultColor.r,defaultColor.g,defaultColor.b,defaultColor.a);

                return result;
            }
        }
        
    }
}
