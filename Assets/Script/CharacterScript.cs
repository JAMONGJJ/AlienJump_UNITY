using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    [HideInInspector]
    public Vector3 positionPivot;
    [HideInInspector]
    public Vector3 scalePivot;
    [HideInInspector]
    public int rotationPivot;
    [HideInInspector]
    public bool isJumping;
    public static bool pauseFlag = false;
    public static int extraLife = 1;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float startPosition_x = 5.0f;
    private float startPostion_y = 2.3f;

    // 초기화 작업
    private void OnEnable()
    {
        switch (rotationPivot)
        {
            case 0:
                startPosition = new Vector3(-startPosition_x * scalePivot.x + positionPivot.x, -startPostion_y * scalePivot.y + positionPivot.y, 0.0f);
                startRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
            case 1:
                startPosition = new Vector3(-startPosition_x * scalePivot.x + positionPivot.x, startPostion_y * scalePivot.y + positionPivot.y, 0.0f);
                startRotation = Quaternion.Euler(new Vector3(180.0f, 0, 0));
                break;
            case 2:
                startPosition = new Vector3(startPosition_x * scalePivot.x + positionPivot.x, startPostion_y * scalePivot.y + positionPivot.y, 0.0f);
                startRotation = Quaternion.Euler(new Vector3(0, 0, 180.0f));
                break;
            case 3:
                startPosition = new Vector3(startPosition_x * scalePivot.x + positionPivot.x, -startPostion_y * scalePivot.y + positionPivot.y, 0.0f);
                startRotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
                break;
            default:
                break;
        }
        GetComponent<JumpScript>().enabled = false;
        GetComponent<CharacterCollidedScript>().enabled = false;
        isJumping = false;
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    private void OnDisable()
    {
        GetComponent<JumpScript>().enabled = false;
        GetComponent<CharacterCollidedScript>().enabled = false;
    }

    private void FixedUpdate()
    {
        if (isJumping && !pauseFlag)    // 점프키 눌렸을 경우 점프 스크립트 활성화
        {
            isJumping = true;
            ActivateJumpScript();
        }
        if(GetComponent<Animator>().enabled == pauseFlag)
            GetComponent<Animator>().enabled = !pauseFlag;
    }

    // 점프 시 해당 스크립트 활성화
    public void ActivateJumpScript()
    {
        GetComponent<JumpScript>().enabled = true;
    }

    public void DeactivateJumpScript()
    {
        GetComponent<JumpScript>().enabled = false;
    }

    // 충돌 시 해당 스크립트 활성화
    public void ActivateCollidedScript()
    {
        GetComponent<CharacterCollidedScript>().enabled = true;
    }

    public void DeactivateCollidedScript()
    {
        GetComponent<CharacterCollidedScript>().enabled = false;
    }
}
