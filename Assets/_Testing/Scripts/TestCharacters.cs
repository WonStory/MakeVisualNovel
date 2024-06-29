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
        
            yield return new WaitForSeconds(1);

            Raelin.SetPriority(1);

            yield return null;



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

