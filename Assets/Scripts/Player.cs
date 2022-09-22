using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static GameObject player;
    private static float topForwardSpeed;
    private static float topBackwardSpeed;
    private static float forwardAcceleration;
    private static float backwardAcceleration;
    private static float velocity;
    private static float rotationSpeed;
    private static string forwardButton;
    private static string backwardButton;
    private static string rightButton;
    private static string leftButton;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        // topSpeed = 10f;
        forwardAcceleration = 10f;
        backwardAcceleration = 5f;
        // velocity = 0f;
        forwardButton = "w";
        backwardButton = "s";
        rightButton = "d";
        leftButton = "a";
        rotationSpeed = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(forwardButton))
        {
            player.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, forwardAcceleration, ForceMode.Acceleration);
        }
        if(Input.GetKey(backwardButton))
        {
            player.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, -backwardAcceleration, ForceMode.Acceleration);
        }
        if (Input.GetKey(rightButton))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(leftButton))
        {
            transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
}
