using UnityEngine;

public class Redd : MonoBehaviour
{
    public Animator animator;
    public SphereCollider col;

    private void Start()
    {
        animator.SetBool("IsAlert", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("IsAlert", true);
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("IsAlert", false);
    }
}
