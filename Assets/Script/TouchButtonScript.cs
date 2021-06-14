using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButtonScript : MonoBehaviour
{
    [SerializeField]
    private GameObject Character_TopLeft;
    [SerializeField]
    private GameObject Character_TopRight;
    [SerializeField]
    private GameObject Character_BottomLeft;
    [SerializeField]
    private GameObject Character_BottomRight;

    // 인트로 화면에서 터치하면 메인 화면으로 진입
    public void IntroButtonTouched()
    {
        ManagingScript.instance.MainScene();
    }

    // 각 캐릭터 점프버튼
    public void JumpButton_TopLeft()
    {
        Character_TopLeft.GetComponent<CharacterScript>().isJumping = true;
    }
    public void JumpButton_TopRight()
    {
        Character_TopRight.GetComponent<CharacterScript>().isJumping = true;
    }
    public void JumpButton_BottomLeft()
    {
        Character_BottomLeft.GetComponent<CharacterScript>().isJumping = true;
    }
    public void JumpButton_BottomRight()
    {
        Character_BottomRight.GetComponent<CharacterScript>().isJumping = true;
    }

    // 음소거 On & Off
    public void Mute()
    {
        ManagingScript.instance.Mute_Sound();
    }

    // 인트로 화면으로 이동
    public void ToMain()
    {
        ManagingScript.instance.IntroScene();
    }

    // 게임 종료
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void EndingCreditSkip()
    {
        ManagingScript.instance.IntroScene();
    }
}
