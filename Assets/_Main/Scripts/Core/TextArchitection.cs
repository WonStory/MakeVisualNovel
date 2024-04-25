using System.Collections;
using UnityEngine;
using TMPro;

public class TextArchitection
{
    private TextMeshProUGUI tmpro_ui;
    private TextMeshPro tmpro_world;

    public TMP_Text tmpro => tmpro_ui != null ? tmpro_ui : tmpro_world;

    public string currentText => tmpro.text;
    public string targetText { get; private set; } = ""; //기본값은 비어있는 배열을 만든다. 공개적으론 되어있지만 할당은 개인적으로
    public string preText { get; private set; } = "";
    private int preTextLength = 0;

    public string fullTargetText => preText + targetText;

    public enum BuildMethod { instant, typewriter, fade} //열거형, 타자기와 페이드효과
    public BuildMethod buildMethod = BuildMethod.typewriter; //일반적인 타입화이터로 설정

    public Color textcolor { get { return tmpro.color; } set { tmpro.color = value; } } //컬러효과

    public float speed {get { return baseSpeed * speedMultiplier;} set { speedMultiplier = value;}} //실제 화면에 나타나는 속도값
    private const float baseSpeed = 0.5f; //기본스피드인데 일단 플롯으로 만든다.
    private float speedMultiplier = 1; //원하는 대로 속도승수를 만들어준다.

    public int charactersPerCycle { get { return speed <= 2f ? characterMultiplier : speed <= 2.5f ? characterMultiplier * 2 : characterMultiplier * 3;}} //점점 빨라지게
    private int characterMultiplier = 1;

    public bool hurryUp = false; //마우스클릭에 따라 대사가 빨라지는 기능, 1개의 단어를 나타내는 걸 2개,3개로 나타내면 빨라진다.

    public TextArchitection(TextMeshProUGUI tmpro_ui)
    {
        this.tmpro_ui = tmpro_ui;
    }

    public TextArchitection(TextMeshPro tmpro_world)
    {
        this.tmpro_world = tmpro_world;
    }

    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    /// <summary>
    /// Append text to that is already in the text architect.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    private Coroutine buildProcess = null;
    public bool isBuilding => buildProcess != null;

    public void Stop()
    {
        if (!isBuilding)
            return;
        
        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;
    }

    IEnumerator Building()
    {
        Prepare();

        switch(buildMethod)
        {
            case BuildMethod.typewriter:
                yield return Build_Typewriter();
                break;
            case BuildMethod.fade:
                yield return Build_Fade();
                break;
        }

        OnComplete();
    }

    private void OnComplete()
    {
        buildProcess = null;
        hurryUp = false;
    }

    public void ForceComplete()
    {
        switch(buildMethod)
        {
            case BuildMethod.typewriter:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;
            case BuildMethod.fade:
                break;
        }

        Stop();
        OnComplete();
    }

    private void Prepare()
    {
        switch (buildMethod)
        {
            case BuildMethod.instant:
                Prepare_Instant();
                break;
            case BuildMethod.typewriter:
                Prepare_Typewriter();
                break;
            case BuildMethod.fade:
                Prepare_Fade();
                break;
        }
    }

    private void Prepare_Instant()
    {
        tmpro.color = tmpro.color; //자체적으로 초기화하도록
        tmpro.text = fullTargetText;
        tmpro.ForceMeshUpdate(); //업데이트
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount; //최대 단어개수
    }

    private void Prepare_Typewriter()
    {
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preText;

        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }

        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();
    }

    private void Prepare_Fade()
    {
        
    }

    private IEnumerator Build_Typewriter()
    {
        while (tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters += hurryUp ? charactersPerCycle * 5 : charactersPerCycle;

            yield return new WaitForSeconds(0.015f / speed);
        }
    }

    private IEnumerator Build_Fade()
    {
        yield return null;
    }













}
