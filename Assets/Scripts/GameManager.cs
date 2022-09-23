using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  private static GameObject mainCamera;
  private static GameObject player;
  private float cameraDistance;

  // Start is called before the first frame update
  void Start()
  {
    mainCamera = GameObject.Find("Main Camera");
    player = GameObject.Find("Player");
    cameraDistance = 15f;
  }

  // Update is called once per frame
  void Update()
  {
      mainCamera.transform.position = player.transform.position - player.transform.forward * cameraDistance;
      mainCamera.transform.LookAt (player.transform.position);
      mainCamera.transform.rotation = Quaternion.Euler(30f, mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);
      mainCamera.transform.position = new Vector3 (mainCamera.transform.position.x, mainCamera.transform.position.y + 20f, mainCamera.transform.position.z);
  }
}
