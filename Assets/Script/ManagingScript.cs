using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataInfo;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DataInfo
{
    [System.Serializable]
    public class GameData
    {
        public int HighestScore;
        public bool TutorialDone;
    }
}

public class ManagingScript : MonoBehaviour
{
    public static ManagingScript instance;

    [SerializeField]
    private GameObject Intro_UI;
    [SerializeField]
    private GameObject Main_UI;
    [SerializeField]
    private GameObject Main_background;
    [SerializeField]
    private GameObject Main_object;
    [SerializeField]
    private GameObject Setting_UI;
    [SerializeField]
    private GameObject Outro_UI_gameover;
    [SerializeField]
    private GameObject Outro_UI_credit;
    [SerializeField]
    private GameObject SignatureImage;
    [SerializeField]
    private GameObject Tutorial_UI;

    [SerializeField]
    private AudioSource BackgroundMusic;
    [SerializeField]
    private AudioSource JumpSound;
    [SerializeField]
    private AudioSource CollideSound;
    [SerializeField]
    private GameObject MuteOnImage;
    [SerializeField]
    private GameObject MuteOffImage;

    private static float backgroundMusicVolume;
    private static float jumpsoundMusicVolume;
    private static float collidesoundMusicVolume;
    private static bool muted;
    private static string dataPath;
    private State2Script state2Script;

    public static bool tutorialDone;
    public static GameData SaveData;    // 데이터 저장을 위한 구조체

    // 초기화 작업
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // 음악 초기화
        backgroundMusicVolume = BackgroundMusic.volume;
        jumpsoundMusicVolume = JumpSound.volume;
        collidesoundMusicVolume = CollideSound.volume;
        muted = false;
        MuteOffImage.SetActive(true);
        MuteOnImage.SetActive(false);

        // 데이터 저장 경로
        dataPath = Application.persistentDataPath + "\\gameData.dat";
        SaveData = Load();

        tutorialDone = SaveData.TutorialDone;
        TutorialFinished(tutorialDone);

        // UI 초기화, 게임 시작전에 모두 비활성화
        state2Script = GetComponent<State2Script>();
        state2Script.enabled = false;
        Intro_active(false);
        Main_active(false);
        Setting_active(false);
        Outro_gameover_active(false);
        Outro_credit_active(false);
        SignatureImage_active(false);
        Tutorial_active(false);
    }

    private void OnApplicationQuit()
    {
        instance = null;
        Save(SaveData);
    }

    private void Start()
    {
        IntroScene();
    }

    // 인트로 화면
    public void IntroScene()
    {
        Play_Background();

        GameClear(false);
        Main_active(false);
        Setting_active(false);
        Outro_gameover_active(false);
        Outro_credit_active(false);
        SignatureImage_active(false);
        state2Script.enabled = false;
        Intro_active(true);
    }

    // 메인 게임 화면
    public void MainScene()
    {
        if(!tutorialDone)
        {
            Tutorial_active(true);
        }
        State2Script.startStage = 0;
        Intro_active(false);
        Outro_gameover_active(false);
        Main_active(true);
        state2Script.enabled = true;
    }

    // 게임 오버 화면
    public void GameOverScene()
    {
        state2Script.enabled = false;
        Intro_active(false);
        Main_active(false);
        Outro_gameover_active(true);
    }

    // 게임 클리어 후 페이드아웃 및 엔딩크레딧
    public void GameClearScene()
    {
        state2Script.enabled = false;
        Main_UI.SetActive(false);
        GameClear(true);
        Outro_credit_active(true);
    }

    // 시그니쳐 이미지 출력
    public void SignatureImageScene()
    {
        Main_active(false);
        Outro_gameover_active(false);
        SignatureImage_active(true);
    }

    // 튜토리얼
    public void TutorialScene()
    {
        Tutorial_active(true);
    }

    // UI element 활성화 및 비활성화
    private void Intro_active(bool state)
    {
        Intro_UI.SetActive(state);
    }
    private void Main_active(bool state)
    {
        Main_UI.SetActive(state);
        Main_background.SetActive(state);
        Main_object.SetActive(state);
    }
    private void Setting_active(bool state)
    {
        if(state == true)
        {
            Pause();
        }
        else
        {
            Resume();
        }
        Setting_UI.SetActive(state);
    }
    private void Outro_gameover_active(bool state)
    {
        Outro_UI_gameover.SetActive(state);
    }
    private void Outro_credit_active(bool state)
    {
        Outro_UI_credit.SetActive(state);
    }
    private void SignatureImage_active(bool state)
    {
        SignatureImage.SetActive(state);
    }
    public void Tutorial_active(bool state)
    {
        Tutorial_UI.SetActive(state);
    }

    // clearFlag 값 변경
    private void GameClear(bool state)
    {
        ObstacleCreatorScript.clearFlag = state;
        ObstacleScript.clearFlag = state;
    }

    // 데이터 저장
    private static void Save(GameData gamedata)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(dataPath);

        GameData data = new GameData();
        data.HighestScore = gamedata.HighestScore;
        data.TutorialDone = gamedata.TutorialDone;

        bf.Serialize(fs, data);
        fs.Close();
    }

    // 데이터 로드
    private static GameData Load()
    {
        if(File.Exists(dataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(dataPath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(fs);
            fs.Close();
            return data;
        }
        else
        {
            GameData data = new GameData();
            return data;
        }
    }

    // 게임 일시 정지(환경설정 메뉴 호출 시, 장애물 충돌 시)
    public void Pause()
    {
        JumpScript.pauseFlag = true;
        CharacterScript.pauseFlag = true;
        BackgroundImageScript.pauseFlag = true;
        ObstacleScript.pauseFlag = true;
        State2Script.pauseFlag = true;
    }

    // 게임 재게
    public void Resume()
    {
        JumpScript.pauseFlag = false;
        CharacterScript.pauseFlag = false;
        BackgroundImageScript.pauseFlag = false;
        ObstacleScript.pauseFlag = false;
        State2Script.pauseFlag = false;
    }

    // state값에 따라 ObstacleCreatorScript, State2Script에 있는 변수의 값을 변경, state가 true일 경우 튜토리얼이 종료되면서 장애물과 점수계산이 정상적으로 작동
    public void TutorialFinished(bool state)
    {
        ObstacleCreatorScript.tutorialFinished = state;
        State2Script.tutorialFinished = state;
    }

    // 환경설정 UI 활성화 및 비활성화
    public void OpenClose_SettingUI()
    {
        Setting_active(!Setting_UI.activeSelf);
    }

    // bgm on
    public  void Play_Background()
    {
        if (BackgroundMusic.isPlaying == false)
            BackgroundMusic.Play();
        else { }
    }

    // bgm off
    public void Stop_Background()
    {
        if (BackgroundMusic.isPlaying == true)
            BackgroundMusic.Stop();
        else { }
    }

    // 점프 사운드 on
    public void Play_Jump()
    {
        JumpSound.Play();
    }

    // 충돌 사운드 on
    public void Play_Collide()
    {
        CollideSound.Play();
    }

    // 음소거 on & off
    public void Mute_Sound()
    {
        if(muted)
        {
            BackgroundMusic.volume = backgroundMusicVolume;
            JumpSound.volume = jumpsoundMusicVolume;
            CollideSound.volume = collidesoundMusicVolume;
            muted = false;
            MuteOffImage.SetActive(true);
            MuteOnImage.SetActive(false);
        }
        else
        {
            BackgroundMusic.volume = 0.0f;
            JumpSound.volume = 0.0f;
            CollideSound.volume = 0.0f;
            muted = true;
            MuteOnImage.SetActive(true);
            MuteOffImage.SetActive(false);
        }
    }
}
