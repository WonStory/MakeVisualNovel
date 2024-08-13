using System.Collections;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;
using TMPro;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {
        public TMP_FontAsset tempFont;

        private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

        // Start is called before the first frame update
        void Start()
        {
            // Character Elen = CharacterManager.instance.CreateCharacter("Elen");
            //Character Fs2 = CharacterManager.instance.CreateCharacter("Raelin");
            // Character Stella2 = CharacterManager.instance.CreateCharacter("Stella");
            // Character Adam = CharacterManager.instance.CreateCharacter("Adam");
            StartCoroutine(Test());
        }

        IEnumerator Test()
        {
            
            //Character_Sprite Guard = CreateCharacter("Guard as Generic") as Character_Sprite;
            Character_Sprite Raelin = CreateCharacter("Raelin") as Character_Sprite;
            Character_Sprite Stella = CreateCharacter("Stella") as Character_Sprite;
            //Character_Sprite Guard = CreateCharacter("Guard as Generic") as Character_Sprite;
        
            Raelin.SetPosition(new Vector2(0,0));
            Stella.SetPosition(new Vector2(1,0));

            yield return new WaitForSeconds(1);

            Stella.TransitionSprite(Stella.GetSprite("Raelin_1"));
            Stella.TransitionSprite(Stella.GetSprite("Raelin_11"), layer: 1);
            Stella.Animate("Hop");
            yield return Stella.Say("Where did this wind chill come from?");

            Raelin.FaceRight();
            Raelin.TransitionSprite(Raelin.GetSprite("Raelin_1"));
            Raelin.TransitionSprite(Raelin.GetSprite("Raelin_11"), layer: 1);
            Raelin.MoveToPosition(new Vector2(0.1f,0));
            Raelin.Animate("Shiver", true);
            Stella.Animate("Hop");
            yield return Raelin.Say("I din't know - but I hate it!{a} It's freezing!");

            Stella.TransitionSprite(Stella.GetSprite("Raelin_13"), layer:1);
            yield return Stella.Say("Oh, it's over!");

            Raelin.TransitionSprite(Raelin.GetSprite("Raelin_2"));
            Raelin.TransitionSprite(Raelin.GetSprite("Raelin_13"), layer:1);
            Raelin.Animate("Shiver", false);
            yield return Raelin.Say("Thank the lord...{a} I'm not wearing enough clothes for that crap.");

            yield return null;




        /*캐릭터 ui 우선순위 솔팅
            yield return new WaitForSeconds(1);

            Raelin.SetPriority(8); // 숫자가 클수록 우선순위가 높아짐
            Stella.SetPriority(15);
            Guard.SetPriority(1000);

            yield return new WaitForSeconds(1);

            CharacterManager.instance.SortCharacters(new string[] { "Stella", "Raelin"});

            yield return new WaitForSeconds(1);

            CharacterManager.instance.SortCharacters();

            yield return new WaitForSeconds(1);

            CharacterManager.instance.SortCharacters(new string[] { "Raelin", "Stella", "Guard"});
            

            yield return null;
*/


/*대화 하이라이트와 플립핑
            //Guard.Show();
            Stella.SetPosition(new Vector2(1, 0));
            Raelin.SetPosition(Vector2.zero);

            yield return new WaitForSeconds(1);

            yield return Raelin.Flip(0.3f);

            yield return Stella.FaceRight(immediate: true);

            yield return Raelin.FaceLeft(immediate: true);

            Stella.UnHighlight();
            yield return Raelin.Say("hello.");

            Raelin.UnHighlight();
            Stella.Highlight();
            yield return Stella.Say("hello. too");

            Raelin.Highlight();
            Stella.UnHighlight();
            yield return Raelin.Say("hello. too too");

            Raelin.UnHighlight();
            Stella.Highlight();
            yield return Stella.Say("hello. too too too");

            Raelin.Highlight();
            Stella.UnHighlight();
            Raelin.TransitionSprite(Raelin.GetSprite("Raelin_9"), layer: 1);
            yield return Raelin.Say("yas baby");

            yield return null;
*/

/*바디와 페이스 바꾸는 법
            yield return new WaitForSeconds(1);

            Sprite body = Raelin.GetSprite("Raelin_2");
            Sprite face = Raelin.GetSprite("Raelin_4");
            
            Raelin.TransitionSprite(body);
            yield return  Raelin.TransitionSprite(face, 1);

            yield return new WaitForSeconds(1);

            yield return Raelin.TransitionSprite(Raelin.GetSprite("Raelin_10"), 1);

            Debug.Log($"{Raelin.isVisible}");

            yield return null;
            */
            /*각 대화 색과 폰트 바꾸는 법
            Character Elen = CharacterManager.instance.CreateCharacter("Elen");
            Character Adam = CharacterManager.instance.CreateCharacter("Adam");
            Character Ben = CharacterManager.instance.CreateCharacter("Benjamin");
            List<string> lines = new List<string>()
            {
                "\"Hi!\"",
                "This is a line",
                "And another",
                "And a last one."
            };
            yield return Elen.Say(lines);

            Elen.SetNameColor(Color.red);
            Elen.SetDialogueColor(Color.green);
            Elen.SetNameFont(tempFont);
            Elen.SetDialogueFont(tempFont);

            yield return Elen.Say(lines);


            lines = new List<string>()
            {
                "iam ironman"
            };
            yield return Adam.Say(lines);

            yield return Ben.Say("fuck");

            Debug.Log("Finished");
            */
        }





        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

