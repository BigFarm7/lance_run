using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public VariableJoystick joy;

    float horizontalInput;
    float verticalInput;
    bool isHorizontalInput;
    bool isVerticalInput;
    public Transform handle;
    bool braking;
    Rigidbody rb;

    public Vector3 COG;

    [SerializeField] float motorforce;
    [SerializeField] float brakeForce;
    float currentbrakeForce;

    float steeringAngle;
    [SerializeField] float currentSteeringAngle;
    [Range(0f, 0.1f)][SerializeField] float speedteercontrolTime;
    [SerializeField] float maxSteeringAngle;
    [Range(0.000001f, 1)][SerializeField] float turnSmoothing;

    [SerializeField] float maxlayingAngle = 45f;
    public float targetlayingAngle;
    [Range(-40, 40)] public float layingammount;
    [Range(0.000001f, 1)][SerializeField] float leanSmoothing;

    [SerializeField] WheelCollider frontWheel;
    [SerializeField] WheelCollider backWheel;

    [SerializeField] Transform frontWheeltransform;
    [SerializeField] Transform backWheeltransform;

    public GameObject Ragdoll,Lance;

    public DetectGround frontGrounded; 
    public DetectGround rearGrounded;


    
    float floatInAirTime = 0.0f;
    float JumpChargeTime = 0.0f;
    Vector3 oncef;

    Vector3 offset;
    Vector3 eulerAng;
    Transform Cam_Offset;
    Vector3 forwardVec;
    bool isGrounded = false;
    bool isChecked = false;
    bool isTumbled = false;
    bool isStabled = true;
    bool isStarted = false;
    bool isFront = true;
    bool isSide = false;
    bool isBack = false;
    bool isLock = false;
    bool isLive = true;
    bool once = false;

    float PowerX, PowerY, PowerYY;

    public float maxTiltAngle;
    public float tumbleForce;
    public float angle;

    Transform MainCam, mask3D, detectingBox;

    CarManager _carManager;

    public GameObject ragdollSpine;
    public Transform[] mats;
    void Start()
    {
        _carManager = GameObject.Find("XR Origin").GetComponent<CarManager>();
        joy = GameObject.Find("Variable Joystick").GetComponent<VariableJoystick>();
        rb = GetComponent<Rigidbody>();
        isHorizontalInput = false;
        isVerticalInput = false;
        rb.centerOfMass = new Vector3(rb.centerOfMass.x, COG.y, rb.centerOfMass.z);

        StartCoroutine(FindDetect());
        
        Cam_Offset = GameObject.Find("Camera Offset").transform;

        detectingBox = GameObject.FindGameObjectWithTag("Detect").transform;

        offset = -transform.position;

        forwardVec = _carManager.forwardVec;


        StartCoroutine(ShaderGen());

        StartCoroutine(GoStart());
    }
    private void Update()
    {
        if (!isStarted)
            return;
        if (!isLive)
            return;
        followCam();
    }

    void FixedUpdate()
    {
        if (isLive)
        {
            GetInput();
            HandleEngine();
            HandleSteering();
            UpdateWheels();
            UpdateHandle();
            LayOnTurn();
        }
       
    }

    public void GetInput()
    {
        if (!isStarted)
            return;

        //joy.gameObject.SetActive(true);
        horizontalInput = joy.Horizontal;
        verticalInput = joy.Vertical;

        isHorizontalInput = joy.Horizontal != 0;
        isVerticalInput = joy.Vertical != 0;

        JumpChargeTime += Time.deltaTime;


        if (frontGrounded.isGrounded || rearGrounded.isGrounded)
        {
            isGrounded = true;
            floatInAirTime = 0.0f;
        }
           
        else
        {
           
            floatInAirTime += Time.deltaTime;
            if(floatInAirTime >= 0.1f)
                isGrounded = false;
        }
           

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            if (JumpChargeTime > 1.0f)
            {
                rb.AddForce(new Vector3(0, 500, 0), ForceMode.Impulse);
                JumpChargeTime = 0;
            }
               
        }
        
    }

    public void HandleEngine()
    {
        if(!isStarted)
            return;

        if (angle > maxTiltAngle)
        {

        }
        else
        {
            transform.Translate(forwardVec.normalized * 0.002f, Space.World);
            
            Vector3 horizonMove = Vector3.Cross(forwardVec.normalized, transform.up);

            
            transform.Translate(horizonMove * -horizontalInput * 0.0025f, Space.World);

            if(isStabled)
             rb.angularVelocity = Vector3.zero;
        }

        if (isBack)
        {
            backWheel.motorTorque = -1 * motorforce;
        }
        else
        {
            backWheel.motorTorque = 1 * motorforce;
        }
        currentbrakeForce = braking ? brakeForce : 0f;
    }
    /*
    public void DownPresureOnSpeed()
    {
        Vector3 downforce = Vector3.down;
        float downpressure;
        if (rb.velocity.magnitude > 5)
        {
            downpressure = rb.velocity.magnitude;
            rb.AddForce(downforce * downpressure, ForceMode.Force);
        }
    }
    */


    public void SpeedSteerinReductor()
    {
        if (isHorizontalInput || isVerticalInput)
        {
            if (rb.velocity.magnitude < 5)
            {
                maxSteeringAngle = Mathf.LerpAngle(maxSteeringAngle, 30, speedteercontrolTime);
            }
            if (rb.velocity.magnitude > 5 && rb.velocity.magnitude < 10)
            {
                maxSteeringAngle = Mathf.LerpAngle(maxSteeringAngle, 15, speedteercontrolTime);
            }
            if (rb.velocity.magnitude > 10 && rb.velocity.magnitude < 15)
            {
                maxSteeringAngle = Mathf.LerpAngle(maxSteeringAngle, 10, speedteercontrolTime);
            }
            if (rb.velocity.magnitude > 15 && rb.velocity.magnitude < 20)
            {
                maxSteeringAngle = Mathf.LerpAngle(maxSteeringAngle, 5, speedteercontrolTime);
            }
            if (rb.velocity.magnitude > 20)
            {
                maxSteeringAngle = Mathf.LerpAngle(maxSteeringAngle, 2.5f, speedteercontrolTime);
            }
        }
        else
        {
            maxSteeringAngle = Mathf.LerpAngle(maxSteeringAngle, 0, speedteercontrolTime);
        }
    }

    public void HandleSteering()
    {
        if (isGrounded)
        {
            if (isHorizontalInput)
            {
                SpeedSteerinReductor();
                currentSteeringAngle = Mathf.Lerp(currentSteeringAngle, maxSteeringAngle * horizontalInput, turnSmoothing);
                frontWheel.steerAngle = currentSteeringAngle;
                targetlayingAngle = maxlayingAngle * -horizontalInput;
                rb.AddForce(forwardVec * 180, ForceMode.Force);
            }
            else
            {
                SpeedSteerinReductor();
                currentSteeringAngle = Mathf.Lerp(currentSteeringAngle, 0, turnSmoothing);
                frontWheel.steerAngle = currentSteeringAngle;

                if (angle > maxTiltAngle)
                {
                   
                }
                else
                {
                    if (isStabled)
                    {
                        Quaternion currentRotation = transform.rotation;
                        transform.rotation = Quaternion.Lerp(currentRotation, Quaternion.LookRotation(forwardVec),turnSmoothing);
                       
                    }
                }
            }
        }
        else
        {

        }
    }

    private void LayOnTurn()
    {
        Vector3 currentRot = transform.rotation.eulerAngles;

        if (isGrounded)
        {
            if (isHorizontalInput)
            {
               
                    layingammount = Mathf.LerpAngle(layingammount, 0f, 0.05f);
                    transform.rotation = Quaternion.Euler(currentRot.x, currentRot.y, layingammount);

                if (currentSteeringAngle < 0.5f && currentSteeringAngle > -0.5)
                {
                    layingammount = Mathf.LerpAngle(layingammount, 0f, leanSmoothing);
                }
                else
                {
                    layingammount = Mathf.LerpAngle(layingammount, targetlayingAngle, leanSmoothing);
                    //rb.centerOfMass = new Vector3(rb.centerOfMass.x, COG.y, rb.centerOfMass.z);
                }

               // transform.rotation = Quaternion.Euler(currentRot.x, currentRot.y, layingammount);
            }
            else
            {
                if (isStabled)
                {
                    layingammount = Mathf.LerpAngle(layingammount, 0f, 0.05f);
                    transform.rotation = Quaternion.Euler(currentRot.x, currentRot.y, layingammount);
                }
            }

            PowerX = 0;
            PowerY = 0;
            isChecked = false;
        }
        else
        {
            if (isHorizontalInput || isVerticalInput)
            {     
                PowerX -= verticalInput * Time.deltaTime * 200;
                PowerY += horizontalInput * Time.deltaTime * 200;
                PowerYY = PowerY;
                if (!isChecked)
                {
                  
                    Quaternion cur = transform.rotation;
                    eulerAng = cur.eulerAngles;
                    isChecked = true;
                }
                transform.rotation = Quaternion.Euler(eulerAng.x + PowerX,eulerAng.y + PowerY, transform.rotation.z);
               
              
            }
        }
    }

    public void UpdateWheels()
    {
        UpdateSingleWheel(frontWheel, frontWheeltransform);
        UpdateSingleWheel(backWheel, backWheeltransform);
    }
    public void UpdateHandle()
    {
        Quaternion sethandleRot;
        sethandleRot = frontWheeltransform.rotation;
        handle.localRotation = Quaternion.Euler(handle.localRotation.eulerAngles.x, currentSteeringAngle, handle.localRotation.eulerAngles.z);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        //wheelTransform.position = pos;
    }
    void followCam()
    {
        Vector3 forvec =  forwardVec;
        Vector3 objectPosition = transform.position;  


        Vector3 projection = Vector3.Project(objectPosition, forvec);


        Vector3 closestPoint = projection;


        float closestDistance = Vector3.Distance(objectPosition, closestPoint);

        if(!once)
        {
            oncef = closestPoint;
            once = true;
        }
        Cam_Offset.transform.position =  closestPoint - oncef;
        mask3D.transform.position =new Vector3(transform.position.x, mask3D.transform.position.y, transform.position.z);
        detectingBox.position = transform.position - forwardVec.normalized;
    }

    void RagdollOn()
    {
        CopyCharacterTransform(Lance.transform,Ragdoll.transform);
        Lance.SetActive(false);
        Ragdoll.SetActive(true);
        Ragdoll.transform.SetParent(null);
        Ragdoll.transform.Translate(0, 0.001f, 0);
      
    }
    
    public void TimeStop()
    {
        Time.timeScale = 0.0f;
    }

    public void TimeGo()
    {
        Time.timeScale = 1.0f;
    }
    IEnumerator Stable()
    {
        yield return new WaitForSeconds(1);

        isStabled = true;
    }

    IEnumerator GoStart()
    {
        yield return new WaitForSeconds(1.5f);

        isStarted = true;
    }
    void CopyCharacterTransform(Transform origin, Transform ragdoll)
    {
        for (int i = 0; i < origin.childCount; i++)
        {
            if(origin.childCount != 0)
            {
                CopyCharacterTransform(origin.GetChild(i),ragdoll.GetChild(i));
            }
            ragdoll.GetChild(i).localPosition = origin.GetChild(i).localPosition;
            ragdoll.GetChild(i).localRotation = origin.GetChild(i).localRotation;
        }

    }
   
    IEnumerator FindDetect()
    {
        yield return new WaitForSeconds(0.2f);
        mask3D = GameObject.FindGameObjectWithTag("Mask").transform;
    }

    IEnumerator ShaderGen()
    {
        float curValue = -2;
        float lanceValue = -2f;
        float elapsedTime = 0;


        
        Material[] mat = new Material[6];

        for (int i = 0; i < 5; i++)
        {
            mat[i] = mats[i].GetComponent<MeshRenderer>().sharedMaterial;
        }
        mat[5] = mats[5].GetComponent<SkinnedMeshRenderer>().sharedMaterial;

       

        foreach(Material ma in mat)
        {
            ma.SetFloat("cut", -5);
        }


        while(elapsedTime<75)
        {
            curValue = Mathf.Lerp(curValue, 30, elapsedTime / 50);
            lanceValue = Mathf.Lerp(lanceValue, 3.5f, elapsedTime / 75);
            elapsedTime += Time.deltaTime;

            for (int i = 0; i < 5; i++)
            {
                mat[i].SetFloat("cut", curValue);
            }

            mat[5].SetFloat("cut", lanceValue);

            yield return null;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            RagdollOn();
            isLive = false;
            rb.automaticCenterOfMass = true;
            _carManager.Die();
        }
    }
}
