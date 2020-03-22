using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalutPart : MonoBehaviour
{
    Image image;
    RectTransform rt;
    public bool isLeft = true;
    void Start()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        col = image.color;
        randomRot = Random.Range(200f, 600f);
        maxX = (isLeft ? 1f : -1f) * Random.Range(30f, 550f);
        maxY = Random.Range(50f, 623f);
        timeScale = Random.Range(0.5f, 0.7f);
        timeScale2 = 3f * Random.Range(0.8f, 1f);
        transform.localScale = new Vector3(0f, 0f, 0f);
    }
    float timeScale2 = 1f;
    float randomRot;
    Color col;
    float timeScale = 0.6f;
    float maxX, maxZ, maxY;
    float timer = 0f;
    float timer1 = 0;
    void Update()
    {
        timer += Time.deltaTime;
        timer1 = Mathf.Pow(timer, timeScale) * timeScale2;
        transform.localEulerAngles = new Vector3(0f, 0f, maxX * 60f + (isLeft ? 1f : -1f) * timer * randomRot);
        if(timer < 6f)
        {
            rt.anchoredPosition = new Vector2(30.75f * (isLeft ? 1f : -1f) + maxX * timer1 / 6f, 50.9f + maxY * (-1f * Mathf.Pow(timer1 - 3f, 2f) + 9f) / 9f);
            if (timer < 0.5f)
            {
                transform.localScale = new Vector3(timer / 0.5f, timer / 0.5f, timer / 0.5f);
            }
            else
            {
                image.color = new Color(col.r, col.g, col.b, 1f - (timer - 0.5f) / (2f * timeScale2));
            }
        }
        else
        {
            Destroy(gameObject);
        }        
    }
}
