using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    GameController gc;
    private void Start()
    {
        gc = Camera.main.GetComponent<GameController>();
    }
    void Update()
    {
        if(transform.position.y < -2f)
        {
            gc.MinusTarget();
            Destroy(this);
        }
    }
}
