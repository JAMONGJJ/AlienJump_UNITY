using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [HideInInspector]
    public int rotationPivot;
    [HideInInspector]
    public float outofScreen;
    [HideInInspector]
    public Vector3 resetPosition;
    [HideInInspector]
    public Vector3 obstacleDirection;
    [HideInInspector]
    public string tagName;
    [HideInInspector]
    public float speed;
    public static bool clearFlag = false;
    public static bool pauseFlag = false;

    // 초기화 작업
    private void OnEnable()
    {
        transform.position = resetPosition;
    }

    // 초기화 작업
    private void OnDisable()
    {
        transform.position = resetPosition;
    }

    // 장애물 위치 갱신
    private void FixedUpdate()
    {
        if (!pauseFlag)
        {
            transform.position += obstacleDirection * speed * Time.deltaTime;
            if (transform.position.x > outofScreen && (rotationPivot == 2 || rotationPivot == 3) || // 장애물이 화면에서 벗어날 경우
                transform.position.x < outofScreen && (rotationPivot == 0 || rotationPivot == 1))
            {
                gameObject.SetActive(false);
            }
        }
    }

    // 충돌 감지
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == tagName && !clearFlag)
        {
            other.gameObject.GetComponent<CharacterScript>().ActivateCollidedScript();
        }
    }
}
