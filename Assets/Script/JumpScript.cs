using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    [HideInInspector]
    public Vector3 positionPivot;
    [HideInInspector]
    public Vector3 scalePivot;
    [HideInInspector]
    public int rotationPivot;
    public static bool pauseFlag = false;
    public static float jumpResolutionRatio;

    private float jumpSpeed;
    private float jumpMaxHeight;
    private int fps;
    private int currentFrame;
    private Stack<float> jumpAmount = new Stack<float>();
    private Vector3 jumpDirection;
    private CharacterScript characterScript;

    private void Awake()
    {
        characterScript = GetComponent<CharacterScript>();
    }

    // 초기화 작업
    private void OnEnable()
    {
        ManagingScript.instance.Play_Jump();
        jumpSpeed = 3.0f;
        fps = (int)(1.0f / Time.deltaTime * 0.7f);
        currentFrame = 0;
        switch (rotationPivot)
        {
            case 0:
            case 3:
                jumpMaxHeight = 3.0f;
                jumpDirection = Vector3.up;
                break;
            case 1:
            case 2:
                jumpMaxHeight = -3.0f;
                jumpDirection = Vector3.down;
                break;
        }
    }

    private void OnDisable()
    {
        if(!ManagingScript.tutorialDone)
        {
            ManagingScript.tutorialDone = true;
            ManagingScript.SaveData.TutorialDone = true;
            ManagingScript.instance.Tutorial_active(false);
            ManagingScript.instance.TutorialFinished(true);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(!pauseFlag)
        {
            if (currentFrame < fps) // 위로 올라가는 구간
            {
                // 올라간 거리와 같은 거리를 내려오기 위해 Stack에 값을 저장
                float tmp = jumpSpeed * Time.deltaTime * Mathf.Abs(jumpMaxHeight * scalePivot.y + positionPivot.y - transform.position.y);
                jumpAmount.Push(tmp);
                transform.position += jumpDirection * tmp;
                currentFrame++;
            }
            else if (currentFrame < fps * 2)    // 아래로 떨어지는 구간
            {
                transform.position -= jumpDirection * jumpAmount.Pop();
                currentFrame++;
            }
            else    // 점프가 끝나고 초기화
            {
                currentFrame = 0;
                characterScript.isJumping = false;
                characterScript.DeactivateJumpScript();
            }
        }
    }
}
