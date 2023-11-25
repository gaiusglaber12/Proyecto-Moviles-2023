using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private SlingController sling = null;
    [SerializeField] private Animator countDownAnimator = null;

    private float speed = 0.0f;
    private bool endGame = false;
    public void Init(float speed)
    {
        countDownAnimator.SetTrigger("countdown");
        this.speed = speed;
    }

    void Update()
    {
        if (!endGame)
        {
            sling.transform.position = new Vector3(sling.transform.position.x, sling.transform.position.y, sling.transform.position.z + (speed * Time.deltaTime));
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (speed * Time.deltaTime));
        }
    }

    public void EndGame()
    {
        endGame = true;
    }
}
