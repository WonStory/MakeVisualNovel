using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CHARACTERS;
using UnityEngine;

namespace COMMANDS
{
    public class CMD_DatabaseExtension_Characters : CMD_DatabaseExtension
    {
        private static string[] PARAM_ENABLE => new string[] { "-e", "-enable" };
        private static string[] PARAM_IMMEDIATE => new string[] { "-i", "-immediate"};
        private static string[] PARAM_SPEED => new string[] { "-spd", "-speed"};
        private static string[] PARAM_SMOOTH => new string[] { "-sm", "-smooth"};
        private static string PARAM_XPOS => "-x";
        private static string PARAM_YPOS => "-y";

        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("createcharacter", new Action<string[]>(CreateCharacter));
            database.AddCommand("movecharacter", new Func<string[], IEnumerator>(MoveCharacter));
            
            database.AddCommand("show", new Func<string[], IEnumerator>(ShowAll));
            database.AddCommand("hide", new Func<string[], IEnumerator>(HideAll));
        }
        public static void CreateCharacter(string[] data)
        {
            string characterName = data[0];
            bool enable = false;
            bool immediate = false;

            var parameters = ConvertDataToParameters(data); //전달하고

            parameters.TryGetValue(PARAM_ENABLE, out enable, defaultValue: false); //값을 받아옴, 디폴트는 비활성화, 다른 불필요한 스트링은 첨가하면 오류가 뜰수도
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false); //현재는 즉시 발현이 없어서 추가한다. enable은 나타내냐마냐 유무만 따질뿐 페이드인 아웃은 관련x

            Character character = CharacterManager.instance.CreateCharacter(characterName);
        
            if (!enable)
            {
                return;
            }
            if (immediate) //즉시냐에 따라 그냥 발현할지 폴스로 설정하고 따로 수식을 만들어 즉시 발현할지
            {
                character.isVisible = true; //이름이 등록되어있어야 텍스트 캐릭터가 아닌 기존의 프리팹으로 불러옴, 생성 순서에 따라 보이는 우선순위가 바뀜
            }
            else
            {
                character.Show();
            }
        }

        private static IEnumerator MoveCharacter(string[] data)
        {
            string characterName = data[0];
            Character character = CharacterManager.instance.GetCharacter(characterName);

            if (character == null) //캐릭터가 없을경우 실행안하도록
            {
                yield break;
            }

            float x = 0, y = 0; //변수들 설정
            float speed = 1;
            bool smooth = false;
            bool immediate = false;

            var parameters = ConvertDataToParameters(data);

            //x의 위치값을 받아옴
            parameters.TryGetValue(PARAM_XPOS, out x);

            //y의 위치값을 받아옴
            parameters.TryGetValue(PARAM_YPOS, out y);

            //스피드값을 받아옴
            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1);

            //스무스할건지 불값을 받아옴
            parameters.TryGetValue(PARAM_SMOOTH, out smooth, defaultValue: false);

            //즉시 나타낼건지 받아옴 
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            Vector2 position = new Vector2(x, y);

            if (immediate)
            {
                character.SetPosition(position);
            }
            else
            {
                yield return character.MoveToPosition(position, speed, smooth);
            }
        }

        public static IEnumerator ShowAll(string[] data)
        {
            List<Character> characters = new List<Character>();
            bool immediate = false; //즉시 전환을 할건지에 대한 불

            foreach (string s in data) //입력받은 배열을 쪼개서 로컬 리스트에 다 넣는다
            {
                Character character = CharacterManager.instance.GetCharacter(s, createIfDoesNotExist: false);
                if (character != null)
                {
                    characters.Add(character); //캐릭터 목록에 추가
                }
            }

            if (characters.Count == 0) //카운트가 0이면 아무것도 안하고 브레이크
            {
                yield break;
            }

            //데이터 배열을 파라미터 컨테이너로 변환하다.
            var parameters = ConvertDataToParameters(data); //변환한 데이터 전달함

            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            //로직을 불러옴
            foreach (Character character in characters)
            {
                if (immediate)
                {
                    character.isVisible = true; //즉시이므로
                }
                else
                {
                    character.Show(); //즉시가 아니면 그냥 쇼로
                }
            }

            if (!immediate)
            {
                while (characters.Any(c => c.isReaveling)) //소스가 있는지 확인한다.
                {
                    yield return null;
                }
            }
        }

        public static IEnumerator HideAll(string[] data)
        {
            List<Character> characters = new List<Character>();
            bool immediate = false; //즉시 전환을 할건지에 대한 불

            foreach (string s in data) //입력받은 배열을 쪼개서 로컬 리스트에 다 넣는다
            {
                Character character = CharacterManager.instance.GetCharacter(s, createIfDoesNotExist: false);
                if (character != null)
                {
                    characters.Add(character); //캐릭터 목록에 추가
                }
            }

            if (characters.Count == 0) //카운트가 0이면 아무것도 안하고 브레이크
            {
                yield break;
            }

            //데이터 배열을 파라미터 컨테이너로 변환하다.
            var parameters = ConvertDataToParameters(data); //변환한 데이터 전달함

            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            //로직을 불러옴
            foreach (Character character in characters)
            {
                if (immediate)
                {
                    character.isVisible = false; //즉시이므로
                }
                else
                {
                    character.Hide(); //즉시가 아니면 그냥 쇼로
                }
            }

            if (!immediate)
            {
                while (characters.Any(c => c.isHiding)) //소스가 있는지 확인한다.
                {
                    yield return null;
                }
            }
        }
    }
}


