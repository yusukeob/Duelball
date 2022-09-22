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
  private static float overTopSpeedPenalty;

  Animator animator;

  // Start is called before the first frame update
  void Start()
  {
    animator = GetComponent<Animator>();

    player = GameObject.Find("Player");
    topForwardSpeed = 100f;
    topBackwardSpeed = 50f;
    forwardAcceleration = 5f;
    backwardAcceleration = 2f;
    forwardButton = "w";
    backwardButton = "s";
    rightButton = "d";
    leftButton = "a";
    rotationSpeed = 100f;
    overTopSpeedPenalty = 0.99f;
  }

  // Update is called once per frame
  void Update()
  {
    Rigidbody prb = player.GetComponent<Rigidbody>();
    if(Input.GetKey(forwardButton))
    {
      prb.AddRelativeForce(0f, 0f, forwardAcceleration, ForceMode.Acceleration);
      if(prb.velocity.magnitude > topForwardSpeed)
      {
        prb.velocity *= overTopSpeedPenalty;
      }
    }
    if(Input.GetKey(backwardButton))
    {
      prb.AddRelativeForce(0f, 0f, -backwardAcceleration, ForceMode.Acceleration);
      if(prb.velocity.magnitude > topBackwardSpeed)
      {
        prb.velocity *= overTopSpeedPenalty;
      }
    }
    if (Input.GetKey(rightButton))
    {
      transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
    if (Input.GetKey(leftButton))
    {
      transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
    }

    animator.SetFloat("playerSpeed", prb.velocity.magnitude);
  }
}
