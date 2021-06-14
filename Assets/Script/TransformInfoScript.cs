using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformInfoScript : MonoBehaviour
{
    [HideInInspector]
    public Vector3 positionPivot;
    [HideInInspector]
    public Vector3 scalePivot;
    [HideInInspector]
    public int rotationPivot;

    [SerializeField]
    private GameObject Character;
    [SerializeField]
    private GameObject ObstacleCreator;
    
    private void Awake()
    {
        Character.SetActive(false);
        ObstacleCreator.SetActive(false);
    }

    // 장애물, 캐릭터, 배경 오브젝트가 화면분할과 회전에 따라 알맞은 transform값을 갖도록 값을 설정
    private void OnEnable()
    {
        transform.position = positionPivot;
        transform.localScale = scalePivot;

        ObstacleCreator.GetComponent<ObstacleCreatorScript>().positionPivot = positionPivot;
        ObstacleCreator.GetComponent<ObstacleCreatorScript>().scalePivot = scalePivot;
        ObstacleCreator.GetComponent<ObstacleCreatorScript>().rotationPivot = rotationPivot;

        Character.GetComponent<JumpScript>().positionPivot = positionPivot;
        Character.GetComponent<JumpScript>().scalePivot = scalePivot;
        Character.GetComponent<JumpScript>().rotationPivot = rotationPivot;

        Character.GetComponent<CharacterScript>().positionPivot = positionPivot;
        Character.GetComponent<CharacterScript>().scalePivot = scalePivot;
        Character.GetComponent<CharacterScript>().rotationPivot = rotationPivot;

        ObstacleCreator.SetActive(true);
        Character.SetActive(true);
    }

    private void OnDisable()
    {
        ObstacleCreator.SetActive(false);
        Character.SetActive(false);
    }
}
