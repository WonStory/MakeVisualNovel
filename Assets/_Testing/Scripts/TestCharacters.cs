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
            yield return new WaitForSeconds(1f);

            Character stella = CharacterManager.instance.CreateCharacter("Stella");

            yield return new WaitForSeconds(1f);

            yield return stella.Hide();

            yield return new WaitForSeconds(0.5f);

            yield return stella.Show();

            yield return stella.Say("Hello!");

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

