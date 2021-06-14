using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundImageScript : MonoBehaviour
{
    public static bool pauseFlag = false;

    [SerializeField]
    private float speed;
    private float offsetX;
    private Vector2 xy = new Vector2(0.0f, 0.0f);
    private Vector2 wh = new Vector2(1.0f, 1.0f);
    private RawImage image;

    private void Awake()
    {
        //speed = 0.28f;
        image = GetComponent<RawImage>();
    }

    // 초기화 작업
    private void OnEnable()
    {
        offsetX = 0.0f;
        image.uvRect = new Rect(xy, wh);
    }

    // 초기화 작업
    private void OnDisable()
    {
        offsetX = 0.0f;
        image.uvRect = new Rect(xy, wh);
    }

    private void FixedUpdate()
    {
        if(!pauseFlag)
        {
            offsetX += speed * Time.deltaTime;
            image.uvRect = new Rect(new Vector2(xy.x + offsetX, xy.y), wh);
        }
    }
}
