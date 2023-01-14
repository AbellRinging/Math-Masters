using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{

    Animator animator;
    int isWalkingHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalkingFWD");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w");

        //if player presses w Key
        if (!isWalking && forwardPressed)
        {
            // then set the isWalking boolean to true
            animator.SetBool(isWalkingHash, true);
        }

        //if player is not pressing w Key
        if (isWalking && !forwardPressed)
        {
            // then set the isWalking boolean to false
            animator.SetBool(isWalkingHash, false);
        }
    }
}
