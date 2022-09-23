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
  }

  // Update is called once per frame
  void Update()
  {
    Rigidbody prb = player.GetComponent<Rigidbody>();
    if(Input.GetKey(forwardButton))
    {
      if(prb.velocity.magnitude < topForwardSpeed)
      {
        prb.AddRelativeForce(0f, 0f, forwardAcceleration, ForceMode.Acceleration);
      }
    }
    if(Input.GetKey(backwardButton))
    {
      if(prb.velocity.magnitude < topBackwardSpeed)
      {
        prb.AddRelativeForce(0f, 0f, -backwardAcceleration, ForceMode.Acceleration);
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
