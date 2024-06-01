using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // Character Elen = CharacterManager.instance.CreateCharacter("Elen");
            // Character Stella = CharacterManager.instance.CreateCharacter("Stella");
            // Character Stella2 = CharacterManager.instance.CreateCharacter("Stella");
            // Character Adam = CharacterManager.instance.CreateCharacter("Adam");
            StartCoroutine(Test());
        }

        IEnumerator Test()
        {
            Character Elen = CharacterManager.instance.CreateCharacter("Elen");
            List<string> lines = new List<string>()
            {
                "\"Hi!\"",
                "This is a line",
                "And another",
                "And a last one."
            };
            yield return Elen.Say(lines);

            Debug.Log("Finished");
        }





        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

