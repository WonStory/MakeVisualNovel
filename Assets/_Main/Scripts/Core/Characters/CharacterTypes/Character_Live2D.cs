using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    public class Character_Live2D : Character
    {
        public Character_Live2D(string name, CharacterConfigData config, GameObject prefab) : base(name, config, prefab) //자체생성자 문자, 이걸 호출할 때마다 할당하는 기본 생성자를 호출한다.
        {
            Debug.Log($"Created Live2D Character: '{name}'");
        }
    }
}
