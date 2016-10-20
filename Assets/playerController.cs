using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{
    Rigidbody body;

    public float torqueAir;

    public float torqueGround;

    public float driveAcceleration;

    bool isGrounded = false;

    public LayerMask driveableSurfaces;

    public float disToGround = .6f;

    float tireSpinAmount = 0;

    public float maxTireSpeed;

	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool b = Input.GetButton("Handbrake");
        bool j = Input.GetButton("Jump");

        CheckGround();

        if(isGrounded) //player is on ground
        {
            

            float speedPercent = tireSpinAmount / maxTireSpeed;
            body.velocity = tireSpinAmount * transform.forward;

            if(v == 0)
            {
                if (tireSpinAmount > 0)
                {
                    tireSpinAmount -= driveAcceleration * Time.deltaTime * .1f;
                    if(tireSpinAmount < 0)
                    {
                        tireSpinAmount = 0;
                    }
                }
                if (tireSpinAmount < 0)
                {
                    tireSpinAmount += driveAcceleration * Time.deltaTime;
                }
            }
            //Vector3 torque = new Vector3(0, h, 0);
            //body.AddRelativeTorque(torque * torqueGround);

            Quaternion newQ = Quaternion.AngleAxis(h * torqueGround, transform.up) * transform.rotation;

            body.MoveRotation(newQ);

            tireSpinAmount += v * driveAcceleration * Time.fixedDeltaTime;
            tireSpinAmount = Mathf.Clamp(tireSpinAmount, -maxTireSpeed, maxTireSpeed);

            //body.AddRelativeForce(new Vector3(0, 0, v * driveAcceleration));

            //body.velocity = transform.forward * body.velocity.magnitude;
        }
        else //player is in air
        {
            Vector3 torque = new Vector3();

            if (b)
            {
                torque.z = h;
            }
            else
            {
                torque.y = h;
            }

            torque.x = v * torqueAir;


            body.AddRelativeTorque(torque);
        }
	} //end FixedUpdate()

    void CheckGround()
    {
        Ray ray = new Ray(transform.position, transform.up * -1);
        RaycastHit hit;

        //Debug.DrawRay(ray.origin, ray.direction * disToGround);

        if (Physics.Raycast(ray, out hit, disToGround, driveableSurfaces))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
