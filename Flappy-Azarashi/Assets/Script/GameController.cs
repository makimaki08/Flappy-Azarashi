using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // ゲームステート
    enum State // ゲームステートの定義
    {
        Reaedy,
        Play,
        Gameover
    }

    State state;
    int score;

    public AzarashiController azarashi;
    public GameObject blocks;
    public Text scoreText;
    public Text stateText;

    void Start()
    {
        // 開始と同時にReadyステートに移行
        Ready();
    }

    void LateUpdate()
    {
        // ゲームのステートごとにイベントを監視
        switch (state)
        {
            case State.Reaedy:
                // タッチしたらゲームスタート
                if (Input.GetButtonDown("Fire1")) GameStart();
                break;

            case State.Play:
                // キャラクターが死亡したらゲームオーバー
                if (azarashi.IsDead()) GameOver();
                break;

            case State.Gameover:
                // タッチしたらシーンをリロード
                if (Input.GetButtonDown("Fiere1")) Reload();
                break;
        }
    }

    // Readytステートへの切り替え
    void Ready()
    {
        state = State.Reaedy;

        // 各オブジェクトを無効状態にする
        azarashi.SetSteerActive(false); // azarashiの重力を無効化
        blocks.SetActive(false); // blocksの生成を停止

        // ラベルを更新
        scoreText.text = "Score：" + 0;

        stateText.gameObject.SetActive(true);
        stateText.text = "Ready";
    }

    void GameStart()
    {
        state = State.Play;

        // 各オブジェクトを有効にする
        azarashi.SetSteerActive(true); // azarashiの重力を有効化
        blocks.SetActive(true); // blocksの生成を稼働

        // 最初の入力だけ、ゲームコントローラーから渡す
        azarashi.Flap(); // ゲーム開始直後に、一回だけ浮くようにする

        // ラベルを更新
        stateText.gameObject.SetActive(false);
        stateText.text = "";
    }

    void GameOver()
    {
        state = State.Gameover;

        // シーン中全てのScrollObjectコンポーネントを探し出す
        ScrollObject[] scrollObjects = FindObjectsOfType<ScrollObject>();

        // 全ScrillObjectのスクロール処理を無効にする
        foreach (ScrollObject so in scrollObjects) so.enabled = false;

        // ラベルを更新
        stateText.gameObject.SetActive(true);
        stateText.text = "GameOver";
    }

    void Reload()
    {
        // 現在読み込んでいるシーンを再読み込み
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score：" + score;
    }
}
