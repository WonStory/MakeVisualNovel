using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COMMANDS
{
    public class CMD_DatabaseExtension_General : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("wait", new Func<string, IEnumerator>(Wait));
        }

        public static IEnumerator Wait(string data) //시간의 대한 값만 필요함
        {
            if (float.TryParse(data, out float time)) //가능하면 참을 반환하면서 변환하고 불가능하면 거짓을 반환
            {
                yield return new WaitForSeconds(time);
            }
        }
        
    }
}

