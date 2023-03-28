using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  private static GameObject player;
  private static GameObject kickArrow;
  private static bool increaseKickArrowScale = true;
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
    kickArrow = GameObject.Find("Player/Kick Arrow Container");
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
        player.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
      }
      if (Input.GetKey(GameManager.leftButton))
      {
        player.transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
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
    if (Input.GetKeyDown(GameManager.kickBallButton) && (GameManager.ballPlayerKickableDistance >= Vector3.Distance(player.transform.position, GameManager.ball.transform.position))) {
      kickArrow.transform.localScale = new Vector3(1f, 1f, 1f);
      kickArrow.transform.localRotation = Quaternion.identity;
      kickArrow.SetActive(true);
    }
    
    if (Input.GetKey(GameManager.kickBallButton) && (GameManager.ballPlayerKickableDistance >= Vector3.Distance(player.transform.position, GameManager.ball.transform.position))) {
      // arrow scale control
      float currKickArrowScale = kickArrow.transform.localScale.x;
      float newKickArrowScale = 0f;

      if (increaseKickArrowScale) {
        newKickArrowScale = currKickArrowScale + GameManager.kickArrowScaleChangePerSecond * Time.deltaTime;
        if (newKickArrowScale >= GameManager.kickArrowMaxScale) {
          increaseKickArrowScale = false;
        }
      } else {
        newKickArrowScale = currKickArrowScale - GameManager.kickArrowScaleChangePerSecond * Time.deltaTime;
        if (newKickArrowScale <= GameManager.kickArrowMinScale) {
          increaseKickArrowScale = true;
        }
      }

      kickArrow.transform.localScale = new Vector3(newKickArrowScale, newKickArrowScale, newKickArrowScale);

      // arrow direction control
      if (Input.GetKey(GameManager.forwardButton) && (kickArrow.transform.eulerAngles.x == 0 || 360 - kickArrow.transform.eulerAngles.x < GameManager.kickArrowMaxVerticalAngle))
      {
        Quaternion q = kickArrow.transform.rotation;
        float x = q.eulerAngles.x - GameManager.kickArrowAngleChangePerSecond * Time.deltaTime;
        if(x >= 0 && x < 300) {
          x = 300;
        }
        q.eulerAngles = new Vector3(x, q.eulerAngles.y, q.eulerAngles.z);
        kickArrow.transform.rotation = q;
      }
      if (Input.GetKey(GameManager.backwardButton) && kickArrow.transform.eulerAngles.x > 300 - GameManager.kickArrowMaxVerticalAngle)
      {
        Quaternion q = kickArrow.transform.rotation;
        float x = q.eulerAngles.x + GameManager.kickArrowAngleChangePerSecond * Time.deltaTime;
        if(x < 300 || x > 360) {
          x = 0;
        }
        q.eulerAngles = new Vector3(x, q.eulerAngles.y, q.eulerAngles.z);
        kickArrow.transform.rotation = q;
      }
      if (Input.GetKey(GameManager.rightButton) && (90 - kickArrow.transform.eulerAngles.y >= GameManager.kickArrowMaxHorizontalAngle || kickArrow.transform.eulerAngles.y <= 90 + GameManager.kickArrowMaxHorizontalAngle))
      {
        Quaternion q = kickArrow.transform.rotation;
        float y = q.eulerAngles.y + GameManager.kickArrowAngleChangePerSecond * Time.deltaTime;
        Debug.Log(y);
        if(y > 90 + GameManager.kickArrowMaxHorizontalAngle) {
          y = 90 + GameManager.kickArrowMaxHorizontalAngle;
        }
        q.eulerAngles = new Vector3(q.eulerAngles.x, y, q.eulerAngles.z);
        kickArrow.transform.rotation = q;
      }
      if (Input.GetKey(GameManager.leftButton) && (90 - kickArrow.transform.eulerAngles.y >= GameManager.kickArrowMaxHorizontalAngle || kickArrow.transform.eulerAngles.y <= 90 + GameManager.kickArrowMaxHorizontalAngle))
      {
        Quaternion q = kickArrow.transform.rotation;
        float y = q.eulerAngles.y - GameManager.kickArrowAngleChangePerSecond * Time.deltaTime;
        if(y < GameManager.kickArrowMaxHorizontalAngle) {
          y = GameManager.kickArrowMaxHorizontalAngle;
        }
        q.eulerAngles = new Vector3(q.eulerAngles.x, y, q.eulerAngles.z);
        kickArrow.transform.rotation = q;
      }
    }

    if (Input.GetKeyUp(GameManager.kickBallButton) || (GameManager.ballPlayerKickableDistance < Vector3.Distance(player.transform.position, GameManager.ball.transform.position))) {
      kickArrow.SetActive(false);
      increaseKickArrowScale = true;
    }
  }
}
