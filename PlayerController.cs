using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Vector3 lookRotationViewingVector = Vector3.zero;

    public float moveSpeed;
    public float jumpForce;
    static Animator anim;
    public CharacterController controller;
    private Vector3 move;
    public float gravityScale;

    public Transform pivot;
    public float rotateSpeed;

    public GameObject playerModel;

    public Text winText;

    public AudioSource jumpSound;
    public AudioSource winSound;

    void Start()
    {  // Use this for initialization
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        winText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        // Move
        //move = new Vector3(Input.GetAxis("Horizontal")*moveSpeed, move.y, Input.GetAxis("Vertical")*moveSpeed);  // Does not move player based on direction player is facing
        float yStore = move.y;
        move = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        move = move.normalized * moveSpeed;
        move.y = yStore;

        // Jump
        if (controller.isGrounded)
        {
            move.y = 0f;  // Prevent gravity from increasing while on ground
            if (Input.GetButtonDown("Jump"))
            {
                move.y = jumpForce;
                jumpSound.Play();
            }

        }
        move.y = move.y + (Physics.gravity.y * gravityScale);
        controller.Move(move * Time.deltaTime);  // Apply move, jump, and gravity

        // Move the player in different directions based on camera look direction
        lookRotationViewingVector = new Vector3(move.x, 0, move.z);  // Enter only if look rotation viewing vector is not zero
        if ((lookRotationViewingVector != Vector3.zero))
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        // Animate run
        if (move.x != 0f || move.z != 0f)
        {
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }
    }

    // Wheel inertia does not affect player movement

    // Win
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            Destroy(other.gameObject);  // Can deactivate instead of removing w/ other.gameObject.SetActive(false);  Destroy w/ Destroy(other.gameObject);
            winText.color = Color.yellow;
            winText.text = "GOAL!";
            winSound.Play();
        }
    }
}
