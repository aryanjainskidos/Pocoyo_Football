using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallGame : MonoBehaviour
{
    [Header("3D Elements")]
    public Transform SoccerBall;
    public Vector3 StartPosition;
    public List<Transform> Points;
    public float YOffset;
    [Header("2D Elements")]
    public int Touches = 0;
    public Text Score;
    public GameObject ScoreAdd;
    public List<GameObject> BallGhosts;
    public UISpriteTween BallPop;
    public float TimeToFail = 5f;

    private void Start()
    {
        SoccerBall.gameObject.SetActive(false);

        foreach (GameObject go in BallGhosts)
            go.SetActive(false);

        ScoreAdd.SetActive(false);

        BallPop.gameObject.SetActive(false);
    }

    float CurrentTime;
    Vector3 startPosition;
    Vector3 middlePosition;
    Vector3 endPosition;
    IEnumerator UpdateSoccerPosition()
    {
        while(true)
        {
            CurrentTime += Time.deltaTime;

            //Mover X hacia el punto en TimeToFail
            float new_x = Mathf.Lerp(startPosition.x, endPosition.x, CurrentTime / TimeToFail);
            float new_z = SoccerBall.position.z;

            if (CurrentTime < (TimeToFail * 0.5f))
            {
                //Mover Y arriba en TimeToFail/2
                float t = Mathf.Clamp01(CurrentTime / (TimeToFail * 0.5f));
                float new_y = Mathf.Lerp(startPosition.y, middlePosition.y + YOffset, EasingHelp.Quadratic.Out(t) );
                SoccerBall.position = new Vector3(new_x, new_y, new_z);
            }
            else
            {
                //Mover Y abajo en TimeToFail/2
                float t = Mathf.Clamp01( (CurrentTime - (TimeToFail * 0.5f)) / (TimeToFail * 0.5f) );
                float new_y = Mathf.Lerp(middlePosition.y + YOffset, endPosition.y, EasingHelp.Quadratic.In(t));
                SoccerBall.position = new Vector3(new_x, new_y, new_z);

                if(t == 1)
                {
                    if (Succeed) DoGhostTouched();
                    else FailTouch();
                    break;
                }
            }
            yield return null;
        }

        yield return null;
    }

    public void StartGame()
    {
        Touches = 0;
        Score.text = Touches.ToString();
        SoccerBall.transform.position = StartPosition;
        SoccerBall.gameObject.SetActive(true);

        ScoreAdd.SetActive(false);
        BallPop.gameObject.SetActive(false);

        SetNextTouch();
    }

    public void StopGame()
    {
        StopAllCoroutines();
        SoccerBall.gameObject.SetActive(false);

        ScoreAdd.SetActive(false);
        BallPop.gameObject.SetActive(false);

        NextTouch = -1;
        Succeed = false;

        foreach (GameObject go in BallGhosts)
            go.SetActive(false);

        SoundBank_B.instance.StopAll("Game");
    }

    private int NextTouch = -1;
    private bool Succeed = false;
    public void SetNextTouch()
    {
        AnimationCtrl.instance.AnimalPlayEnded.RemoveListener(SetNextTouch);

        CurrentTime = 0f;
        Succeed = false;

        startPosition = SoccerBall.position;
        if (NextTouch > -1)
        {
            startPosition = Points[NextTouch].position;
            SoccerBall.position = startPosition;
        }


        NextTouch = Random.Range(0, BallGhosts.Count);
        BallGhosts[NextTouch].SetActive(true);

        float endY = startPosition.y > Points[NextTouch].position.y ? startPosition.y : Points[NextTouch].position.y;
        middlePosition = new Vector3(Points[NextTouch].position.x, endY, Points[NextTouch].position.z);
        endPosition = new Vector3( Points[NextTouch].position.x, Points[NextTouch].position.y, Points[NextTouch].position.z);
                
        StartCoroutine(UpdateSoccerPosition());
    }

    public void GhostTouched()
    {
        Succeed = true;
        Touches += 1;
        Score.text = Touches.ToString();

        ScoreAdd.SetActive(true);
        Invoke("HideScoreAdd", 0.15f);

        BallGhosts[NextTouch].SetActive(false);

        BallPop.gameObject.transform.position = BallGhosts[NextTouch].transform.position;
        BallPop.gameObject.SetActive(true);

        SoundBank_B.instance.PlayRange("Game", 0, 2);
    }

    void HideScoreAdd()
    {
        ScoreAdd.SetActive(false);
    }

    public void DoGhostTouched()
    {        
        StopAllCoroutines();

        CurrentTime = 0f;
        
        AnimationCtrl.instance.SetBallTouch(NextTouch);
        AnimationCtrl.instance.PlayBallGameResult(true);

        SoundBank_B.instance.PlayRange("Game", 3, 4);

        Invoke("SetNextTouch", 0.08f);        
    }

    void FailTouch()
    {
        StopAllCoroutines();
        AnimationCtrl.instance.AnimalPlayEnded.AddListener(SetNextTouch);
        AnimationCtrl.instance.SetBallTouch(NextTouch);
        AnimationCtrl.instance.PlayBallGameResult(false);

        Touches = 0;
        Score.text = Touches.ToString();
        BallGhosts[NextTouch].SetActive(false);

        NextTouch = -1;
        SoccerBall.position = new Vector3(0f,2f,-0.18f);

        SoundBank_B.instance.Play("Game", 5);
    }
}
