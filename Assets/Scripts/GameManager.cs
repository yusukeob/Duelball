using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  private static GameObject mainCamera;
  private static GameObject player;
  public static GameObject ball;
  private float cameraDistance = 15f;

  public static KeyCode forwardButton = KeyCode.UpArrow;
  public static KeyCode backwardButton = KeyCode.DownArrow;
  public static KeyCode rightButton = KeyCode.RightArrow;
  public static KeyCode leftButton = KeyCode.LeftArrow;
  public static KeyCode tackleButton = KeyCode.W;
  public static KeyCode engageBallButton = KeyCode.A;
  public static KeyCode kickBallButton = KeyCode.D;

  public static float ballPlayerKickableDistance = 12f;
  public static float playerIsKickingGameSpeedModifier = 0.2f;
  public static float kickArrowMaxScale = 3f;
  public static float kickArrowMinScale = 1f;
  public static float kickArrowScaleChangePerSecond = 1f;
  public static float kickArrowMaxVerticalAngle = 60f;
  public static float kickArrowMinVerticalAngle = 0f;
  public static float kickArrowMaxHorizontalAngle = 45f;
  public static float kickArrowAngleChangePerSecond = 20f;

  public static float ballPlayerPickupDistance = 12f;
  public static float ballPlacementDistance = 3f;

  // Start is called before the first frame update
  void Start()
  {
    mainCamera = GameObject.Find("Main Camera");
    player = GameObject.Find("Player");
    ball = GameObject.Find("Ball");
  }

  // Update is called once per frame
  void Update()
  {
      mainCamera.transform.position = player.transform.position - player.transform.forward * cameraDistance;
      mainCamera.transform.LookAt (player.transform.position);
      mainCamera.transform.rotation = Quaternion.Euler(30f, mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);
      mainCamera.transform.position = new Vector3 (mainCamera.transform.position.x, mainCamera.transform.position.y + 22f, mainCamera.transform.position.z - 5f);
  }
}
