using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class TrainMovementController : MonoBehaviour {
    public PathCreator pathCreator;
    public float trainHeight;
    public GameObject[] stations;
    private float speed;
    private float distanceTravelled;
    public float maxSpeed;
    public float deltaSpeed;
    private float newSpeed;
    private bool isStopped;
    public delegate void Action();
    public static event Action OnTrainStopped;
    public static event Action OnTrainMovement;
    

    void Start()
    {
        speed = 0f;
        newSpeed = 0f;
        isStopped = false;
        GameController.OnRoundEnd += EndOfRound;
        GameController.OnRoundBegin += BeginningOfRound;
    }
    void FixedUpdate () {
        distanceTravelled += speed * Time.fixedDeltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled) + new Vector3(0f, trainHeight / 2, 0f);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
        if (Mathf.Abs(speed-newSpeed) > deltaSpeed)
        {
            speed += -Time.fixedDeltaTime * (speed - newSpeed);
        }
        if (speed < deltaSpeed && !isStopped) 
        {
            speed = 0f;
            if (OnTrainStopped != null)
                OnTrainStopped.Invoke();
            isStopped = true;
        }
    }

    public void SetSpeed (float koeff)
    {
        newSpeed = maxSpeed * koeff;
        if (Mathf.Abs(newSpeed) >= deltaSpeed) //&& speed < deltaSpeed)
        {
            if (OnTrainMovement != null)
                OnTrainMovement.Invoke();
            isStopped = false;
        }
    }

    public void EndOfRound ()
    {
        speed = 0f;
        newSpeed = 0f;
    }
    

    public void BeginningOfRound()
    {
        distanceTravelled = 0f;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled) + new Vector3(0f, trainHeight / 2, 0f);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }
}
