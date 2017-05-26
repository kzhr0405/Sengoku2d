using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyOneExistBGMController : MonoBehaviour{
    void Awake()
    {
        if (FindObjectsOfType<OnlyOneExistBGMController>().Length >= 2)
        {
            Destroy(gameObject);
        }
    }
}
