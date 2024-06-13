using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CHARACTERS
{
    public class Character_Sprite : Character
    {
        private const string SPRITE_RENDERERD_PARENT_NAME = "Renderers";
        private const string SPRITESHEET_DEFAULT_SHEETNAME = "Default";
        private const char SPRITESHEET_TEX_SPRITE_DELIMITTER = '-';
        private CanvasGroup rootCG => root.GetComponent<CanvasGroup>();

        public List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();

        private string artAssetsDirectory = "";

        public override bool isVisible 
        {
            get { return isReaveling || rootCG.alpha == 1; } //알파나 리빌링을 통해 보이고 있는지 판단가능하게 한다.
            set { rootCG.alpha = value ? 1 : 0; } //나중에 isvisible을 폴스로 설정해놓으면 show를 할 때까지 가려져있는다.
        }
        public Character_Sprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetFolder) : base(name, config, prefab) //자체생성자 문자, 이걸 호출할 때마다 할당하는 기본 생성자를 호출한다.
        {
            rootCG.alpha = ENABLE_ON_START ? 1 : 0; //show()를 쓰면 강제로 보이므로 같이 쓸 때 주의
            artAssetsDirectory = rootAssetFolder + "/Images";
            
            GetLayers();

            Debug.Log($"Created Sprite Character: '{name}, {prefab}, {artAssetsDirectory}, {rootCG.alpha}'");
        }

        private void GetLayers() //캐릭터 레이어생성
        {
            Transform rendererRoot = animator.transform.Find(SPRITE_RENDERERD_PARENT_NAME);//이름으로 찾기

            if (rendererRoot == null)
            {
                return;
            }

            for (int i = 0; i < rendererRoot.transform.childCount; i++) //반복해서 하위요소를 다 찾는다.
            {
                Transform child = rendererRoot.transform.GetChild(i);

                Image rendererImage = child.GetComponentInChildren<Image>(); //레이어를 강제로 지정해줫으므로(안그러면 페이스와 몸의 순서가 바꼈을 때 페이스가 몸에 가려진다.)

                if (rendererImage != null) //널이 아니면 추가한다.
                {
                    CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage, i); //이미지와 번호
                    layers.Add(layer);
                    child.name = $"Layer: {i}"; //레이어를 번호로 통일

                }
            }
        }

        public void SetSprite(Sprite sprite, int layer = 0)
        {
            layers[layer].SetSprite(sprite);
        }

        public Sprite GetSprite(string spriteName)
        {
            if (config.characterType == CharacterType.SpriteSheet)
            {
                
                string[] data = spriteName.Split(SPRITESHEET_TEX_SPRITE_DELIMITTER);
                Sprite[] spriteArray = new Sprite[0];

                if (data.Length == 2)
                {
                    string texturename = data[0];
                    spriteName = data[1];
                    spriteArray = Resources.LoadAll<Sprite>($"{artAssetsDirectory}/{texturename}");

                    if (spriteArray.Length == 0)
                    {
                        Debug.LogWarning($"Character '{name}' does not have an default art asset called '{texturename}'");
                    }

                    return Array.Find(spriteArray, sprite => sprite.name == spriteName);
                }
                else
                {
                    spriteArray = Resources.LoadAll<Sprite>($"{artAssetsDirectory}/{SPRITESHEET_DEFAULT_SHEETNAME}");
                }
                if (spriteArray.Length == 0)
                    {
                        Debug.LogWarning($"Character '{name}' does not have a default art asset called '{SPRITESHEET_DEFAULT_SHEETNAME}'");
                    }
                    
                return Array.Find(spriteArray, sprite => sprite.name == spriteName);
            }
            else
            {
                return Resources.Load<Sprite>($"{artAssetsDirectory}/{spriteName}");
            }
            
        }

        public Coroutine TransitionSprite(Sprite sprite, int layer = 0, float speed = 1) //시간이 지남에 따라 스프라이트를 교체해야된다. 하나의 이미지를 점진적으로 바꾸는 것
        { //실제 적용할때 얼굴만 바꾼다 치면 레이어를 1로 설정해야 몸도 살아있음
            CharacterSpriteLayer spriteLayer = layers[layer];

            return spriteLayer.TransitionSprite(sprite, speed);
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

        public override void SetColor(Color color)
        {
            base.SetColor(color);

            foreach (CharacterSpriteLayer layer in layers)
            {
                layer.SetColor(color);
            }
        }

        public override IEnumerator ChangingColor(Color color, float speed)
        {
            foreach (CharacterSpriteLayer layer in layers)
            {
                layer.TransitionColor(color,speed);
            }

            yield return null;

            while (layers.Any(l => l.isChangingColor))
            {
                yield return null;
            }

            co_changeingColor = null;
        }
    }
}