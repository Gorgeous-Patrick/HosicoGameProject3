using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBirdController : MonoBehaviour
{
    Subscription<EventStopTank> sub_EventStopTank;
    public UnityEvent OnChangeFormTank;

    [SerializeField] private float spd = 5f;
    [SerializeField] private Transform tank;

    private Rigidbody2D RB2D;
    private Vector2 playerInput;
    private Animator animatorComponent;

    private void Awake()
    {
        RB2D = GetComponent<Rigidbody2D>();
        animatorComponent = GetComponent<Animator>();
        sub_EventStopTank = EventBus.Subscribe<EventStopTank>(OnEventBirdSummon);
        transform.position = tank.position;
    }

    private void OnEventBirdSummon(EventStopTank obj)
    {
        transform.position = tank.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            ResetBird(tank);
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");

        animatorComponent.SetFloat("Horizontal", playerInput.x);
        animatorComponent.SetFloat("Vertical", playerInput.y);
        animatorComponent.SetFloat("spd", playerInput.sqrMagnitude);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var dir = playerInput * spd * Time.deltaTime;
        RB2D.MovePosition(RB2D.position + dir);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("BelowFlightHeight"))
        {
            var collided = collision.transform.GetComponent<Collider2D>();
            var birdCollide = transform.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(collided, birdCollide);
        }


    }

    public void ResetBird(Transform tank)
    {
        transform.position = tank.position;
        OnChangeFormTank?.Invoke();
    }
}
