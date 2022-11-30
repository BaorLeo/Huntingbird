using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBird : MonoBehaviour
{
    
    public float xSpeed = 0;
    public float XSpeedRandom = 0.25f;
    private float ySpeed = 0;
    private bool isAlive = true;
    private SpriteRenderer spriteRenderer;
    Collider2D a_colider;
    public GameObject death;
    


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        a_colider = GetComponent<Collider2D>();
        Flip();
        StartCoroutine(ChangeDirection(Random.Range(2.5f, 4f)));
        Invoke("Disappear", 20);
    }

    
    void LateUpdate()
    {
        transform.Translate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, 0f);
    }
    public void Flip()
    {
        xSpeed = xSpeed + Random.Range(-XSpeedRandom, XSpeedRandom);
        if (xSpeed >= 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
    public void SetDirection(float dir)
    {
        xSpeed *= dir;
    }
    public void NockOut()
    {
        isAlive = false;
        a_colider.enabled = false;
        spriteRenderer.flipY = true;
        ySpeed = -1f;
        StartCoroutine(DisappearSec(0.6f));
        Destroy(GetComponent<Animator>());
        var ctr = FindObjectOfType<Controller>();
        if (death)
        {
            ctr.ScoreNum++;
            Instantiate(death, transform.position, Quaternion.identity);
        }
        ctr.NewScore(ctr.ScoreNum);
    }
    
    IEnumerator DisappearSec(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    IEnumerator ChangeDirection (float time)
    {
        yield return new WaitForSeconds(time);
        if (isAlive)
        {
            
            ySpeed = Mathf.Abs(xSpeed);
            
        }
    }
    
}
