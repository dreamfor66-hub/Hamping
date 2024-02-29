using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float camSpd = 5f;
    public HamBehaviour ham;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var diff = Vector3.Distance(new Vector3(0, ham.transform.position.y, 0), new Vector3(0, transform.position.y, 0));

        diff = Mathf.Abs(diff);
        
        if (ham.hamState == HamState.Jump)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, ham.transform.position.y, transform.position.z), 1f * Time.deltaTime);
        }
        
        if (ham.hamState == HamState.Bound)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, ham.transform.position.y, transform.position.z), 1f * Time.deltaTime);
        }

        if (ham.hamState == HamState.Stomp)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, ham.transform.position.y, transform.position.z), 5f * Time.deltaTime);
        }
    }

    public void SetCameraPosition()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, ham.transform.position.y, transform.position.z), 0.2f * Time.deltaTime);


        //transform.position = new Vector3(transform.position.x, ham.transform.position.y, transform.position.z);
    }
}
