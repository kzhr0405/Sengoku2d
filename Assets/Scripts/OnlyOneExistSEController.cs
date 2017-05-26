using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyOneExistSEController : MonoBehaviour {
    void Awake()
    {
        if (FindObjectsOfType<OnlyOneExistSEController>().Length >= 2)
        {
            Destroy(gameObject);
        }
    }
}
