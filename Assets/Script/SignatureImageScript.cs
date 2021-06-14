using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignatureImageScript : MonoBehaviour
{
    [SerializeField]
    private GameObject BackgroundImage;
    private Image BG_image;

    // 초기화 작업
    private void Awake()
    {
        BG_image = BackgroundImage.GetComponent<Image>();
        BackgroundImage.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine("Wait");
    }
    // 3초간 뜸들이기
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);
        BackgroundImage.SetActive(true);
        StartCoroutine("FadeOut_one");
    }
    // 투명도 1.0까지 페이드아웃
    IEnumerator FadeOut_one()
    {
        while (BG_image.color.a < 1.0f)
        {
            var tmpColor = BG_image.color;
            tmpColor.a += 0.1f;
            BG_image.color = tmpColor;
            yield return new WaitForSeconds(0.05f);
        }

        // 초기 상태로
        ManagingScript.instance.IntroScene();
    }

    private void OnDisable()
    {
        var tmpColor = BG_image.color;
        tmpColor.a = 0.0f;
        BG_image.color = tmpColor;

        BackgroundImage.SetActive(false);
    }
}
