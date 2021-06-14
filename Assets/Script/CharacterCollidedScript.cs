using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollidedScript : MonoBehaviour
{
    private SpriteRenderer sr;

    // Start is called before the first frame update
    private void OnEnable()
    {
        ManagingScript.instance.Play_Collide();
        ManagingScript.instance.Pause();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine("Blink");
    }
    // 충돌 시 캐릭터 깜빡깜빡
    IEnumerator Blink()
    {
        for (int i = 0; i < 20; i++)
        {
            var tmp = sr.color;
            tmp.a = 1 - tmp.a;
            sr.color = tmp;
            yield return new WaitForSeconds(0.1f);
        }
        ManagingScript.instance.Resume();
        ManagingScript.instance.GameOverScene();
        GetComponent<CharacterScript>().DeactivateCollidedScript();
    }

    private void OnDisable()
    {
        var tmp = sr.color;
        tmp.a = 1.0f;
        sr.color = tmp;
    }
}
