using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    public class Character_Model3D : Character
    {
        public Character_Model3D(string name, CharacterConfigData config, GameObject prefab, string rootAssetFolder) : base(name, config, prefab) //자체생성자 문자, 이걸 호출할 때마다 할당하는 기본 생성자를 호출한다.
        {
            Debug.Log($"Created Model3D Character: '{name}'");
        }
    }
}

