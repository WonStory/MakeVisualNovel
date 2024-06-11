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

        private const string CHARACTER_CASTING_ID = " as ";
        private const string CHARACTER_NAME_ID = "<charname>";
        private string characterRootPath => $"Characters/{CHARACTER_NAME_ID}";
        private string characterPrefabPath => $"{characterRootPath}/Character - [{CHARACTER_NAME_ID}]";//prefab의 이름을 경로안에서 찾아서 호출한다.

        [SerializeField] private RectTransform _characterpanel = null;
        public RectTransform characterPanel => _characterpanel;

        private void Awake()
        {
            instance = this;
        }

        public CharacterConfigData GetCharacterConfig(string characterName)
        {
            return config.GetConfig(characterName);
        }

        public Character GetCharacter(string chaacterName, bool createIfDoesNotExist = false)
        {
            if (characters.ContainsKey(chaacterName.ToLower()))
            {
                return characters[chaacterName.ToLower()];
            }
            else if (createIfDoesNotExist)
            {
                return CreateCharacter(chaacterName);
            }
            
            return null; //존재하지 않으면 생성 x

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

            string[] nameData = characterName.Split(CHARACTER_CASTING_ID, System.StringSplitOptions.RemoveEmptyEntries);//캐스팅 아이디로 분할한 다음 빈칸없애고 저장
            result.name = nameData[0];
            result.castingName = nameData.Length > 1 ? nameData[1] : result.name;//캐스팅 네임이 진짜라고 보면 편함. 없다면 원래 있는게 진짜이름

            result.config = config.GetConfig(result.castingName);

            result.prefab = GetPrefabForCharacter(result.castingName); //정보에 프리팯도 포함

            return result;
        }

        private GameObject GetPrefabForCharacter(string characterName)
        {
            string prefabPath = FormatCharacterPath(characterPrefabPath, characterName);
            return Resources.Load<GameObject>(prefabPath);
        }

        private string FormatCharacterPath(string path, string characterName) => path.Replace(CHARACTER_NAME_ID, characterName);

        private Character CreateCharacterFromInfo(CHARACTER_INFO Info)
        {
            CharacterConfigData config = Info.config;

            switch (config.characterType)
            {
                case Character.CharacterType.Text:
                    return new Character_Text(Info.name, config, Info.prefab);

                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    return new Character_Sprite(Info.name, config, Info.prefab);

                case Character.CharacterType.Live2D:
                    return new Character_Live2D(Info.name, config, Info.prefab);

                case Character.CharacterType.Model3D:
                    return new Character_Model3D(Info.name, config, Info.prefab);

                default:
                    return null;
            }


/*
            if (config.characterType == Character.CharacterType.Text)
            {
                return new Character_Text(Info.name, config); //config를 넣음으로써 이름과 고유한 데이터가 들어간다.
            }
            if (config.characterType == Character.CharacterType.Sprite || config.characterType == Character.CharacterType.SpriteSheet)
            {
                return new Character_Sprite(Info.name, config);
            }
            if (config.characterType == Character.CharacterType.Live2D)
            {
                return new Character_Live2D(Info.name, config);
            }
            if (config.characterType == Character.CharacterType.Model3D)
            {
                return new Character_Model3D(Info.name, config);
            }

            return null;
            */
        }



        private class CHARACTER_INFO //비공개 클래스 내부에 저장해둔다.
        {
            public string name = "";
            public string castingName = "";

            public CharacterConfigData config = null;

            public GameObject prefab = null;//정보를 받을 때 프리팯도 받아서 미리 만들어둔걸 바로 불러올 수 있게.
        }







    }

}
