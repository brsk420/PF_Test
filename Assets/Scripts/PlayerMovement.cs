using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость перемещения игрока
    
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
        if (Input.GetMouseButtonDown(0)) // Проверяем, был ли произведен клик левой кнопкой мыши
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Создаем луч от камеры к позиции мыши
            RaycastHit hit; // Переменная для хранения информации о пересечении луча с объектом

            if (Physics.Raycast(ray, out hit)) // Проверяем, пересек ли луч объект
            {
                // Если луч пересек террейн, перемещаем игрока к точке пересечения
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

