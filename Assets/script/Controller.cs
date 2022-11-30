using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public new AudioSource audio;
    public AudioClip shoot;
    public AudioClip Win;
    [SerializeField] GameObject EndMenu;
    public Text myScore;
    public Text txtSoreW;
    public Text txtTime;
    int scoreNum;
    int TimeLimit;
    public int StartTime;
    public ControllerBird[] birdPrefabs;
   
    public Texture2D Cross;
    public float BirdDelay = 1.5f;

    private bool notgameover = true;
    private bool mouseLeftDown = false;
    private Vector2 ClickPos;
    private float nextBirdTime = 0f;
    float pos = 9.2f;
    float dir = 1;
    public int ScoreNum { get => scoreNum; set => scoreNum = value; }


    public  void Awake()
    {
        TimeLimit = StartTime;
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        Play();
        audio = GetComponent<AudioSource>();
    }
    public void Play()
    {
        Cursor.SetCursor(Cross, new Vector2(Cross.width / 2, Cross.height / 2), CursorMode.Auto);
        NewScore(scoreNum);
        StartCoroutine(TimeCountDown());
    }
    

    // Update is called once per frame
    void LateUpdate()
    {
        Clicked();
    }
    public void Clicked()
    {
        if (notgameover)
        {
            if (Time.time >= nextBirdTime)
            {
                SpawnBird();
                nextBirdTime = Time.time + BirdDelay;
                if (BirdDelay >= 1.2f)
                {
                    BirdDelay *= 0.9f;
                }
                if (BirdDelay < 1.2f && BirdDelay > 0.5f)
                {
                    BirdDelay *= 0.99f;
                }
            }
            mouseLeftDown = Input.GetMouseButtonDown(0);
            if (mouseLeftDown)
            {
                audio.PlayOneShot(shoot);
                ClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ClickPos, ClickPos, 0f);
                if (hit && hit.collider.gameObject.layer == LayerMask.NameToLayer("Bird"))
                {
                    hit.collider.SendMessage("NockOut", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        /*else Debug.Log("Game over");*/
    }
    public void SpawnBird()
    {
        float newScale = Random.Range(0.1f, 0.1989898f);
        Vector3 randomPositionRight = new Vector3(pos, Random.Range(-3f ,3f));
        int randIdx = Random.Range(0, birdPrefabs.Length);
        ControllerBird newBird = Instantiate(birdPrefabs[randIdx], randomPositionRight, Quaternion.identity);
        newBird.transform.localScale = new Vector3(newScale, newScale, 1f);
        newBird.SendMessage("SetDirection", dir, SendMessageOptions.DontRequireReceiver);
        dir *= -1;
        pos *= -1;
    }
    public void NewScore(int score)
    {
        if (myScore)
        {
            myScore.text = "" + score.ToString();
        }
    }
    public void UpdateTimer(string time)
    {
        if (txtTime)
        {
            txtTime.text = time;
        }
    }
    public string IntToTime(int time)
    {
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.RoundToInt(time % 60);
        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }
    IEnumerator TimeCountDown()
    {

        while (TimeLimit > 0)
        {
            yield return new WaitForSeconds(1f);
            TimeLimit--;
            if (TimeLimit <= 0)
            {
                var aud = FindObjectOfType<Manager>();
                aud.GetComponent<AudioSource>().Pause();
                audio.PlayOneShot(Win);
                EndMenu.SetActive(true);
                Time.timeScale = 0f;
                
                    UpdateScore(scoreNum);
               
            }
            UpdateTimer(IntToTime(TimeLimit));
        }
    }
    public void UpdateScore(int updateScore)
    {
        
        if (txtSoreW)
        {
            txtSoreW.text = "" + updateScore.ToString(); ;
        }
    }
}
