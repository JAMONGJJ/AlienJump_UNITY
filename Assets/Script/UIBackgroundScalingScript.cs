using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBackgroundScalingScript : MonoBehaviour
{
    [SerializeField]
    private GameObject IntroBackground;
    [SerializeField]
    private GameObject IntroText;
    [SerializeField]
    private GameObject IntroButton;

    [SerializeField]
    private GameObject MainBackground1;
    [SerializeField]
    private GameObject MainBackground2;

    [SerializeField]
    private GameObject GameOverBackground;
    [SerializeField]
    private GameObject GameOverText;
    [SerializeField]
    private GameObject GameOverButton;

    [SerializeField]
    private GameObject EndingCreditBackground;
    [SerializeField]
    private GameObject EndingCreditText;

    [SerializeField]
    private GameObject SignatureBackground1;
    [SerializeField]
    private GameObject SignatureBackground2;
    [SerializeField]
    private GameObject SignatureText;

    [SerializeField]
    private GameObject SettingBackground;

    [SerializeField]
    private GameObject TutorialBackground;

    [SerializeField]
    private GameObject TwoDivisionImage;
    [SerializeField]
    private GameObject FourDivisionImage;

    private Vector2 screenRatio = new Vector2(Screen.width, Screen.height);
    public static Vector2 referenceRatio = new Vector2();
    public static Vector2 calculatedRatio = new Vector2();

    // UI elements 중에 해상도에 따라 크기가 바뀌어야 할 것들의 sizeDelta값을 변경
    private void Awake()
    {
        referenceRatio = GetComponent<CanvasScaler>().referenceResolution;
        calculatedRatio = GetComponent<CanvasScaler>().referenceResolution;
        ResolutionRatioChange();

        IntroBackground.GetComponent<RectTransform>().sizeDelta = calculatedRatio;
        IntroText.GetComponent<RectTransform>().sizeDelta = calculatedRatio;
        IntroButton.GetComponent<RectTransform>().sizeDelta = calculatedRatio;

        MainBackground1.GetComponent<RectTransform>().sizeDelta = calculatedRatio;
        MainBackground2.GetComponent<RectTransform>().sizeDelta = calculatedRatio;

        GameOverBackground.GetComponent<RectTransform>().sizeDelta = calculatedRatio;
        GameOverText.GetComponent<RectTransform>().sizeDelta = calculatedRatio;
        GameOverButton.GetComponent<RectTransform>().sizeDelta = calculatedRatio;

        EndingCreditBackground.GetComponent<RectTransform>().sizeDelta = calculatedRatio;
        EndingCreditText.GetComponent<RectTransform>().sizeDelta = new Vector2(calculatedRatio.x, EndingCreditText.GetComponent<RectTransform>().sizeDelta.y);

        SignatureBackground1.GetComponent<RectTransform>().sizeDelta = calculatedRatio;
        SignatureBackground2.GetComponent<RectTransform>().sizeDelta = calculatedRatio;
        SignatureText.GetComponent<RectTransform>().sizeDelta = new Vector2(calculatedRatio.x, calculatedRatio.y * 0.5f);

        SettingBackground.GetComponent<RectTransform>().sizeDelta = calculatedRatio;

        TutorialBackground.GetComponent<RectTransform>().sizeDelta = calculatedRatio;

        TwoDivisionImage.GetComponent<RectTransform>().sizeDelta = calculatedRatio;
        FourDivisionImage.GetComponent<RectTransform>().sizeDelta = calculatedRatio;

        State2Script.Width = calculatedRatio.x;
        State2Script.Height = calculatedRatio.y;
        State2Script.objectResolutionRatio = (referenceRatio.y * screenRatio.x) / (referenceRatio.x * screenRatio.y);
    }

    // referenceRatio값과 screenRatio값을 이용해 둘에 차이가 생길 경우 실제 UI값들에 적용될 calculatedRatio값을 계산
    private void ResolutionRatioChange()
    {
        if (referenceRatio.x / referenceRatio.y > screenRatio.x / screenRatio.y)
        {
            calculatedRatio.y *= (referenceRatio.x * screenRatio.y) / (referenceRatio.y * screenRatio.x);    // 너비 고정 후 높이 조절
        }
        else if (referenceRatio.x / referenceRatio.y < screenRatio.x / screenRatio.y)
        {
            calculatedRatio.x *= (referenceRatio.y * screenRatio.x) / (referenceRatio.x * screenRatio.y);     // 높이 고정 후 너비 조절
        }
        else { }
    }
}
