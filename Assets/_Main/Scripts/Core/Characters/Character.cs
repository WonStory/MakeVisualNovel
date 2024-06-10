using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace CHARACTERS
{
    public abstract class Character
    {
        public string name = "";
        public string displayname = "";
        public RectTransform root = null; //각 캐릭터마다 다른 특징을 보유해야된다.
        public CharacterConfigData config; //구성하는 데이터가 여기에 저장시키고 방출시킴
        public Animator animator;

        protected CharacterManager manager => CharacterManager.instance; //꽤 많이 참조할 것이므로 바로가기를 만들어준다.

        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        //코루틴, 프로텍트는 상속받은 곳에만 쓰임
        protected Coroutine co_revealing, co_hiding;
        public bool isReaveling => co_revealing != null;
        public bool isHiding => co_hiding != null;
        public virtual bool isVisible => false;

        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            displayname = name;
            this.config = config;

            if (prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, manager.characterPanel);
                ob.SetActive(true);
                root = ob.GetComponent<RectTransform>();
                animator = root.GetComponentInChildren<Animator>();//애니메이션 구성요소를 할당하고 가져옴
            }
        }

        public Coroutine Say(string dialogue) => Say(new List<string> {dialogue});
        public Coroutine Say(List<string> dialogue)
        {
            dialogueSystem.ShowSpeakerName(displayname); //화자 이름
            UpdateTextCustomizationsOnScreen(); //화자 데이터를 사용자정의 해준다.
            return dialogueSystem.Say(dialogue);
        }

        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueColor(Color color) => config.dialogueColor = color;

        public void ResetConfigurationData() => config = CharacterManager.instance.GetCharacterConfig(name); //저장된 인스턴스를 다시 불러와서 저장한다.
        public void UpdateTextCustomizationsOnScreen() => dialogueSystem.ApplySpeakerDataToDialogueContainer(config);

        public virtual Coroutine Show()//캐릭터의 페이드인 페이드 아웃을 나타낸다.
        {
            if (isReaveling)
            {
                return co_revealing;
            }

            if (isHiding)
            {
                manager.StopCoroutine(co_hiding);
            }

            co_revealing = manager.StartCoroutine(ShowingOrHiding(true));

            return co_revealing;
        }

        public virtual Coroutine Hide()
        {
            if (isHiding)
            {
                return co_hiding;
            }
            if (isReaveling)
            {
                manager.StopCoroutine(co_revealing);
            }

            co_hiding = manager.StartCoroutine(ShowingOrHiding(false));

            return co_hiding;
        }

        public virtual IEnumerator ShowingOrHiding(bool show) //현재 숨겨져있는지 나타나있는지 확인하는것
        {
            Debug.Log("Show/hide cannot be called from a base character type");//기본문자유형에서 호출 불가능
            yield return null;

        }

        public enum CharacterType
        {
            Text,
            Sprite,
            SpriteSheet,
            Live2D,
            Model3D,
        }


    }
}

