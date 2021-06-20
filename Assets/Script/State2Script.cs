using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class State2Script : MonoBehaviour
{
    public List<GameObject> ObjectPresets;
    public List<GameObject> ButtonPresets;
    public List<GameObject> BackgroundPresets;
    public GameObject TwoDivisionImage;
    public GameObject FourDivisionImage;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI alertText;
    public TextMeshProUGUI OutroResult_highestscore;
    public TextMeshProUGUI OutroResult_score;
    public TextMeshProUGUI OutroResult_stage;

    public static float Width;                          // 게임을 실행하는 기기의 해상도가 referenceResolution값과 다를 경우 UI element에 적용되어야 할 너비값
    public static float Height;                         // 게임을 실행하는 기기의 해상도가 referenceResolution값과 다를 경우 UI element에 적용되어야 할 높이값
    public static bool pauseFlag = false;   // 장애물에 부딪히거나, 설정창을 열었을 때 게임 일시정지를 위한 flag 변수
    public static bool tutorialFinished;
    public static int startStage;   // 첫 시작 스테이지
    public static float objectResolutionRatio;

    private static int endScore;    // 마지막 스테이지가 종료되는 시점의 점수
    private static int stage; // 0 ~ 10
    private static float score; // 현재 스코어
    private static int alertTerm = 20;  // 다음 스테이지 진입전에 알림메시지 출력할 점수 차이

    // 스테이지별 화면분할과 회전방향, 점수 정보를 담은 리스트
    // 각 리스트 0번째 인덱스는 화면 분할 갯수, 1~4번째 인덱스는 각 화면 회전 방향, 5번째 인덱스는 해당 스테이지 점수
    private static List<List<int>> QuadrapleDivisionPreset =
        new List<List<int>> {
            new List<int> { 0, 0, -1, -1, -1, 100 },
            new List<int> { 1, 0, 0, -1, -1, 100 },
            new List<int> { 1, 1, 3, -1, -1, 200 },
            new List<int> { 2, 0, 0, 0, 0, 300 },
            new List<int> { 2, 2, 0, 1, 0, 300 },
            new List<int> { 2, 0, 3, 0, 1, 500 },
            new List<int> { 2, 3, 1, 0, 2, 500 },
            new List<int> { 2, 1, 2, 3, 0, 1000 },
            new List<int> { 2, 1, 2, 0, 1, 1500 },
            new List<int> { 2, 1, 2, 3, 1, 2500 },
    };
    private static List<int> stageOffset = new List<int>(); // 각 스테이지 별 점수를 더해서 다음 스테이지 진입 점수를 계산해놓은 리스트
    private int stageCount; // 스테이지 총 갯수

    // 초기화 작업
    private void Awake()
    {
        TwoDivisionImage.SetActive(false);
        FourDivisionImage.SetActive(false);
        stageCount = QuadrapleDivisionPreset.Count;
        stageOffset.Add(0);
        for (int i = 0; i < stageCount; i++)
        {
            stageOffset.Add(stageOffset[i] + QuadrapleDivisionPreset[i][5]);
        }
        endScore = stageOffset[stageOffset.Count - 1];
    }

    // 초기화 작업
    private void OnEnable()
    {
        DeactivatePresets();
        alertText.gameObject.SetActive(false);

        stage = startStage;
        stageText.text = (stage + 1).ToString();
        score = 0;
        ActivatePresets(stage);
    }

    private void OnDisable()
    {
        // 최고점수 갱신
        if (score > ManagingScript.SaveData.HighestScore)
            ManagingScript.SaveData.HighestScore = (int)score;

        // 결과화면 갱신
        OutroResult_highestscore.text = ManagingScript.SaveData.HighestScore.ToString();
        OutroResult_score.text = ((int)score).ToString();
        OutroResult_stage.text = (stage + 1).ToString();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // 점수 계산
        if (!pauseFlag && tutorialFinished && score < endScore)
        {
            score += 0.1f + stage * 0.05f;
        }
        scoreText.text = ((int)score).ToString();

        // 일정 점수 획득 시 다음 스테이지로 진입
        if (score >= stageOffset[stage + 1])
        {
            // endScore 도달 시 게임 클리어
            if (score >= endScore)
            {
                ManagingScript.instance.GameClearScene();
            }
            // 다음 스테이지 진입
            else
            {
                DeactivatePresets();
                ActivatePresets(stage + 1);
                stageText.text = (stage + 1).ToString();
                alertText.gameObject.SetActive(false);
                stage++;
            }
        }

        // 다음 스테이지 진입 전에 알림 텍스트 출력
        if (score >= stageOffset[stage + 1] - alertTerm)
        {
            if (stage + 1 < stageCount)
                alertText.gameObject.SetActive(true);
        }
    }

    // 버튼을 주어진 위치와 크기로 변경
    private void ButtonPresetSetting(GameObject obj, Vector3 pos, Vector2 wh)
    {
        obj.transform.GetComponent<RectTransform>().anchoredPosition = pos;
        obj.transform.GetComponent<RectTransform>().localScale = new Vector2(wh.x / UIBackgroundScalingScript.referenceRatio.x, wh.y / UIBackgroundScalingScript.referenceRatio.y);
    }

    // 배경 이미지를 주어진 위치와 크기로 변경
    private void BackgroundPresetSetting(GameObject obj, Vector3 pos, int rot, Vector2 wh)
    {
        obj.transform.GetComponent<RectTransform>().anchoredPosition = pos;
        obj.transform.GetComponent<RectTransform>().localScale = new Vector2(wh.x / UIBackgroundScalingScript.referenceRatio.x, wh.y / UIBackgroundScalingScript.referenceRatio.y);
        switch (rot)
        {
            case 0:
                obj.transform.GetChild(0).GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                obj.transform.GetChild(1).GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
            case 1:
                obj.transform.GetChild(0).GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(180, 0, 0));
                obj.transform.GetChild(1).GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(180, 0, 0));
                break;
            case 2:
                obj.transform.GetChild(0).GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                obj.transform.GetChild(1).GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
            case 3:
                obj.transform.GetChild(0).GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                obj.transform.GetChild(1).GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                break;
            default:
                break;
        }

    }

    // 오브젝트(스프라이트)들을 주어진 위치, 회전, 크기로 변경
    private void ObjectPresetSetting(GameObject obj, Vector3 pos, int rot, Vector3 scl)
    {
        obj.GetComponent<TransformInfoScript>().positionPivot = pos;
        obj.GetComponent<TransformInfoScript>().rotationPivot = rot;
        obj.GetComponent<TransformInfoScript>().scalePivot = scl;
    }

    // QuadrapleDivisionPreset 값에 따라 각 스테이지마다 화면이 어떤 방향으로 회전하는지 오브젝트 UI 프리셋들을 설정
    private void ActivatePresets(int index)
    {
        switch (QuadrapleDivisionPreset[index][0])
        {
            case 0: // 1분할 화면 / Object, Button 프리셋 0번만 활성화
                ObjectPresetSetting(ObjectPresets[0], new Vector3(0, 0, 0), QuadrapleDivisionPreset[index][1], new Vector3(objectResolutionRatio, 1, 1));
                ButtonPresetSetting(ButtonPresets[0], new Vector3(0, 0, 0), new Vector2(Width, Height));
                BackgroundPresetSetting(BackgroundPresets[0], new Vector3(0, 0, 0), QuadrapleDivisionPreset[index][1], new Vector2(Width, Height));
                ObjectPresets[0].SetActive(true);
                ButtonPresets[0].SetActive(true);
                BackgroundPresets[0].SetActive(true);

                ObjectPresets[1].SetActive(false);
                ObjectPresets[2].SetActive(false);
                ObjectPresets[3].SetActive(false);
                ButtonPresets[1].SetActive(false);
                ButtonPresets[2].SetActive(false);
                ButtonPresets[3].SetActive(false);
                BackgroundPresets[1].SetActive(false);
                BackgroundPresets[2].SetActive(false);
                BackgroundPresets[3].SetActive(false);
                break;
            case 1: // 2분할 화면 / 프리셋 0, 1번 활성화
                TwoDivisionImage.SetActive(true);
                ObjectPresetSetting(ObjectPresets[0], new Vector3(0, 2.5f, 0), QuadrapleDivisionPreset[index][1], new Vector3(objectResolutionRatio, 0.5f, 1));
                ButtonPresetSetting(ButtonPresets[0], new Vector3(0, Height * 0.25f, 0), new Vector2(Width, Height * 0.5f));
                BackgroundPresetSetting(BackgroundPresets[0], new Vector3(0, Height * 0.25f, 0), QuadrapleDivisionPreset[index][1], new Vector2(Width, Height * 0.5f));
                ObjectPresets[0].SetActive(true);
                ButtonPresets[0].SetActive(true);
                BackgroundPresets[0].SetActive(true);

                ObjectPresetSetting(ObjectPresets[1], new Vector3(0, -2.5f, 0), QuadrapleDivisionPreset[index][2], new Vector3(objectResolutionRatio, 0.5f, 1));
                ButtonPresetSetting(ButtonPresets[1], new Vector3(0, -Height * 0.25f, 0), new Vector2(Width, Height * 0.5f));
                BackgroundPresetSetting(BackgroundPresets[1], new Vector3(0, -Height * 0.25f, 0), QuadrapleDivisionPreset[index][2], new Vector2(Width, Height * 0.5f));
                ObjectPresets[1].SetActive(true);
                ButtonPresets[1].SetActive(true);
                BackgroundPresets[1].SetActive(true);

                ObjectPresets[2].SetActive(false);
                ObjectPresets[3].SetActive(false);
                ButtonPresets[2].SetActive(false);
                ButtonPresets[3].SetActive(false);
                BackgroundPresets[2].SetActive(false);
                BackgroundPresets[3].SetActive(false);
                break;
            case 2: // 3분할 화면 / 프리셋 모두 활성화
                FourDivisionImage.SetActive(true);
                ObjectPresetSetting(ObjectPresets[0], new Vector3(objectResolutionRatio * -4.45f, 2.5f, 0), QuadrapleDivisionPreset[index][1], new Vector3(objectResolutionRatio * 0.5f, 0.5f, 1));
                ButtonPresetSetting(ButtonPresets[0], new Vector3(-Width * 0.25f, Height * 0.25f, 0), new Vector2(Width * 0.5f, Height * 0.5f));
                BackgroundPresetSetting(BackgroundPresets[0], new Vector3(-Width * 0.25f, Height * 0.25f, 0), QuadrapleDivisionPreset[index][1], new Vector2(Width * 0.5f, Height * 0.5f));
                ObjectPresets[0].SetActive(true);
                ButtonPresets[0].SetActive(true);
                BackgroundPresets[0].SetActive(true);

                ObjectPresetSetting(ObjectPresets[1], new Vector3(objectResolutionRatio * 4.45f, 2.5f, 0), QuadrapleDivisionPreset[index][2], new Vector3(objectResolutionRatio * 0.5f, 0.5f, 1));
                ButtonPresetSetting(ButtonPresets[1], new Vector3(Width * 0.25f, Height * 0.25f, 0), new Vector2(Width * 0.5f, Height * 0.5f));
                BackgroundPresetSetting(BackgroundPresets[1], new Vector3(Width * 0.25f, Height * 0.25f, 0), QuadrapleDivisionPreset[index][2], new Vector2(Width * 0.5f, Height * 0.5f));
                ObjectPresets[1].SetActive(true);
                ButtonPresets[1].SetActive(true);
                BackgroundPresets[1].SetActive(true);

                ObjectPresetSetting(ObjectPresets[2], new Vector3(objectResolutionRatio * -4.45f, -2.5f, 0), QuadrapleDivisionPreset[index][3], new Vector3(objectResolutionRatio * 0.5f, 0.5f, 1));
                ButtonPresetSetting(ButtonPresets[2], new Vector3(-Width * 0.25f, -Height * 0.25f, 0), new Vector2(Width * 0.5f, Height * 0.5f));
                BackgroundPresetSetting(BackgroundPresets[2], new Vector3(-Width * 0.25f, -Height * 0.25f, 0), QuadrapleDivisionPreset[index][3], new Vector2(Width * 0.5f, Height * 0.5f));
                ObjectPresets[2].SetActive(true);
                ButtonPresets[2].SetActive(true);
                BackgroundPresets[2].SetActive(true);

                ObjectPresetSetting(ObjectPresets[3], new Vector3(objectResolutionRatio * 4.45f, -2.5f, 0), QuadrapleDivisionPreset[index][4], new Vector3(objectResolutionRatio * 0.5f, 0.5f, 1));
                ButtonPresetSetting(ButtonPresets[3], new Vector3(Width * 0.25f, -Height * 0.25f, 0), new Vector2(Width * 0.5f, Height * 0.5f));
                BackgroundPresetSetting(BackgroundPresets[3], new Vector3(Width * 0.25f, -Height * 0.25f, 0), QuadrapleDivisionPreset[index][4], new Vector2(Width * 0.5f, Height * 0.5f));
                ObjectPresets[3].SetActive(true);
                ButtonPresets[3].SetActive(true);
                BackgroundPresets[3].SetActive(true);
                break;
            default:
                break;
        }
    }

    // 모든 Object, UI 프리셋 비활성화
    private void DeactivatePresets()
    {
        TwoDivisionImage.SetActive(false);
        FourDivisionImage.SetActive(false);
        foreach (var p in ObjectPresets)
            p.SetActive(false);
        foreach (var p in ButtonPresets)
            p.SetActive(false);
        foreach (var p in BackgroundPresets)
            p.SetActive(false);
    }
}
