using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.Unity;

public class GameController : MonoBehaviour
{
    public Salut salut;
    public Image Progress;
    public Text TimerText;
    public GameObject RestartButton;
    public GameObject EndText;
    public Transform CannonTransform;
    public GameObject BulletPrefab;
    public Text BulletCountText;
    public Text LevelText1;
    public Text LevelText2;
    Save sv;
    Camera cam;
    int BULLET_COUNT;
    public int TARGET_COUNT;
    bool gameEnded = false;
    void Start()
    {
        cam = GetComponent<Camera>();
        if (PlayerPrefs.HasKey("sv"))
        {
            sv = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("sv"));
        }
        else
        {
            sv = new Save(0);
            SavePrefs();
        }
        TARGET_COUNT = Instantiate(Resources.Load<GameObject>("Level " + (sv.level < 10 ? sv.level : 9))).transform.childCount - 1;
        maxTarget = TARGET_COUNT;
        Progress.fillAmount = 0;
        LevelText1.text = (sv.level + 1).ToString();
        LevelText2.text = (sv.level + 2).ToString();
        BULLET_COUNT = 3 * (sv.level + 1);
        UpdateBulletCountText();
        FB.Init();
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    void UpdateBulletCountText()
    {
        if(BULLET_COUNT > 0)
            BulletCountText.text = "x" + BULLET_COUNT;
        else
            BulletCountText.text = "NO MORE";
    }
    int maxTarget;
    public void MinusTarget()
    {
        if (!gameEnded)
        {
            TARGET_COUNT--;
            Progress.fillAmount = 1f - (TARGET_COUNT * 1f) / (maxTarget * 1f);
            if (TARGET_COUNT <= 0)
                Win();
        }
    }
    void MinusBullet()
    {
        BULLET_COUNT--;
        if (BULLET_COUNT == 0)
        {
            timer = 5;
            TimerText.gameObject.SetActive(true);
            StartCoroutine("CheckEnd");
        }
        UpdateBulletCountText();
    }
    int timer = 5;
    IEnumerator CheckEnd()
    {
        while (timer > 0)
        {
            TimerText.text = timer.ToString();
            yield return new WaitForSeconds(1f);
            timer--;
        }
        if (TARGET_COUNT > 0)
            Lose();
    }
    void Win()
    {
        gameEnded = true;
        salut.enabled = true;
        StopCoroutine("CheckEnd");
        TimerText.gameObject.SetActive(false);
        EndText.SetActive(true);
        RestartButton.SetActive(true);
        sv.level++;
        SavePrefs();
        if (FB.IsInitialized)
        {
            FB.LogAppEvent(AppEventName.AchievedLevel, sv.level);
        }
    }
    void Lose()
    {
        gameEnded = true;
        TimerText.gameObject.SetActive(false);
        EndText.GetComponent<Text>().text = "NO MORE BULLETS!";
        EndText.SetActive(true);
        RestartButton.transform.GetChild(0).GetComponent<Text>().text = "REPLAY";
        RestartButton.SetActive(true);
    }
    void SavePrefs()
    {
        PlayerPrefs.SetString("sv", JsonUtility.ToJson(sv));
    }
    void Shoot(Vector3 goal)
    {
        CannonTransform.LookAt(goal);
        CannonTransform.localEulerAngles = new Vector3(0f, CannonTransform.localEulerAngles.y, 0f);
        CannonAnimate();
        Transform newBullet = Instantiate(BulletPrefab).transform;
        newBullet.position = CannonTransform.GetChild(0).position;
        newBullet.GetComponent<Rigidbody>().AddForce((goal - newBullet.position) * 100f);
        MinusBullet();
    }
    RaycastHit[] hits;
    Ray ray;
    void Update()
    {
        if (Input.anyKeyDown && BULLET_COUNT > 0 && !gameEnded)
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray, 120f);
            for(int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.CompareTag("aim"))
                {
                    Shoot(hits[i].point + Vector3.up * 8f);
                    break;
                }
            }
        }
        CannonAnim();
    }
    void CannonAnimate()
    {
        cannonAnimBool = true;
        cannonAnimFloat = 0f;
    }
    bool cannonAnimBool = false;
    float cannonAnimFloat = 0f;
    void CannonAnim()
    {
        if (cannonAnimBool)
        {
            cannonAnimFloat += Time.deltaTime;
            if(cannonAnimFloat < 0.1f)
            {
                CannonTransform.position = new Vector3(0f, -3.3f, -35.8f - cannonAnimFloat / 0.1f);
            }
            else if (cannonAnimFloat < 0.2f)
            {
                CannonTransform.position = new Vector3(0f, -3.3f, -36.8f + (cannonAnimFloat - 0.1f) / 0.1f);
            }
            else
            {
                CannonTransform.position = new Vector3(0f, -3.3f, -35.8f);
                cannonAnimBool = false;
            }
        }
    }
}
class Save
{
    public int level;
    public Save(int level)
    {
        this.level = level;
    }
}
