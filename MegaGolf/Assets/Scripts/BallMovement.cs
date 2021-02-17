using System;
using UnityEngine;
using UnityEngine.Events;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float shotPower, maxForce, minSpeed;
    [SerializeField] private LineRenderer myLR;
    [SerializeField] private UnityEvent<string> shotTaken;

    private Rigidbody myRB;
    private float shotForce;
    private Vector3 startPos, endPos, direction;
    private bool canShoot, shotStarted;
    private int strokes;
    

    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
        canShoot = true;
        myRB.sleepThreshold = minSpeed;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            startPos = MousePositionInWorld();
            shotStarted = true;
            myLR.gameObject.SetActive(true);
            myLR.SetPosition(0, myLR.transform.localPosition);
        }

        if (Input.GetMouseButton(0) && shotStarted)
        {
            endPos = MousePositionInWorld();
            shotForce = Mathf.Clamp(Vector3.Distance(endPos, startPos), 0, maxForce);
            myLR.SetPosition(1, transform.InverseTransformPoint(endPos));
        }

        if (Input.GetMouseButtonUp(0) && shotStarted)
        {
            canShoot = false;
            shotStarted = false;
            myLR.gameObject.SetActive(false);
            strokes++;
            shotTaken.Invoke(strokes.ToString());
        }
    }

    private void FixedUpdate()
    {
        if (!canShoot)
        {
            direction = startPos - endPos;
            myRB.AddForce(Vector3.Normalize(direction) * shotForce * shotPower, ForceMode.Impulse);
            startPos = endPos = Vector3.zero;
        }

        if (myRB.IsSleeping())
        {
            canShoot = true;
        }
    }

    private Vector3 MousePositionInWorld()
    {
        Vector3 position = Vector3.zero;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            position = hit.point;
        }

        return position;
    }
}
