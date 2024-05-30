using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

public class DL_DIALOGUE_DATA
{
    public List<DIALOGUE_SEGMENT> segments;//목록으로 만들고 그것을 segments라고 명명함
    private const string segmentIdentifierPattern = @"\{[ca]\}|\{w[ca]\s\d*\.?\d*\}";//@를 사용하여 문장 시작 s는 공백 d는 숫자(정수)

    public DL_DIALOGUE_DATA(string rawDialogue)
    {
        segments = RipSegments(rawDialogue);
    }

    public List<DIALOGUE_SEGMENT> RipSegments(string rawDialogue)
    {
        List<DIALOGUE_SEGMENT> segments = new List<DIALOGUE_SEGMENT>();
        MatchCollection matches = Regex.Matches(rawDialogue, segmentIdentifierPattern); //맨 처음부분은 식별자가 없으므로

        int LastIndex = 0;
        //하나 이상의 시그멘트를 파일 내에서 찾는다
        DIALOGUE_SEGMENT segment = new DIALOGUE_SEGMENT();
        segment.dialogue = matches.Count == 0 ? rawDialogue : rawDialogue.Substring(0, matches[0].Index);//맞춰보고 안맞으면 바꾸고, 맞으면 첫번째 일치까지 찾으면 된다.
        segment.startSignal = DIALOGUE_SEGMENT.StartSignal.NONE; //시작신호가 없음
        segment.signalDelay = 0; //신호지연을 일단 0으로
        segments.Add(segment);

        if (matches.Count == 0)
        {
            return segments;
        }
        else
        {
            LastIndex = matches[0].Index;
        }

        for (int i = 0; i < matches.Count; i++)
        {
            Match match = matches[i];
            segment = new DIALOGUE_SEGMENT();

            //스타트시그널을 시그멘트에서 얻는다.
            string signalMatch = match.Value;//(A)
            signalMatch = signalMatch.Substring(1, match.Length - 2); //값자체만 원하므로
            string[] signalSplit = signalMatch.Split(' ');

            segment.startSignal = (DIALOGUE_SEGMENT.StartSignal) Enum.Parse(typeof(DIALOGUE_SEGMENT.StartSignal), signalSplit[0].ToUpper());//대문자로 변환시켜서 패턴이랑 일치 시킴
        
            //시그널 딜레이 얻기
            if (signalSplit.Length > 1)
            {
                float.TryParse(signalSplit[1], out segment.signalDelay);
            }

            //시그멘트에서 다이어로그 얻기
            int nextIndex = i + 1 < matches.Count ? matches[i+1].Index : rawDialogue.Length;
            segment.dialogue = rawDialogue.Substring(LastIndex + match.Length, nextIndex - (LastIndex + match.Length));
            LastIndex = nextIndex;

            segments.Add(segment);
        }
        
        return segments;
    }


    public struct DIALOGUE_SEGMENT
    {
        public string dialogue;
        public StartSignal startSignal;
        public float signalDelay;

        public enum StartSignal { NONE, C, A, WA, WC}

        public bool appendText => (startSignal == StartSignal.A || startSignal == StartSignal.WA);
    }



}
