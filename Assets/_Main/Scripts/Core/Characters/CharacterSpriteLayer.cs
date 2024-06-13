using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CHARACTERS
{
    public class CharacterSpriteLayer
    {
        private CharacterManager characterManager => CharacterManager.instance; //모노비해이어 없이는 코루틴을 할 수 없으므로 참조받는다.

        private const float DEFAULT_TRANSITION_SPEED = 3F;
        private float transitionSpeedMultiplier = 1;

        public int layer { get; private set; } = 0; //레이어 번호를 알고 싶다.(추적하고싶다)
        public Image renderer { get; private set; } = null;
        public CanvasGroup rendererCG => renderer.GetComponent<CanvasGroup>(); //이미지를 쉽게 페이드인 아웃할 수 있게 캔버스 그룹에 대한 포인터를 만들어둠.

        private List<CanvasGroup> oldRenderers = new List<CanvasGroup>();

        private Coroutine co_transitioningLayer = null; //장면을 스킾할때마다 코루틴을 계속 실행하면 최악이므로 너무 많은 코루틴을 건너뛰지 않도록한다.
        private Coroutine co_levelingAlpha = null;
        private Coroutine co_changingColor = null;
        public bool isTransitioningLayer => co_transitioningLayer != null;
        public bool isLevelingAlpha => co_levelingAlpha != null;
        public bool isChangingColor => co_changingColor != null;

        public CharacterSpriteLayer (Image defaultRenderer, int layer = 0) //캐릭터에 대한 각레이어를 추가하기 쉽게 생성자를 만듬
        {
            renderer = defaultRenderer;
            this.layer = layer;
        }

        public void SetSprite(Sprite sprite)
        {
            renderer.sprite = sprite; //렌더러의 스프라이트로 적용시킨다.
        }

        public Coroutine TransitionSprite(Sprite sprite, float speed = 1)
        {
            if (sprite == renderer.sprite)
            {
                return null;
            }

            if (isTransitioningLayer)
            {
                characterManager.StopCoroutine(co_transitioningLayer);
            }

            co_transitioningLayer = characterManager.StartCoroutine(TransitioningSprite(sprite, speed));

            return co_transitioningLayer;
        }

        private IEnumerator TransitioningSprite(Sprite sprite, float speedMultiplier) //스프라이트 전환 속도.
        {
            transitionSpeedMultiplier = speedMultiplier;

            Image newRenderer = CreateRenderer(renderer.transform.parent); //새로운 렌더러
            newRenderer.sprite = sprite;

            yield return TryStartLevelingAlphas();

            co_transitioningLayer = null;
        }

        private Image CreateRenderer(Transform parent) //비공개로 이미지 유형 전달
        {
            Image newRenderer = Object.Instantiate(renderer, parent); //부모의 부모가 되길 원한다.
            oldRenderers.Add(rendererCG);

            newRenderer.name = renderer.name;
            renderer = newRenderer;
            renderer.gameObject.SetActive(true);
            rendererCG.alpha = 0;

            return newRenderer;
        }

        private Coroutine TryStartLevelingAlphas()
        {
            if (isLevelingAlpha) //이미활성화중이면 활성화중인거 반환
            {
                return co_levelingAlpha;
            }

            co_levelingAlpha = characterManager.StartCoroutine(RunAlphaLeveling());

            return co_levelingAlpha;
        }

        private IEnumerator RunAlphaLeveling()
        {
            while (rendererCG.alpha < 1 || oldRenderers.Any(oldCG => oldCG.alpha > 0)) //이전것 페이드아웃중인가, 즉 현재 이미지가 밝혀지는 중이거나 이전 렌더러 중에 하나라도 밝혀져있으면 이 루프를 실행한다.
            {
                float speed = DEFAULT_TRANSITION_SPEED * transitionSpeedMultiplier * Time.deltaTime;

                rendererCG.alpha = Mathf.MoveTowards(rendererCG.alpha, 1, speed);

                for (int i = oldRenderers.Count - 1; i >= 0; i--)//배열의 시작이 아닌 끝부터 하는 이유는 항목을 변경한다면 바로 항목의 개수가 변하므로 i를 처음부터 재는것보다 낫다.
                {
                    CanvasGroup oldCG = oldRenderers[i];
                    oldCG.alpha = Mathf.MoveTowards(oldCG.alpha, 0, speed);

                    if (oldCG.alpha <= 0)
                    {
                        oldRenderers.RemoveAt(i);
                        Object.Destroy(oldCG.gameObject);
                    }
                }

                yield return null;
            }

            co_levelingAlpha = null; //코루틴이 종료되었음을 레이어전환에 널을 할당함으로써 함.
        }


        public void SetColor(Color color)
        {
            renderer.color = color;

            foreach (CanvasGroup oldCG in oldRenderers)
            {
                oldCG.GetComponent<Image>().color = color;
            }
        }

        public Coroutine TransitionColor(Color color, float speed)
        {
            if (isChangingColor)
            {
                characterManager.StopCoroutine(co_changingColor);
            }

            co_changingColor = characterManager.StartCoroutine(ChangingColor(color, speed));

            return co_changingColor;
        }

        private IEnumerator ChangingColor(Color color, float speedMultiplier) //점진적으로 색상을 바꾸는 과정
        {
            Color oldColor = renderer.color;
            List<Image> oldImages = new List<Image>();

            foreach (var oldCG in oldRenderers)
            {
                oldImages.Add(oldCG.GetComponent<Image>());
            }

            float colorPercent = 0;
            while (colorPercent < 1)
            {
                colorPercent += DEFAULT_TRANSITION_SPEED * speedMultiplier * Time.deltaTime;

                renderer.color = Color.Lerp(oldColor, color, colorPercent);

                foreach (Image oldImage in oldImages)
                {
                    oldImage.color = renderer.color;
                }

                yield return null;
            }

            co_changingColor = null;
        }


    }

}
