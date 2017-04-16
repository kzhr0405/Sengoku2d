using UnityEngine;
using System.Collections;

public class SimpleHP : MonoBehaviour {

    public float initLife = 0;
    public float life = 100;
    public float dfc = 10;

    void Start() {
        initLife = life;
    }

    void Update() {
        if (life <= 0) {
            Dead();
        }
    }

    public void Damage(float damage) {
        life -= damage;
        if(life<0) {
            life = 0;
        }
    }

    public void Dead() {
        Destroy(gameObject);
    }


}
