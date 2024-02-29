using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HamState
{
    Idle,
    Jump,
    Bound,
    Stomp,
}

public class HamBehaviour : MonoBehaviour
{
    Rigidbody rb;
    public HamState hamState;
    public float holdTime = 5f;
    public float stompDelay = 1f;
    public float stompTime = 99f;

    Vector3 lastVelocity;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        hamState = HamState.Idle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastVelocity = rb.velocity;
    }

    public void Shoot(Vector2 dir)
    {
        dir = dir * 0.02f;
        //if (routine != null)
        //{
        //    StopCoroutine(routine);
        //    routine = null;
        //}
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector2.ClampMagnitude(dir, 20f), ForceMode.Impulse);

        hamState = HamState.Jump;
    }

    private void OnCollisionEnter(Collision collision)
    {

        
        if (collision.gameObject.CompareTag("Floor"))
        {
            hamState = HamState.Idle;

            StopCoroutine("OnStomp");
            StopCoroutine("StompDelay");
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            hamState = HamState.Bound;
            Debug.Log("พั!");

            var spd = lastVelocity.magnitude;
            var dir = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            Debug.Log(dir);

            rb.velocity = new Vector3((dir * Mathf.Max(spd, 0f) * 0.22f).x, (dir * Mathf.Max(spd, 0f) * 0.4f).y);
            rb.AddForce(Vector2.up * 7f, ForceMode.Impulse);

            //Vector3.Reflect()
            //TriggerHold();
        }
        
    }
    //Coroutine routine = null;

    //public void TriggerHold()
    //{
    //    routine = StartCoroutine(HoldTime());
    //}

    //IEnumerator HoldTime()
    //{
    //    var time = holdTime;
    //    while (time > 0)
    //    {
    //        rb.velocity = Vector3.zero;
    //        hamState = HamState.Hold;
    //        time -= Time.deltaTime;
    //        yield return new WaitForFixedUpdate();
    //    }
    //    hamState = HamState.Slope;
    //}


    public void TriggerStomp()
    {
        hamState = HamState.Stomp;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.AddForce(Vector2.up * 2, ForceMode.Impulse);
        StartCoroutine(StompDelay());
    }
    public void DoStomp()
    {
        rb.useGravity = true;
        rb.AddForce(Vector2.down * 10, ForceMode.Impulse);
        StartCoroutine(OnStomp());
    }

    IEnumerator StompDelay()
    {
        var time = stompDelay;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        DoStomp();
    }
    
    IEnumerator OnStomp()
    {
        var time = stompTime;
        while (time > 0)
        {
            time -= Time.deltaTime;
            rb.AddForce(Vector2.down/1.5f, ForceMode.VelocityChange);
            yield return new WaitForFixedUpdate();
        }
    }
}
