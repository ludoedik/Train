using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public UI2DSprite takeButton;
    public UI2DSprite endRoundWindow;
    public UI2DSprite soundButton;
    public GameObject gameUI;
    public UISlider slider;
    public UILabel packagesCountLabel;
    public UILabel roundTimeLabel;
    public UILabel resultsLabel;
    public GameObject train;
    public GameObject[] stations;
    public AudioSource trainWhistle;
    public float sqrMaxStationDistance;
    public float cooldownTime;
    private float timeAfterTaking;
    public float totalTime = 10f;
    private float currentTime;
    public delegate void Round();
    public static event Round OnRoundEnd;
    public static event Round OnRoundBegin;
    private bool isEnd;
    private int packagesCount;
    void Start () {
        endRoundWindow.gameObject.SetActive(false);
        takeButton.gameObject.SetActive(false);
        packagesCountLabel.text = "Собрано груза: 0";
        packagesCount = 0;
        TrainMovementController.OnTrainStopped += EnableTakeButton;
        TrainMovementController.OnTrainMovement += DisableTakeButton;
        currentTime = 0f;

        timeAfterTaking = cooldownTime;

	}

    private void FixedUpdate()
    {
        if (currentTime >= totalTime)
        {
            if (!isEnd)
            {
                isEnd = true;
                EndOfRound();
                return;
            }
        }
        currentTime += Time.fixedDeltaTime;
        roundTimeLabel.text = ConvertTime();

        if (timeAfterTaking < cooldownTime)
            timeAfterTaking += Time.fixedDeltaTime;
    }
    public void EnableTakeButton ()
    {
        if (timeAfterTaking >= cooldownTime)
        {
            foreach (GameObject station in stations)
            {
                if (Vector3.SqrMagnitude(station.transform.position - train.transform.position) < sqrMaxStationDistance)
                
                    takeButton.gameObject.SetActive(true);
                
            }
        }
    }
    
    public void DisableTakeButton ()
    {
        takeButton.gameObject.SetActive(false);
    }
    
    public void AddPackage()
    {
            timeAfterTaking = 0f;
            packagesCount++;
            packagesCountLabel.text = "Собрано груза: " + packagesCount.ToString();
            DisableTakeButton();
        
    }
    
    public void NewRoundButtonHandler ()
    {
        currentTime = 0f;
        packagesCountLabel.text = "Собрано груза: 0";
        packagesCount = 0;
        isEnd = false;
        endRoundWindow.gameObject.SetActive(false);
        gameUI.SetActive(true);
        slider.value = 0f;
        if (OnRoundBegin != null)
            OnRoundBegin.Invoke();
    }

    private string ConvertTime ()
    {
        int minutes = Mathf.FloorToInt((totalTime - currentTime) / 60f);
        int seconds = Mathf.FloorToInt(totalTime - currentTime) - minutes*60;
        string output = "Осталось времени: " + minutes.ToString() + ":" + seconds.ToString();
        return output;
    }

    private void EndOfRound()
    {
        if (OnRoundEnd  != null)
            OnRoundEnd.Invoke();
        endRoundWindow.gameObject.SetActive(true);
        gameUI.SetActive(false);
        resultsLabel.text = "Собрано груза за раунд: " + packagesCount.ToString();
    }
    
    public void PlayTrainWhistle()
    {
        trainWhistle.Play();
    }
}
