using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerFollow : MonoBehaviour
{
    private bool frozen = false;

    public Transform Target;
    [SerializeField] private Rigidbody2D rb;
    public float speed;
    public float stoppingDistance;
    private bool ducollision = false;
    private float hAxis;
    private float vAxis;
    private bool following;
    public Animator animator;
    private NavMeshAgent agent;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            frozen = false;
            following = false;
            ScoreScript.ducklings = 0;
        }
    }

    void FixedUpdate(){
        Vector2 targetPosition = Target.position;
        Vector2 currentPosition = transform.position;
        if(!frozen){
            rb.velocity = new Vector2();
        }
        float distance = Vector2.Distance(currentPosition, targetPosition);
        if(distance <= stoppingDistance){
            rb.velocity = new Vector2();
            WaddlingControl.halt = true;
        }  

        if(WaddlingControl.halt && ducollision && distance < 3f){
            rb.velocity = new Vector2();
        }
        else if(frozen && distance > stoppingDistance){
            //transform.right = Target.position - transform.position;
            Vector2 directionOfTravel = targetPosition - currentPosition;
            directionOfTravel.Normalize();

            agent.SetDestination(Target.transform.position);
            rb.MovePosition(currentPosition + (directionOfTravel * speed * Time.deltaTime));
            ducollision = false;
        }
        if(distance > 7f && following)
        {
            frozen = false;
            following = false;
            ScoreScript.ducklings--;
        }
        hAxis = transform.position.x - Target.transform.position.x;
        vAxis = transform.position.y - Target.transform.position.y;
        if(frozen){
            if (Mathf.Abs(hAxis) > Mathf.Abs(vAxis))
            {
                if(hAxis < 0)
                    animator.Play("Duckling_Walk_Right");
                else
                    animator.Play("Duckling_Walk_Left");
            }
            else
            {
                if(vAxis < 0)
                    animator.Play("Duckling_Walk_Up");
                else
                    animator.Play("Duckling_Walk_Down");
            }
        }
        else
            animator.Play("Duckling_Idle");
    }
    IEnumerator checkP()
    {
        yield return new WaitForSeconds(0.5f);
        rb.velocity = new Vector2();
        frozen = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !frozen)
        {
            ScoreScript.ducklings++;
            frozen = true;
            following = true;
        }
        if(collision.gameObject.tag == "CheckPoint")
        {
            StartCoroutine(checkP());
        }
        if(collision.gameObject.tag == "duckling")
        {
            ducollision = true;
        }
    }

}
