using System.Collections;
using System.Collections.Generic;
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
            
            Character guard1 = CreateCharacter("Guard1 as Generic");
            Character guard2 = CreateCharacter("Guard2 as Generic");
            Character guard3 = CreateCharacter("Guard3 as Generic");

            guard1.SetPosition(Vector2.zero);
            guard2.SetPosition(new Vector2(0.5f, 0.5f));
            guard3.SetPosition(Vector2.one);

            guard2.Show();
            guard3.Show();
            yield return guard1.Show();

            yield return guard1.MoveToPosition(Vector2.one,  smooth: true);
            yield return guard1.MoveToPosition(Vector2.zero,  smooth: true);

            yield return null;
            /*
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

