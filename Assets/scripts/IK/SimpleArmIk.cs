using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimpleArmIK : MonoBehaviour
{
    public Transform rightHandTarget;
    public Transform rightElbowHint;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (rightHandTarget != null && rightElbowHint != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1f);

            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
            animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowHint.position);
        }
    }
}

