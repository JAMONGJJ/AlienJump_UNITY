using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCreatorScript : MonoBehaviour
{
    [HideInInspector]
    public Vector3 positionPivot;
    [HideInInspector]
    public Vector3 scalePivot;
    [HideInInspector]
    public int rotationPivot;
    public static bool clearFlag = false;
    public static bool tutorialFinished;

    private List<GameObject> Obstacles;
    [SerializeField]
    private GameObject Obstacle1;
    [SerializeField]
    private GameObject Obstacle2;
    [SerializeField]
    private GameObject Obstacle3;
    private GameObject LatestObstacle;
    private ObstacleScript LatestObstacleScript;

    private float distance = 0.0f;  // 장애물 초기 생성 위치부터 가장 마지막 장애물의 위치까지의 x값의 차이
    private int ObstacleCount = 2;  // 각 종류의 장애물 갯수, 즉 총 장애물 갯수는 장애물 종류를 곱한 값
    private float outofScreen = 25.0f;
    private float resetPosition_x;
    private float resetPosition_y;
    private float speed = 5.0f;

    // 초기에 모든 장애물을 2개씩 미리 생성 후 비활성화(오브젝트 풀링)
    private void Awake()
    {
        LatestObstacle = new GameObject();
        Obstacles = new List<GameObject>();
        for (int i = 0; i < ObstacleCount; i++)
        {
            var tmp1 = Instantiate(Obstacle1);
            tmp1.GetComponent<SpriteRenderer>().sortingLayerName = gameObject.tag;
            tmp1.gameObject.transform.SetParent(gameObject.transform, false);
            Obstacles.Add(tmp1);
            tmp1.SetActive(false);

            var tmp2 = Instantiate(Obstacle2);
            tmp2.GetComponent<SpriteRenderer>().sortingLayerName = gameObject.tag;
            tmp2.gameObject.transform.SetParent(gameObject.transform, false);
            Obstacles.Add(tmp2);
            tmp2.SetActive(false);

            var tmp3 = Instantiate(Obstacle3);
            tmp3.GetComponent<SpriteRenderer>().sortingLayerName = gameObject.tag;
            tmp3.gameObject.transform.SetParent(gameObject.transform, false);
            Obstacles.Add(tmp3);
            tmp3.SetActive(false);
        }
    }

    // ObstacleScript 변수값 초기 설정
    private void OnEnable()
    {
        LatestObstacle = null;

        resetPosition_x = Random.Range(10.0f, 20.0f);
        foreach (var o in Obstacles)
        {
            switch (o.tag)
            {
                case "Obstacle1":
                    resetPosition_y = -2.4f;
                    break;
                case "Obstacle2":
                    resetPosition_y = -2.2f;
                    break;
                case "Obstacle3":
                    resetPosition_y = 1.0f;
                    break;
                default:
                    break;
            }

            ObstacleScript obstacleScript = o.GetComponent<ObstacleScript>();
            obstacleScript.rotationPivot = rotationPivot;
            obstacleScript.speed = speed * scalePivot.x;
            obstacleScript.tagName = gameObject.tag;
            switch (rotationPivot)
            {
                case 0:
                    obstacleScript.outofScreen = -outofScreen * scalePivot.x + positionPivot.x;
                    obstacleScript.resetPosition = new Vector3(resetPosition_x * scalePivot.x + positionPivot.x, resetPosition_y * scalePivot.y + positionPivot.y, 0.0f);
                    obstacleScript.obstacleDirection = Vector3.left;
                    o.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case 1:
                    obstacleScript.outofScreen = -outofScreen * scalePivot.x + positionPivot.x;
                    obstacleScript.resetPosition = new Vector3(resetPosition_x * scalePivot.x + positionPivot.x, -resetPosition_y * scalePivot.y + positionPivot.y, 0.0f);
                    obstacleScript.obstacleDirection = Vector3.left;
                    o.transform.rotation = Quaternion.Euler(new Vector3(180.0f, 0, 0));
                    break;
                case 2:
                    obstacleScript.outofScreen = outofScreen * scalePivot.x + positionPivot.x;
                    obstacleScript.resetPosition = new Vector3(-resetPosition_x * scalePivot.x + positionPivot.x, -resetPosition_y * scalePivot.y + positionPivot.y, 0.0f);
                    obstacleScript.obstacleDirection = Vector3.right;
                    o.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180.0f));
                    break;
                case 3:
                    obstacleScript.outofScreen = outofScreen * scalePivot.x + positionPivot.x;
                    obstacleScript.resetPosition = new Vector3(-resetPosition_x * scalePivot.x + positionPivot.x, resetPosition_y * scalePivot.y + positionPivot.y, 0.0f);
                    obstacleScript.obstacleDirection = Vector3.right;
                    o.transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
                    break;
                default:
                    break;
            }
            o.SetActive(false);
        }
    }

    private void OnDisable()
    {
        foreach (var o in Obstacles)
            o.SetActive(false);
        LatestObstacle = null;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(!clearFlag && tutorialFinished)
        {
            if (LatestObstacle == null || Mathf.Abs(LatestObstacle.transform.position.x - LatestObstacleScript.resetPosition.x) > distance)
            {
                LatestObstacle = RandomObject();
                LatestObstacleScript = LatestObstacle.GetComponent<ObstacleScript>();
                LatestObstacle.SetActive(true);
                distance = Random.Range(10.0f, 30.0f) * scalePivot.x;
            }
        }
    }

    // 비활성화된 장애물 오브젝트 중 임의의 장애물 리턴, FixedUpdate에서 해당 장애물을 활성화
    GameObject RandomObject()
    {
        int randomNumber;
        do
        {
            randomNumber = Random.Range(0, Obstacles.Count);
        }
        while (Obstacles[randomNumber].activeSelf == true);
        return Obstacles[randomNumber];
    }
}
