using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzarashiController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator animator;
    float angle;
    bool isDead;

    public float maxHeight;
    public float flapVelocity;
    public float relativeVelocityX;
    public GameObject sprite;

    public bool IsDead()
    {
        return isDead; // 死亡判定の公開
    }

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>(); // Rigidbody2Dコンポーネントを取得
        animator = sprite.GetComponent<Animator>(); // Animatorコンポーネント取得
    }

    // Update is called once per frame
    void Update()
    {
        // 最高高度に達していない場合に限りタップの入力を受け付ける
        if (Input.GetButtonDown("Fire1") && transform.position.y < maxHeight)
        {
            Flap();
        }

        // 角度を反映
        ApplyAngle();

        // angleが水平以上だったら、アニメーターのflapフラグをtrueにする
        animator.SetBool("flap", angle >= 0.0f);
    }

    public void Flap()
    {
        // 死んでいれば、羽ばたかせない
        if (isDead) return;

        // Velocityを直接書き換えて、上方向に加速
        rb2d.velocity = new Vector2(0.0f, flapVelocity);
    }
    void ApplyAngle()
    {
        // 現在の速度、相対速度から進んでいる角度を求める
        float targetAngle;

        // 死亡したら常にひっくり返る
        if (isDead)
        {
            targetAngle = 180.0f;
        }
        else
        {
            targetAngle = Mathf.Atan2(rb2d.velocity.y, relativeVelocityX) * Mathf.Rad2Deg;

        }

        // 回転アニメをスムージング
        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 10.0f);

        // Rotationの反映
        sprite.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // 何かにぶつかったら、死亡フラグを立てる
        isDead = true;
    }
}
