using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Quaternion TouchRotation; // 최종회전 값
    private Ray InputRay; // 레이 변수
    private RaycastHit Hit; // 정보가 들어갈 변수
    private LayerMask Layer; // 레이어마스크 변수 
    private float RayLength = 100; // 레이 길이 변수

    public float speed = 6f;            // The speed that the player will move at.
    Vector3 movement;                   // The vector to store the direction of the player's movement.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    Animator anim;                      // Reference to the animator component.

    // Start is called before the first frame update
    void Start()
    {   
        // Set up references.
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControl();
    }


    void PlayerControl()
    {
        if (Application.platform == RuntimePlatform.Android) // 현재 플렛폼이 안드로이드라면
        {
            InputRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); // 사용할 레이의 위치 생성(터치)
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                InputRay = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스
            }
        }
        if (Physics.Raycast(InputRay, out Hit, RayLength)) // 레이생성(Hit= 변수에 결과값 저장, RayLength= 레이 길이, Layer= 레이가 충돌된 오브젝트 레이어 가 해당레이어인경우에만 캐치함)
        {
            if (Hit.transform.name == "Wall")
            {
                return;
            }
            Vector3 playerToMouse = Hit.point - transform.position;
            playerToMouse.y = 0f;  // Y방향은 사용하지 않기 때문에 0
            TouchRotation = Quaternion.LookRotation(playerToMouse); // LookRotation함수를 이용해 회전
            transform.rotation = TouchRotation;



            if (Vector3.Distance(transform.position, Hit.point) > 0.5f)
            {
                transform.position = Vector3.Lerp(transform.position, Hit.point, Time.deltaTime);
                anim.SetBool("IsWalking", true);
            }
            else
            {
                anim.SetBool("IsWalking", false);

            }
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }
    }
}
