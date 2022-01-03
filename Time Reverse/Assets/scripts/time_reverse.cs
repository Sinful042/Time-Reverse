using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class time_reverse : MonoBehaviour
{
    List<Past_values> past_list;
     

    bool reverse = false;


    Rigidbody rb;

    Vector3 temp_velocity;
    Vector3 temp_angvelo;

    public Animator anim;
    private void Start()
    {
        past_list = new List<Past_values>();
        rb = GetComponent<Rigidbody>();
        anim.GetComponent<Animator>();
        anim.SetBool("walk", true);
        
       
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
    public void reversing()
    {
        if (past_list.Count>0)
        {
            transform.position = past_list[0].position;
            transform.rotation = past_list[0].rotation;
            temp_velocity = past_list[0].velocity;
            temp_angvelo = past_list[0].angular_velocity;
            past_list.RemoveAt(0);
            if (anim != null)
            {
                animation_Reverse(anim, past_list[0].animation);
            }
        }
        else
        {
            time_reverse_stop();
        }
        
        
        
    }
    public void past_data_save()
    {
        Past_values past = new Past_values();
        past.position = transform.position;
        past.rotation=transform.rotation;
        past.velocity = (rb != null) ? rb.velocity : Vector3.zero;
        past.angular_velocity = (rb != null) ? rb.angularVelocity : Vector3.zero;
        past.animation = (anim != null) ? read_animation() : null;

        past_list.Insert(0, past);
    }


    //time reverse 
    public void time_reverse_start()
    {
        reverse = true;
        if (rb!=null)
        {
            rb.isKinematic = true;
        }
        if (anim!=null)
        {
            anim.SetFloat("time", -1);
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
            rb.AddTorque(temp_angvelo, ForceMode.VelocityChange);
        }
        if (anim!=null)
        {
            anim.SetFloat("time", 1);
        }
    }
    AnimationParam[] read_animation()
    {
        AnimationParam[] temp = new AnimationParam[anim.parameterCount];
        for (int i = 0; i < temp.Length; i++)
        {
            if (anim.parameters[i].name == "time")
                continue;

            temp[i].name = anim.parameters[i].name;
            temp[i].type = anim.GetParameter(i).type;
            switch (temp[i].type)
            {
                case AnimatorControllerParameterType.Bool:
                    temp[i].values = anim.GetBool(temp[i].name) == true ? 1 : 0;
                    break;
                case AnimatorControllerParameterType.Float:
                    temp[i].values = anim.GetFloat(temp[i].name);
                    break;
                case AnimatorControllerParameterType.Int:
                    temp[i].values = anim.GetFloat(temp[i].name);
                    break;

            }
        
        }


        return temp;
    }
    void animation_Reverse(Animator anim,AnimationParam[] data)
    {
        for (int i = 0; i < anim.parameterCount; i++)
        {
            switch (data[i].type)
            {
                case AnimatorControllerParameterType.Bool:
                    anim.SetBool(data[i].name, (int)data[i].values == 0 ? true : false);
                    break;
                case AnimatorControllerParameterType.Float:
                    anim.SetFloat(data[i].name, data[i].values);
                    break;
                case AnimatorControllerParameterType.Int:
                    anim.SetInteger(data[i].name, (int)data[i].values);
                    break;
            }
        }
    }
    //We  keep  historical data in struct
    public struct Past_values
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
        public Vector3 angular_velocity;
        public AnimationParam[] animation;
    }
    
    public struct AnimationParam
    {

        public string name;
        public AnimatorControllerParameterType type;
        public float values;
    }
    
}
