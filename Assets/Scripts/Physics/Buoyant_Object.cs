using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyant_Object : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    //Int untuk berapa banyak floater objek
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    [SerializeField]
    private float wavefactor=1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.AddForceAtPosition(Physics.gravity/floaterCount, transform.position, ForceMode.Acceleration);
        float waveHeight = 0;
            //Mathf.Sin(Mathf.PI * 2 * Time.time * wavefactor);
        //Debug.Log("transform vs sin value: "+transform.position.y+ " "+waveHeight);
        // perkiraan bahwa posisi < 0 itu dibawah air
        if (transform.position.y<=waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f),transform.position, ForceMode.Acceleration);
            rigidbody.AddForce(displacementMultiplier * -rigidbody.velocity * waterDrag * Time.deltaTime, ForceMode.VelocityChange);
            rigidbody.AddTorque(displacementMultiplier * -rigidbody.angularVelocity * waterAngularDrag * Time.deltaTime, ForceMode.VelocityChange);

        }
    }
}
