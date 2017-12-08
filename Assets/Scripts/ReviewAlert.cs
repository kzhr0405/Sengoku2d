using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewAlert : MonoBehaviour {

    // レビューボタンを押されたときの挙動
    public void Review()
    {
        ReviewManager.Request();
        ReviewManager.SetReviewRequestFalse();
        Destroy(gameObject);
    }

    // あとでボタンを押されたときの挙動
    public void Later()
    {
        Destroy(gameObject);
    }

    // 断るボタンを押したときの挙動
    public void Refuse()
    {
        ReviewManager.SetReviewRequestFalse();
        Destroy(gameObject);
    }
}
