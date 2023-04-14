using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  private static GameObject player;
  private static GameObject kickArrow;
  private static Quaternion initKickArrowRotation;
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
    initKickArrowRotation = kickArrow.transform.localRotation;
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

    // tackle action
    bool tackle = false;
    if (Input.GetKeyDown(GameManager.tackleButton)) {
      tackle = true;
    }

    animator.SetBool("tackle", tackle);

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
      kickArrow.transform.localRotation = initKickArrowRotation;
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
      float verticalIncrease = 0f;
      float verticalDecrease = 0f;
      float horizontalIncrease = 0f;
      float horizontalDecrease = 0f;
      if (Input.GetKey(GameManager.forwardButton))
      {
        verticalIncrease = -GameManager.kickArrowAngleChangePerSecond * Time.deltaTime;
      }
      if (Input.GetKey(GameManager.backwardButton))
      {
        verticalDecrease = -GameManager.kickArrowAngleChangePerSecond * Time.deltaTime;
      }
      if (Input.GetKey(GameManager.rightButton))
      {
        horizontalIncrease = GameManager.kickArrowAngleChangePerSecond * Time.deltaTime;
      }
      if (Input.GetKey(GameManager.leftButton))
      {
        horizontalDecrease = GameManager.kickArrowAngleChangePerSecond * Time.deltaTime;
      }

      float verticalChange = verticalIncrease - verticalDecrease;
      float horizontalChange = horizontalIncrease - horizontalDecrease;

      Vector3 currKickArrowEulerAngles = kickArrow.transform.localEulerAngles;

      float newKickArrowEulerAngleX = currKickArrowEulerAngles.x + verticalChange;
      if (verticalChange < 0) {
        if (newKickArrowEulerAngleX > GameManager.kickArrowMinVerticalAngle && Mathf.Abs(newKickArrowEulerAngleX) <= GameManager.kickArrowMaxVerticalAngle || Mathf.Abs(newKickArrowEulerAngleX) > GameManager.kickArrowMaxVerticalAngle && Mathf.Abs(newKickArrowEulerAngleX) < 360 - GameManager.kickArrowMaxVerticalAngle) {
          newKickArrowEulerAngleX = -GameManager.kickArrowMaxVerticalAngle;
        }
      } else if (verticalChange > 0) {
        if (newKickArrowEulerAngleX > GameManager.kickArrowMinVerticalAngle && newKickArrowEulerAngleX < GameManager.kickArrowMaxVerticalAngle) {
          newKickArrowEulerAngleX = GameManager.kickArrowMinVerticalAngle;
        }
      }

      float newKickArrowEulerAngleY = currKickArrowEulerAngles.y + horizontalChange;
      // first condition value arbitrary, inserted to protect against arrow jump
      if (Mathf.Abs(newKickArrowEulerAngleY) < 60 && newKickArrowEulerAngleY < 360 - GameManager.kickArrowMaxHorizontalAngle && newKickArrowEulerAngleY > GameManager.kickArrowMaxHorizontalAngle) {
        newKickArrowEulerAngleY = GameManager.kickArrowMaxHorizontalAngle;
      }
      // first condition value arbitrary, inserted to protect against arrow jump
      if (Mathf.Abs(newKickArrowEulerAngleY) > 300 && newKickArrowEulerAngleY < 360 - GameManager.kickArrowMaxHorizontalAngle && newKickArrowEulerAngleY > GameManager.kickArrowMaxHorizontalAngle) {
        newKickArrowEulerAngleY = -GameManager.kickArrowMaxHorizontalAngle;
      }

      kickArrow.transform.localEulerAngles = new Vector3(newKickArrowEulerAngleX, newKickArrowEulerAngleY, currKickArrowEulerAngles.z);
    }

    if (Input.GetKeyUp(GameManager.kickBallButton) || (GameManager.ballPlayerKickableDistance < Vector3.Distance(player.transform.position, GameManager.ball.transform.position))) {
      // ball kick resolution
      if (Input.GetKeyUp(GameManager.kickBallButton) && (GameManager.ballPlayerKickableDistance >= Vector3.Distance(player.transform.position, GameManager.ball.transform.position))) {
        Rigidbody brb = GameManager.ball.GetComponent<Rigidbody>();
        float kickScale = kickArrow.transform.localScale.x;
        Vector3 kickDirection = kickArrow.transform.forward;

        hasBall = false;
        brb.AddForce(kickDirection * GameManager.defaultBallKickForce * kickScale);
      }

      kickArrow.SetActive(false);
      increaseKickArrowScale = true; 
    }
  }
}
