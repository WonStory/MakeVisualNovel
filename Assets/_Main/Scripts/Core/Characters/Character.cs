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
        protected Coroutine co_moving;
        public bool isReaveling => co_revealing != null;
        public bool isHiding => co_hiding != null;
        public bool isMoving => co_moving != null;
        public virtual bool isVisible => false;

        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            displayname = name;
            this.config = config;

            if (prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, manager.characterPanel);
                ob.name = manager.FormatCharacterPath(manager.characterPrefabNameFormat, name);//리네이밍하는단계, 하지만 두번 이름을 참조하므로 안좋다.
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

        public virtual void SetPosition(Vector2 position)
        {
            if (root == null)
            {
                return;
            }

            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPosionToRelativeCharacterAnchorTargets(position);

            root.anchorMin = minAnchorTarget;
            root.anchorMax = maxAnchorTarget;
        }

        public virtual Coroutine MoveToPosition(Vector2 position, float speed = 2f, bool smooth = false)
        {
            if (root == null)
            {
                return null; //뿌리에 아무것도 없으면 널을 반환
            }

            if (isMoving)//위치이동을 할 때 움직이고 있나를 판단하고 코루틴을 중지시킴
            {
                manager.StopCoroutine(co_moving);
            }

            co_moving = manager.StartCoroutine(MovingToPosition(position, speed, smooth));

            return co_moving;
        } 

        private IEnumerator MovingToPosition(Vector2 position, float speed, bool smooth) //위랑 기본적으로 같은 매개변수를 사용
        {
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPosionToRelativeCharacterAnchorTargets(position);
            Vector2 padding = root.anchorMax - root.anchorMin;

            float t = 1f / 20f;// 원본은 t를 speed * deltatime으로 고치면댐.
            t = t*t*(3f - 2f *t);

            while (root.anchorMin != minAnchorTarget || root.anchorMax != maxAnchorTarget)
            {   //스무스 존재여부에 따라 러프를 적용하면 끝에서 비선형적인 속도로 동작함
                root.anchorMin = smooth ? Vector2.Lerp(root.anchorMin, minAnchorTarget, t ) : Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed * Time.deltaTime * 0.35f);
            
                root.anchorMax = root.anchorMin + padding;

                if (smooth && Vector2.Distance(root.anchorMin, minAnchorTarget) <= 0.001f)
                {
                    root.anchorMin = minAnchorTarget;
                    root.anchorMax = maxAnchorTarget;

                    break;
                }

                yield return null;
            }

            Debug.Log("done moving");
            co_moving = null;

        }

        protected (Vector2, Vector2) ConvertUITargetPosionToRelativeCharacterAnchorTargets(Vector2 position) //화면밖으로 뛰쳐나가지 않게
        {
            Vector2 padding = root.anchorMax - root.anchorMin;

            float maxX = 1f - padding.x;
            float maxY = 1f - padding.y;

            Vector2 minAnchorTarget = new Vector2(maxX * position.x, maxY * position.y);
            Vector2 maxAnchorTarget = minAnchorTarget + padding; // 캐릭터 크기에 따라 이동

            return (minAnchorTarget, maxAnchorTarget);
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

