using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE
{
    public class DL_SPEAKER_DATA
    {
        public string name, castName; //트루 네임과 화면에 표시하는 네임(가짜)

        public string displayname => isCastingName ? castName : name; //캐스팅하고있는지에 따라 이름을 반환(일치하는가)

        public Vector2 castPosition; //캐릭터가 화면에 나타나는 위치
        public List<(int layer, string expression)> CastExpresstions { get; set; }//하나이상의 레이어를 가지고 있고, 여러 모션을 보여줄 수 있다. 하나의 튜플로 조합하면 해결된다.

        public bool isCastingName => castName != string.Empty; //이름을 캐스팅하고 있는가,
        public bool isCastingPosition = false; //수동으로 할당해야댐(at 커맨드에 해당)
        public bool isCastingExpressions => CastExpresstions.Count > 0; //캐스팅 표현식의 도트수가 0보다 큰지
        
        public bool makeCharacterEnter = false; //이 클래스에 엑세스하려면 다른 클래스가 필요하므로 공개 불을 만듬

        private const string NAMECAST_ID = " as ";
        private const string POSITIONCAST_ID = " at ";
        private const string EXPRESSIONCAST_ID = " ["; //대괄호만 쓰고싶어서 아래로 내림
        private const char AXISDELIMITER_ID = ':';
        private const char EXPRESSIONLAYER_JOINER = ','; //표현식과 결합
        private const char EXPRESSIONLAYER_DELIMITER = ':'; //표현식의 분리

        private const string ENTER_KEYWORD = "enter "; //대화파일 안에 엔터를 넣어놓으면 캐릭터가 생설될 뿐만 아니라 등장하게 하고 싶어서 넣는 키워드(맨처음 쓰는 키워드이기에 공백이 따라오므로 공백도 넣어줌)

        private string  ProcessKeywords(string rawSpeaker)
        {
            if (rawSpeaker.StartsWith(ENTER_KEYWORD)) //엔터랑 같이 시작하면(지정된 문자열로 시작하는지)
            {
                rawSpeaker = rawSpeaker.Substring(ENTER_KEYWORD.Length); //범위를 지정안하는 서브스트링은 그 이후 인덱스부터 쭉 출력함을 의미한다. 즉, 엔터 이후부터 출력하겠다는거
                makeCharacterEnter = true;
            }

            return rawSpeaker;
        }

        public DL_SPEAKER_DATA(string rawSpeaker)
        {
            rawSpeaker = ProcessKeywords(rawSpeaker);

            string pattern = @$"{NAMECAST_ID}|{POSITIONCAST_ID}|{EXPRESSIONCAST_ID.Insert(EXPRESSIONCAST_ID.Length-1, @"\")}"; //@으로 나타내서 \를 하나만 적어도 된다.
            MatchCollection matches = Regex.Matches(rawSpeaker, pattern);

            //일치 항목이 있든 없든 채워넣어야 돼서 if밖으로 빼냄
            castName = "";
            castPosition = Vector2.zero;
            CastExpresstions = new List<(int layer, string expression)>();

            if (matches.Count == 0)
            {
                name =rawSpeaker;
                return;
            }

            int index = matches[0].Index;
            name = rawSpeaker.Substring(0, index);

            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                int startIndex = 0, endIndex = 0;//계속 참조할 것이기에 if 밖에다가 해놈

                if (match.Value == NAMECAST_ID)
                {
                    startIndex = match.Index + NAMECAST_ID.Length;
                    endIndex = (i < matches.Count - 1) ? matches[i+1].Index : rawSpeaker.Length;
                    castName = rawSpeaker.Substring(startIndex, endIndex - startIndex);
                }
                else if (match.Value == POSITIONCAST_ID)
                {
                    isCastingPosition = true;
                    startIndex = match.Index + POSITIONCAST_ID.Length;
                    endIndex = (i < matches.Count - 1) ? matches[i+1].Index : rawSpeaker.Length;
                    string castPos = rawSpeaker.Substring(startIndex, endIndex - startIndex);
                    string[] axis = castPos.Split(AXISDELIMITER_ID, System.StringSplitOptions.RemoveEmptyEntries);

                    float.TryParse(axis[0], out castPosition.x); //파싱을 하여 반응하는 경우 가져온다 x를

                    if (axis.Length > 1)
                    {
                        float.TryParse(axis[1], out castPosition.y);
                    }
                }
                else if (match.Value == EXPRESSIONCAST_ID)
                {
                    startIndex = match.Index + EXPRESSIONCAST_ID.Length;
                    endIndex = (i < matches.Count - 1) ? matches[i+1].Index : rawSpeaker.Length;
                    string castExp = rawSpeaker.Substring(startIndex, endIndex - (startIndex + 1));

                    CastExpresstions = castExp.Split(EXPRESSIONLAYER_JOINER).Select(
                    x => 
                    { 
                    var parts = x.Trim().Split(EXPRESSIONLAYER_DELIMITER);

                    if (parts.Length == 2)
                    {
                        return (int.Parse(parts[0]), parts[1]);    
                    }
                    else
                    {
                        return (0, parts[0]);    
                    }
                    }).ToList();
                }

            }
        }








    }
}

