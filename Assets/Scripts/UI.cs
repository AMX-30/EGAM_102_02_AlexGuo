using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int score = PlayerPrefs.GetInt("score");
        Debug.Log("Final Score: " + score);
        score = PlayerPrefs.GetInt("score");
        text.text = "Total violation time:\n" + score;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
