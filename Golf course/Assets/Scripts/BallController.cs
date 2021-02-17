using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public AudioClip puttSound, holeSound;
    public float maxPower; //allows acess in inspector
    public float changeAngleSpeed; //
    public float lineLength;
    public Slider powerSlider;
    public TextMeshProUGUI puttCountLabel; //Setting up counter for the amount of putts in takes to complete the course
    public float minHoleTime; //to decide how long ball needs to be in the hole to count as in
    public Transform startTransform;
    public LevelManager levelManager;

    private LineRenderer line; //Reference to line renderer
    private Rigidbody ball;
    private float angle;
    private float powerUpTime;
    private float power; //when release space, need to refernce the resulting power level
    private int putts; //to store information
    private float holeTime;
    private Vector3 lastPosition;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ball = GetComponent<Rigidbody>();
        ball.maxAngularVelocity = 1000; //limits how fast ball can spin
        line = GetComponent<LineRenderer>(); //Reference
        startTransform.GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {  
        if (ball.velocity.magnitude < 0.01f) //prevents player from putting while ball still rolling
        {
            if (Input.GetKey(KeyCode.A))
            {
                ChangeAngle(-1); //When holding A key, turns left. Left negative. In update so adds 1 every second.
            }
            if (Input.GetKey(KeyCode.D))
            {
                ChangeAngle(+1); //When holding D key, turns right. Right Positive.
            }
            if (Input.GetKeyUp(KeyCode.Space)) //to shoot the ball
            {
                Putt(); //assigning putt method
            }
            if (Input.GetKey(KeyCode.Space))
            {
                PowerUp(); //while space bar held power bar charges up
            }
            UpdateLinePositions(); //Ensures we call this every update
        }
        else
        {
            line.enabled = false;
        }
    }

    private void ChangeAngle(int direction)
    {
        angle += changeAngleSpeed * Time.deltaTime * direction; //multiplied by +1 or -1, makes it go in positive (Right) or negative (Left) direction
    }

    private void UpdateLinePositions() //Setting line positions
    {
        if (holeTime == 0) //as long as its not in the hole
        {
            line.enabled = true;
        }
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * lineLength); //taking our position, then multiplying global forward vector forward/positive direction by the quaternion, giving a position of 1 
    }

    private void Putt()
    {
        audioSource.PlayOneShot(puttSound);
        StartCoroutine(PuttDelay());
    
    }

    private IEnumerator PuttDelay()
    {
        yield return new WaitForSeconds(0.3f); //delay added to make it seem more realistic
        lastPosition = transform.position;
        ball.AddForce(Quaternion.Euler(0, angle, 0) * Vector3.forward * maxPower * power, ForceMode.Impulse);  //referencing force. Putting ball in same direction as line renderer is pointing. Impulse selcted for sudden accerleration
        power = 0;
        powerSlider.value = 0;
        powerUpTime = 0; //so everything resets for next putt
        putts++; //setting up the counter for each putt
        puttCountLabel.text = putts.ToString();
    }

    //to measure to power up affect
    private void PowerUp()
    {
        powerUpTime += Time.deltaTime;
        power = Mathf.PingPong(powerUpTime, 1);
        powerSlider.value = power;
    }

    //triggering when ball enters hole
    private void OnTriggerStay(Collider other) //called on every frame while ball inside trigger
    {
        if(other.tag == "Hole")
        {
            CountHoleTime(); //counted every frame, starts when ball enters a collider and checks if hole collider. if so calls count hole time
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hole")
        {
            audioSource.PlayOneShot(holeSound);
        }
    }

    //to not count if ball bounces in then out of hole or if skims over hole
    private void CountHoleTime()
    {
        holeTime += Time.deltaTime; //delta time added every frame = regular time
        if(holeTime >= minHoleTime)
        {
            levelManager.NextPlayer(putts);
            holeTime = 0;
        }
    }

    private void OnTriggerExit(Collider other) //called on once leaves hole trigger
    {
        if (other.tag == "Hole")
        {
            LeftHole();
        }
    }

    private void LeftHole() //sets ball back to 0
    {
        holeTime = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Out Of Bounds")
        {
            transform.position = lastPosition; //if hits out of bounds with any momentum, going to hold momentum
            ball.velocity = Vector3.zero;
            ball.angularVelocity = Vector3.zero; //these 2 lines takes care of momentum
        }
    }

    public void SetupBall(Color color)
    {
        transform.position = startTransform.position;
        angle = startTransform.rotation.eulerAngles.y;
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
        GetComponent<MeshRenderer>().material.SetColor("_Color", color); //set colour of ball
        line.material.SetColor("_Color", color);
        line.enabled = true; //so we can see line
        putts = 0;
        puttCountLabel.text = "0";
    }
}
