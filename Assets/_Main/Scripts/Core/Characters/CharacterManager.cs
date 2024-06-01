using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace CHARACTERS
{
    public class CharacterManager : MonoBehaviour
    {
        
        public static CharacterManager instance { get; private set; }
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();

        private CharacterConfigSO config => DialogueSystem.instance.config.characterConfigurationAsset;

        private void Awake()
        {
            instance = this;
        }

        public Character CreateCharacter(string characterName)
        {
            if (characters.ContainsKey(characterName.ToLower())) //중복되는지 확인하는 키
            {
                Debug.LogWarning($"A Character called '{characterName}' already exists. Did not create the character.");
                return null;
            }

            CHARACTER_INFO info = GetCharacterInfo(characterName);

            Character character = CreateCharacterFromInfo(info);

            characters.Add(characterName.ToLower(), character); //동일한 문자를 2번생성하지 않도록 설정함.

            return character;
        }

        private CHARACTER_INFO GetCharacterInfo(string characterName) //비공개 클래스에서 정보를 얻어온다.
        {
            CHARACTER_INFO result = new CHARACTER_INFO();

            result.name = characterName;

            result.config = config.GetConfig(characterName);

            return result;
        }

        private Character CreateCharacterFromInfo(CHARACTER_INFO Info)
        {
            CharacterConfigData config = Info.config;

            if (config.characterType == Character.CharacterType.Text)
            {
                return new Character_Text(Info.name);
            }
            if (config.characterType == Character.CharacterType.Sprite || config.characterType == Character.CharacterType.SpriteSheet)
            {
                return new Character_Sprite(Info.name);
            }
            if (config.characterType == Character.CharacterType.Live2D)
            {
                return new Character_Live2D(Info.name);
            }
            if (config.characterType == Character.CharacterType.Model3D)
            {
                return new Character_Model3D(Info.name);
            }

            return null;
        }



        private class CHARACTER_INFO //비공개 클래스 내부에 저장해둔다.
        {
            public string name = "";

            public CharacterConfigData config = null;
        }







    }

}
