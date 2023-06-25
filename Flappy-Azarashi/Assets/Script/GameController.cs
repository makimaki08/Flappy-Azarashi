using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }

    void GameStart()
    {
        state = State.Play;

        // 各オブジェクトを有効にする
        azarashi.SetSteerActive(true); // azarashiの重力を有効化
        blocks.SetActive(true); // blocksの生成を稼働

        // 最初の入力だけ、ゲームコントローラーから渡す
        azarashi.Flap(); // ゲーム開始直後に、一回だけ浮くようにする
    }

    void GameOver()
    {
        state = State.Gameover;

        // シーン中全てのScrollObjectコンポーネントを探し出す
        ScrollObject[] scrollObjects = FindObjectsOfType<ScrollObject>();

        // 全ScrillObjectのスクロール処理を無効にする
        foreach (ScrollObject so in scrollObjects) so.enabled = false;
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
    }
}
