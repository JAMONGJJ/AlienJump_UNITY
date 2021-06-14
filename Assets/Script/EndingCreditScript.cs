using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingCreditScript : MonoBehaviour
{
    [SerializeField]
    private GameObject BackgroundImage;
    [SerializeField]
    private GameObject EndingCreditText;
    private Image BG_image;
    private RectTransform RT_text;
    private float endingCreditUpperBoundary;    // 엔딩크레딧이 올라가면서 최대로 도달할 좌표의 y값

    // 초기화 작업
    private void Awake()
    {
        EndingCreditText.SetActive(false);
        BG_image = BackgroundImage.GetComponent<Image>();
        RT_text = EndingCreditText.GetComponent<RectTransform>();
        endingCreditUpperBoundary = RT_text.sizeDelta.y * 0.5f + UIBackgroundScalingScript.calculatedRatio.y;
    }
    
    // 초기화 작업
    private void OnEnable()
    {
        ManagingScript.instance.Stop_Background();

        var tmpColor = BG_image.color;
        tmpColor.a = 0.0f;
        BG_image.color = tmpColor;

        var tmpPos = RT_text.anchoredPosition;
        tmpPos.y = -RT_text.sizeDelta.y * 0.5f;
        RT_text.anchoredPosition = tmpPos;

        StartCoroutine("FadeOut_half");
    }
    // 배경 투명도 0.5까지 페이드아웃
    IEnumerator FadeOut_half()
    {
        while (BG_image.color.a < 0.5f)
        {
            var tmpColor = BG_image.color;
            tmpColor.a += 0.01f;
            BG_image.color = tmpColor;
            yield return new WaitForSeconds(0.02f);
        }
        StartCoroutine("EndingCredit_text");
    }
    // 엔딩크레딧 출력
    IEnumerator EndingCredit_text()
    {
        EndingCreditText.SetActive(true);
        while (RT_text.anchoredPosition.y < endingCreditUpperBoundary)
        {
            var tmpPos = RT_text.anchoredPosition;
            tmpPos.y += 5.0f;
            RT_text.anchoredPosition = tmpPos;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(3.0f);
        EndingCreditText.SetActive(false);

        // 다음 상태로(시그니처 이미지 출력)
        ManagingScript.instance.SignatureImageScene();
    }

    // 초기화 작업
    private void OnDisable()
    {
        var tmpColor = BG_image.color;
        tmpColor.a = 0.0f;
        BG_image.color = tmpColor;

        var tmpPos = RT_text.anchoredPosition;
        tmpPos.y = -RT_text.sizeDelta.y * 0.5f;
        RT_text.anchoredPosition = tmpPos;

        EndingCreditText.SetActive(false);
    }
}
