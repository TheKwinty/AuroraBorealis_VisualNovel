 using System.Collections;
using UnityEngine;
using TMPro;

public class TextArchitect 
{
    private TextMeshProUGUI tmpro_ui;
    private TextMeshPro tmpro_world;
    public TMP_Text tmpro => tmpro_ui != null ? tmpro_ui : tmpro_world;//??

    public string currentText => tmpro.text;
    public string targetText { get; private set; } = "";

    public string preText { get; private set; } = "";
    private int preTextLengh = 0;

    public string fullTargetText => preText + targetText;

    public enum BuildMethod { instant, typewriter, fade}
    public BuildMethod buildMethod = BuildMethod.typewriter;

    public Color textColor { get { return tmpro.color; } set { tmpro.color = value; } }

    public float speed{ get { return baseSpeed * speedMultiplier; } set { speedMultiplier = value; } }
    private const float baseSpeed = 1;
    private float speedMultiplier = 1;//змінна яка буде налаштовуватися юзером

    //Система мультикліка коли декілька разів нажимаєш і воно прискорюється => 2 рази нажати прискорюється 3 рази показує весь текст
    //Посимвольна зміна швидкості

    //*2 - 2 символа в секунду, *3 - 3 символа в секунду
    public int charactersPerCycle { get { return speed <= 2f ? charactersMultiplier : speed <= 2.5f ? charactersMultiplier * 2 : charactersMultiplier * 3; }  }
    private int charactersMultiplier = 1;

    public bool hurryUp = false;

    //Constructor for the text architect 

    public TextArchitect(TextMeshProUGUI tmpro_ui)
    {
        this.tmpro_ui = tmpro_ui;
    }
    public TextArchitect(TextMeshPro tmpro_world)
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

    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    private Coroutine buildProcess = null;
    public bool isBuilding => buildProcess != null; //спосіб моніторингу чи текстовий архітектор вже щось будує, і якщо так то ми зупиняємо процес корутиною

    //Пізнавальний факт без MonoBeheavior наш архітектор не здатний запускати 
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
        //Instant не потребує побудовибо текст появляється моментально
        switch (buildMethod)
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

    public void ForceComplite()
    {
        switch (buildMethod)
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
        tmpro.color = tmpro.color;
        tmpro.text = fullTargetText;
        tmpro.ForceMeshUpdate();
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
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
        while(tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
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
