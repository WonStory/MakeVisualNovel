using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    public class Character_Sprite : Character
    {
        private CanvasGroup rootCG => root.GetComponent<CanvasGroup>();

        public Character_Sprite(string name, CharacterConfigData config, GameObject prefab) : base(name, config, prefab) //자체생성자 문자, 이걸 호출할 때마다 할당하는 기본 생성자를 호출한다.
        {
            rootCG.alpha = 0;
            Show();
            Debug.Log($"Created Sprite Character: '{name}, {prefab}'");
        }

        public override IEnumerator ShowingOrHiding(bool show)
        {
            float tarGetAlpha = show ? 1f : 0;
            CanvasGroup self = rootCG;

            while (self.alpha != tarGetAlpha)
            {
                self.alpha = Mathf.MoveTowards(self.alpha, tarGetAlpha, 3f * Time.deltaTime);
                yield return null;
            }

            co_revealing = null;
            co_hiding = null;
        }
    }
}