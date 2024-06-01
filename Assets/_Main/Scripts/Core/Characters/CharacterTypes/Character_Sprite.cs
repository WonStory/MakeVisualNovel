using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    public class Character_Sprite : Character
    {
        public Character_Sprite(string name) : base(name) //자체생성자 문자, 이걸 호출할 때마다 할당하는 기본 생성자를 호출한다.
        {
            Debug.Log($"Created Sprite Character: '{name}'");
        }
    }
}