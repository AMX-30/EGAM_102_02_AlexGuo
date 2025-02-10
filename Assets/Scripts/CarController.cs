using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CarController : MonoBehaviour
{
    private GameObject headLights;
    private Rigidbody2D rb2d;

    public float driftTime;
    public float driftSpd;
    public float driftBackSpd;
    public bool outOfLane;
    public Vector2 DrifDirec;

    public float eventWaitTime;

    public int headLightsTrigger;
    public bool headLightsBool;

    public float SpeedingSpd;
    public float speedingTime;
    public float speedingBackSpd;

    public float failTime;
    private float failingTimer;
    public float violationTime;
    public bool isFailing;

    public float winTimer;
    public float winThreshold;
    public int score;

    public TMP_Text Spd;
    public TMP_Text Steering;
    public TMP_Text HeadL;
    public TMP_Text Home;
    public TMP_Text PSL;

    List<string> eventGrupo = new List<string> {"LaneDrift", "HeadLightsOff", "Speeding"};

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        headLights = GameObject.Find("Headlights");
        failingTimer = 7f;
        winTimer = 0;
        violationTime = 0;
        score = 0;
        PlayerPrefs.SetInt("score",score);
        PlayerPrefs.Save();
        Steering.text = "Steering: <color=green>||</color>";
        HeadL.text = "HeadLights: \n\n<color=green>Normal</color>";
        Spd.text = "Speed: <color=green>65</color>";
        PSL.text = "<color=green>0</color>";
        StartCoroutine(EventGrupo());

    }

    IEnumerator EventGrupo()
    {
        while(true)
        {
            if(eventGrupo.Count > 0)
            {
                int randomIndex = Random.Range(0, eventGrupo.Count);
                string eventInString = eventGrupo[randomIndex];

                switch (eventInString)
                {
                    case "LaneDrift":
                        StartCoroutine(DriftLane());
                        Debug.Log(randomIndex);
                        break;

                    case "HeadLightsOff":
                        headLightsTrigger = 0;
                        headLightsBool = true;
                        StartCoroutine(HeadLightsOff());                   
                        Debug.Log(randomIndex);
                        break;
                    
                    case "Speeding":
                        StartCoroutine(SpeedingFunction());
                        break;
                }
            }
            yield return new WaitForSeconds(eventWaitTime);
        }
    }

    IEnumerator DriftLane()
    {
        float driftTimer = 0f;
        Vector2 driftDirection = Random.value > 0.5f ? Vector2.right:Vector2.left;

        while (driftTimer < driftTime)
        {
            rb2d.AddForce(driftDirection * driftSpd, ForceMode2D.Force);
            driftTimer += Time.deltaTime;
            if(driftDirection == Vector2.right)
            {
                Steering.text = "Steering: <color=red>=></color>";
            }
            else if(driftDirection == Vector2.left)
            {
                Steering.text = "Steering: <color=red><=</color>";
            }
            yield return null;
        }
        if(driftTimer >= driftTime)
        {
            Steering.text = "Steering: <color=green>||</color>";
        }
    }

    IEnumerator SpeedingFunction()
    {
        float speedingTimer = 0f;
        float speed = Random.value > 0.5f ? SpeedingSpd * 1:SpeedingSpd * -1;
        Vector2 speedingDirection = Vector2.down;

        while(speedingTimer < speedingTime)
        {
            rb2d.AddForce(speedingDirection * speed, ForceMode2D.Force);
            speedingTimer += Time.deltaTime;
            if(speed < 0)
            {
                Spd.text = "Speed: <color=red>75</color>";
            }
            else
            {
                Spd.text = "Speed: <color=red>55</color>";
            }
            yield return null;
        }
        if(speedingTimer >= speedingTime)
        {
            Spd.text = "Speed: <color=green>65</color>";
        }
    }

    IEnumerator HeadLightsOff()
    {
        while(headLightsBool)
        {
            HeadL.text = "HeadLights: \n\n<color=red>Bad</color>";

            headLights.SetActive(false);
            Debug.Log("off");
            yield return new WaitForSeconds(0.5f);

            headLights.SetActive(true);
            Debug.Log("on");
            yield return new WaitForSeconds(0.5f);
        }
    }

    void DriftBack()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            rb2d.AddForce(Vector2.left * driftBackSpd, ForceMode2D.Impulse);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            rb2d.AddForce(Vector2.right * driftBackSpd, ForceMode2D.Impulse);
        }
    }
    void SpeedBack()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            rb2d.AddForce(Vector2.down * speedingBackSpd, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb2d.AddForce(Vector2.up * speedingBackSpd, ForceMode2D.Impulse);
        }
    }
    void HeadLightFunction()
    {
        if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        {
            headLightsTrigger ++;
        }

        if(headLightsTrigger >= 30)
        {
            headLightsBool = false;
            HeadL.text = "HeadLights: \n\n<color=green>Normal</color>";
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Lane"))
        {
            outOfLane = true;
        }
        if(other.gameObject.name == "RightLane")
        {
            outOfLane = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Edge"))
        {
            SceneManager.LoadScene("CrashScene");
        }
    }

    void FailCondition()
    {
        int failConver = Mathf.RoundToInt(failTime);

        if(outOfLane || headLightsBool)
        {
            failTime += Time.deltaTime;
            violationTime += Time.deltaTime;

            if(failConver > 0)
            {
                PSL.text = "<color=red>" + failConver.ToString() + "</color>";
            }

            if(failTime >= failingTimer)
            {
                //Debug.Log("fail");
                SceneManager.LoadScene("FailScene");
            }
        }
        if(!outOfLane && !headLightsBool)
        {
            if(failTime > 0)
            {
                failTime -= Time.deltaTime;
            }
            PSL.text = "<color=green>" + failConver.ToString() + "</color>";
            //Debug.Log("reset");
        }
    }

    void WinCondition()
    {
        winTimer += Time.deltaTime;
        int TimerConver = Mathf.RoundToInt(winTimer);

        if(TimerConver <= 20)
        {
            Home.text = "Home:<color=green>" + TimerConver.ToString() + "</color>";
        }
        else
        {
            Home.text = "Home:<color=red>" + TimerConver.ToString() + "</color>";
            eventWaitTime = 3.5f;
        }

        if(winTimer >= winThreshold)
        {
            //Debug.Log("win");
            score = Mathf.RoundToInt(violationTime);
            PlayerPrefs.SetInt("score", score);
            SceneManager.LoadScene("WinScene");
        }
    }

    void Reload()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(EventGrupo());
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(HeadLightsOff());
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(SpeedingFunction());
        }

        DriftBack();
        HeadLightFunction();
        SpeedBack();

        FailCondition();
        WinCondition();
        Reload();
    }
}
