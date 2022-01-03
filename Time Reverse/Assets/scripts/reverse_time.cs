using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reverse_time : MonoBehaviour
{
    Rigidbody rb;



    bool reverse = false;


    List<past_data> past_list;


    private Vector3 temp_velocity;
    private Vector3 temp_angular;

    void Start()
    {
        past_list = new List<past_data>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (reverse)
        {
            reversing();
        }
        else
        {
            past_data_save();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            time_reverse_start();

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            time_reverse_stop();
        }
    }
    public void reversing()
    {
        if (past_list.Count>0)
        {
            transform.position = past_list[0].position;
            transform.rotation = past_list[0].rotation;
            temp_velocity = past_list[0].velocity;
            temp_angular = past_list[0].angular_velocity;
            past_list.RemoveAt(0);
        }
        else
        {
            time_reverse_stop();
        }
    }
    public void past_data_save()
    {
        past_data temp = new past_data();
        temp.position = transform.position;
        temp.rotation = transform.rotation;
        temp.velocity = (rb != null) ? rb.velocity : Vector3.zero;
        temp.angular_velocity = (rb != null) ? rb.angularVelocity : Vector3.zero;


        past_list.Insert(0, temp);

    }
    public void time_reverse_start()
    {
        reverse = true;
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }
    public void time_reverse_stop()
    {
        reverse = false;
        if (rb!=null)
        {
            rb.isKinematic = false;
            rb.WakeUp();
            rb.AddForce(temp_velocity, ForceMode.VelocityChange);
            rb.AddTorque(temp_angular, ForceMode.VelocityChange);
        }
    }

    public struct past_data
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
        public Vector3 angular_velocity;


    }
}
