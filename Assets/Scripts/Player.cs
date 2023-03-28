using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  private static GameObject player;
  private static GameObject kickArrow;
  private static float topForwardSpeed = 100f;
  private static float topBackwardSpeed = 50f;
  private static float forwardAcceleration = 5f;
  private static float backwardAcceleration = 2f;
  private static float rotationSpeed = 100f;
  private static bool hasBall = false;

  Animator animator;

  // Start is called before the first frame update
  void Start()
  {
    animator = GetComponent<Animator>();
    player = GameObject.Find("Player");
    kickArrow = GameObject.Find("Player/Kick Arrow");
    kickArrow.SetActive(false);
  }

  // Update is called once per frame
  void Update()
  {
    // movement
    Rigidbody prb = player.GetComponent<Rigidbody>();

    if (!Input.GetKey(GameManager.kickBallButton) || (GameManager.ballPlayerKickableDistance < Vector3.Distance(player.transform.position, GameManager.ball.transform.position))) {
      if(Input.GetKey(GameManager.forwardButton))
      {
        if(prb.velocity.magnitude < topForwardSpeed)
        {
          prb.AddRelativeForce(0f, 0f, forwardAcceleration, ForceMode.Acceleration);
        }
      }
      if(Input.GetKey(GameManager.backwardButton))
      {
        if(prb.velocity.magnitude < topBackwardSpeed)
        {
          prb.AddRelativeForce(0f, 0f, -backwardAcceleration, ForceMode.Acceleration);
        }
      }
      if (Input.GetKey(GameManager.rightButton))
      {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
      }
      if (Input.GetKey(GameManager.leftButton))
      {
        transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
      }
    }

    float playerSpeed = prb.velocity.magnitude;
    if (Input.GetKey(GameManager.kickBallButton) && (GameManager.ballPlayerKickableDistance >= Vector3.Distance(player.transform.position, GameManager.ball.transform.position))) {
      playerSpeed *= GameManager.playerIsKickingGameSpeedModifier;
    }

    animator.SetFloat("playerSpeed", playerSpeed);

    // ball interaction
    if (Input.GetKeyDown(GameManager.engageBallButton) && (hasBall || GameManager.ballPlayerPickupDistance >= Vector3.Distance(player.transform.position, GameManager.ball.transform.position)))
    {
      hasBall = !hasBall;
    }
    if (hasBall)
    {
      Vector3 ballPlacement = (player.transform.forward * GameManager.ballPlacementDistance) + player.transform.position;
      GameManager.ball.transform.position = new Vector3(ballPlacement.x, ballPlacement.y + 10f, ballPlacement.z);
      GameManager.ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }
    else
    {
      GameManager.ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    // ball kick action
    if (Input.GetKey(GameManager.kickBallButton) && (GameManager.ballPlayerKickableDistance >= Vector3.Distance(player.transform.position, GameManager.ball.transform.position))) {
      kickArrow.SetActive(true);
    }

    if (Input.GetKeyUp(GameManager.kickBallButton) || (GameManager.ballPlayerKickableDistance < Vector3.Distance(player.transform.position, GameManager.ball.transform.position))) {
      kickArrow.SetActive(false);
    }
  }
}
