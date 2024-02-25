using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{ 
    private Camera mainCamera;
    private NavMeshAgent agent;
    private Animator animator;
    

    void Start()
    {
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hit; 

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Terrain")) {
                    agent.SetDestination(hit.point);
                }
            }
        }
        if (agent.remainingDistance > 0.1f) {
            animator.SetBool("IsRun", true);
        } else animator.SetBool("IsRun", false);
        
    }

   
   
}

