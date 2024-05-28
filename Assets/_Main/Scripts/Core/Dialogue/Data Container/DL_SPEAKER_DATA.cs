using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DL_SPEAKER_DATA
{
    public string name, castName; //트루 네임과 화면에 표시하는 네임(가짜)
    public Vector2 castPosition; //캐릭터가 화면에 나타나는 위치
    public List<(int layer, string expression)> CastExpresstions { get; set; }//하나이사으이 레이어를 가지고 있고, 여러 모션을 보여줄 수 있다. 하나의 튜플로 조합하면 해결된다.

    public DL_SPEAKER_DATA(string rawSpeaker)
    {
        string pattern = @" as | at | \["; //@으로 나타내서 \를 하나만 적어도 된다.
        MatchCollection matches = Regex.Matches(rawSpeaker, pattern);
        if (matches.Count == 0)
        {
            name =rawSpeaker;
            castName = "";
            castPosition = Vector2.zero;
            CastExpresstions = new List<(int layer, string expression)>();
            return;
        }
    }








}
