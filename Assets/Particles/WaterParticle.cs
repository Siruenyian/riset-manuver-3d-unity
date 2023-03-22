using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticle : MonoBehaviour
{
    public ParticleSystem ripple;
    public Rigidbody rb;
    public GameObject rippleCamera;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rippleCamera.transform.position = transform.position + Vector3.up * 10;
        Shader.SetGlobalVector("_RippleRpos", transform.position);
        Debug.Log(Shader.GetGlobalVector("_RippleRpos"));
    }
    void CreateRipple(int start, int end, int delta, float speed, float size, float lifetime)
    {
        Vector3 forward = ripple.transform.eulerAngles;
        forward.y = start;
        ripple.transform.eulerAngles = forward;
        for (int i = start; i < end; i=i+delta)
        {

            ripple.Emit(transform.position + ripple.transform.forward*0.5f, ripple.transform.forward*speed, size, lifetime, Color.white);
            ripple.transform.eulerAngles += Vector3.up * 3;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==4 && rb.velocity.y> 0.05f)
        {
            CreateRipple(-180, 180, 3, 2, 2, 2);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 4&& rb.velocity.x!=0 && Time.renderedFrameCount%5==0)
        {
            int y = (int)transform.eulerAngles.y;
            CreateRipple(y - 90, y + 90, 3, 5, 2, 1);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 4 && rb.velocity.y > 0.05f)
        {
            CreateRipple(-180, 180, 3, 2, 2, 2);
        }
    }
}
