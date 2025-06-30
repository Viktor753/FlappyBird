using System;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenPipeSpawns;

    [SerializeField]
    private float variablePipeHeight = 1;

    [SerializeField]
    private GameObject[] pipes;

    private DateTime nextPipeSpawnTime;

    private Queue<GameObject> pipesAvailable = new Queue<GameObject>();
    private Queue<GameObject> pipesInUse = new Queue<GameObject>();

    private bool pipesMoving = false;

    private void OnEnable()
    {
        Player.OnBirdStateChange += Player_OnBirdStateChange;
    }

    private void OnDisable()
    {
        Player.OnBirdStateChange -= Player_OnBirdStateChange;
    }

    private void Player_OnBirdStateChange(BirdState state)
    {
        if (state == BirdState.Idle)
            foreach (var pipe in pipes)
                pipe.SetActive(false);

        TogglePipes(state == BirdState.Flying);
    }

    private void Start()
    {
        nextPipeSpawnTime = DateTime.Now + TimeSpan.FromSeconds(timeBetweenPipeSpawns);

        foreach(var pipe in pipes)
            pipesAvailable.Enqueue(pipe);
    }

    private void Update()
    {
        if (!pipesMoving)
            return;

        if(DateTime.Now > nextPipeSpawnTime)
        {
            nextPipeSpawnTime = DateTime.Now + TimeSpan.FromSeconds(timeBetweenPipeSpawns);
            SpawnPipe();

            if (pipesInUse.Count >= pipesAvailable.Count - 2)
            {
                var pipe = pipesInUse.Dequeue();
                pipesAvailable.Enqueue(pipe);
                pipe.SetActive(false);
            }
        }

        foreach(var pipe in pipesInUse)
        {
            pipe.transform.Translate(Vector3.left * Time.deltaTime);
        }
    }

    private void SpawnPipe()
    {
        var pipe = pipesAvailable.Dequeue();
        pipesInUse.Enqueue(pipe);
        pipe.SetActive(true);
        var verticalPosition = Mathf.Sin(variablePipeHeight * Time.time);
        pipe.transform.localPosition = new Vector3(0, verticalPosition, 0);
    }

    private void TogglePipes(bool pipesEnabled)
    {
        pipesMoving = pipesEnabled;

        //true = reset positions, reset inuse, and put everything into available
        //false = stop moving, that's it

        if(pipesEnabled)
        {
            var pipesToDequeue = new List<GameObject>();

            foreach (var pipe in pipesInUse)
                pipesToDequeue.Add(pipe);

            foreach (var pipe in pipesToDequeue)
                pipesAvailable.Enqueue(pipesInUse.Dequeue());

            foreach (var pipe in pipes)
            {
                pipe.SetActive(false);
                pipe.transform.localPosition = Vector3.zero;
            }

            nextPipeSpawnTime = DateTime.Now;
        }
    }
}
