using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE
{
    public class DL_SPEAKER_DATA
    {
        public string name, castName; //트루 네임과 화면에 표시하는 네임(가짜)

        public string displayname => (castName != string.Empty ? castName : name);

        public Vector2 castPosition; //캐릭터가 화면에 나타나는 위치
        public List<(int layer, string expression)> CastExpresstions { get; set; }//하나이사으이 레이어를 가지고 있고, 여러 모션을 보여줄 수 있다. 하나의 튜플로 조합하면 해결된다.

        private const string NAMECAST_ID = " as ";
        private const string POSITIONCAST_ID = " at ";
        private const string EXPRESSIONCAST_ID = " ["; //대괄호만 쓰고싶어서 아래로 내림
        private const char AXISDELIMITER_ID = ':';
        private const char EXPRESSIONLAYER_JOINER = ','; //표현식과 결합
        private const char EXPRESSIONLAYER_DELIMITER = ':'; //표현식의 분리

        public DL_SPEAKER_DATA(string rawSpeaker)
        {
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
                    return (int.Parse(parts[0]), parts[1]);
                    }).ToList();
                }

            }
        }








    }
}

