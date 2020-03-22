using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Salut : MonoBehaviour
{
    GameObject SalutPartPrefabL;
    GameObject SalutPartPrefabR;
    GameObject SalutPartPrefabL1;
    GameObject SalutPartPrefabR1;
    public GameController gc;
    void Start()
    {
        SalutPartPrefabL = Resources.Load<GameObject>("SalutPartL");
        SalutPartPrefabR = Resources.Load<GameObject>("SalutPartR");
        SalutPartPrefabL1 = Resources.Load<GameObject>("SalutPartL1");
        SalutPartPrefabR1 = Resources.Load<GameObject>("SalutPartR1");
    }
    float timer = 0f;
    float deltaTime;
    Transform t;
    float timer1 = 0f;
    float s = 0.1f;
    bool isCircle = false;
    void FixedUpdate()
    {
        //if (timer1 < 1f)
        {
            timer1 += Time.fixedDeltaTime;
            timer += 0.02f;
            if (timer > deltaTime)
            {
                deltaTime = 0.06f;
                for (int i = 0; i < 100; i++)
                {
                    isCircle = Random.Range(0f, 1f) > 0.7f;
                    t = Instantiate(i % 2 == 0? (isCircle ? SalutPartPrefabL1 : SalutPartPrefabL)  : (isCircle ? SalutPartPrefabR1 : SalutPartPrefabR), transform).transform;
                    t.GetComponent<Image>().color =
                        Random.Range(0, 3) == 0 ? new Color(1f, 0f, 124f / 255f, 1f) :
                        (Random.Range(0, 2) == 0 ? new Color(141f / 255f, 1f, 0f, 1f) : new Color(0f, 165f / 255f, 1f, 1f));
                    s = (Random.Range(0, 2) == 0 ? Random.Range(5f, 10f) : Random.Range(15f, 30f));
                    t.GetComponent<RectTransform>().sizeDelta = new Vector2(s, s);
                    t.GetComponent<RectTransform>().sizeDelta = new Vector2(s / 1.25f, s / 1.25f);
                }
            }            
        }
       // else
        {
            Destroy(this);
        }
    }
}