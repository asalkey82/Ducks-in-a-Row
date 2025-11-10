using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject player;

    private bool hasLineOfSight = false;
    private bool interest = true;
    private bool chasing = false;
    private bool dying = false;
    private float hAxis;
    private float vAxis;
    private NavMeshAgent agent;
    public Animator animator;

    void Start()
    {
        ScoreScript.hearts = 3;
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if(hasLineOfSight && interest && distance < 8f && !chasing)
        {
            StartCoroutine(chase());
        }
        if(interest)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.tag == "Player";
        }
        hAxis = transform.position.x - player.transform.position.x;
        vAxis = transform.position.y - player.transform.position.y;
        if(!dying && !chasing){
            if (Mathf.Abs(hAxis) > Mathf.Abs(vAxis))
            {
                if(hAxis < 0)
                    animator.Play("Devil_Walk_Right");
                else
                    animator.Play("Devil_Walk_Left");
            }
            else
            {
                if(vAxis < 0)
                    animator.Play("Devil_Walk_Up");
                else
                    animator.Play("Devil_Walk_Down");
            }
        }
        else if(!dying){
            if (Mathf.Abs(hAxis) > Mathf.Abs(vAxis))
            {
                if(hAxis < 0)
                    animator.Play("Devil_Glow_Right");
                else
                    animator.Play("Devil_Glow_Left");
            }
            else
            {
                if(vAxis < 0)
                    animator.Play("Devil_Walk_Up");
                else
                    animator.Play("Devil_Glow_Down");
            }
        }
    }
    private IEnumerator chase()
    {  
        chasing = true;
        agent.speed = 5f;
        //speed = 4f;
        yield return new WaitForSeconds(2);
        agent.speed = 0f;
        yield return new WaitForSeconds(2);
        agent.speed = 2f;
        chasing = false;
    }
    private IEnumerator consumption()
    {
        float hAxis = transform.position.x - player.transform.position.x;

        if(hAxis < 0)
            animator.Play("Devil_Eat_Right");
        else
            animator.Play("Devil_Eat_Left");

        yield return new WaitForSeconds(5);
        interest = true;
        dying = false;
    }

    private IEnumerator death()
    {
        float hAxis = transform.position.x - player.transform.position.x;

        if(hAxis < 0)
            animator.Play("Devil_Eat_Right");
        else
            animator.Play("Devil_Eat_Left");
        yield return new WaitForSeconds(5);
        interest = true;
        dying = false;
    }


     private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!dying){
            if(collision.gameObject.tag == "Player")
            {
                interest = false;
                dying = true;
                ScoreScript.hearts--;
                if(ScoreScript.hearts == 0)
                    SceneChange.GoToScene("End");
                StopAllCoroutines();
                StartCoroutine(death());
            }
            if(collision.gameObject.tag == "duckling")
            {
                interest = false;
                dying = true;
                ScoreScript.ducklings--;
                StopAllCoroutines();
                StartCoroutine(consumption());
                Destroy(collision.gameObject);
            }
        }
    }
}
