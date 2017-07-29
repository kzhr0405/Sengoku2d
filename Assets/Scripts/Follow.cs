using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Follow : MonoBehaviour {

    public GameObject objTarget;
    public Vector3 offset;

    void Start() {
        updatePostion();
    }

    void LateUpdate() {        
        updatePostion();        
    }

    void updatePostion() {
        if (SceneManager.GetActiveScene().name == "tutorialKassen" || SceneManager.GetActiveScene().name == "kassen" || SceneManager.GetActiveScene().name == "kaisen") {
            if(objTarget != null) {
                Vector3 pos = objTarget.transform.localPosition;
                transform.localPosition = pos + offset;
            }
        }
    }
}