using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float setSpeed;
    private float speed;
    public float speedLimit;
    public Animator animator;
    private string temp = "How are you the messiah when you haven't even saved all your own children?";
    private string temp2 = "Come back when your a little, mmmm holier";

    [SerializeField] private Rigidbody2D rb;
    float vertical;
    float horizontal;
    public float textSpeed;
    public TMP_Text dial;
    [SerializeField] public Transform church;

    // Update is called once per frame
    void Start(){
        speed = setSpeed;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Vector3 pos = transform.position;
        if(Input.GetKey("left shift")){
            speed = speed*2;
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if(horizontal != 0 && vertical != 0){
            horizontal *= speedLimit;
            vertical *= speedLimit;
        }
        if(horizontal != 0 || vertical != 0){
            WaddlingControl.halt = false;
        }
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);

        speed = setSpeed;

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetFloat("Speed", transform.position.sqrMagnitude);

        Vector2 targetPosition = church.position;
        Vector2 currentPosition = transform.position;
        float distance = Vector2.Distance(currentPosition, targetPosition);
        if(Input.GetKeyDown(KeyCode.E) && distance < 6f){
            if(ScoreScript.ducklings == 6){
                SceneManager.LoadScene("Ascend");
            }
            else{

                StartCoroutine(TypeLine());
            }       
        }
    }
    
    private IEnumerator TypeLine()
    {
        dial.text = string.Empty;
        StopAllCoroutines();
        foreach(char c in temp.ToCharArray())
        {
            dial.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        yield return new WaitForSeconds(2f);
        dial.text = string.Empty;
        foreach(char c in temp2.ToCharArray())
        {
            dial.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        yield return new WaitForSeconds(2f);
        dial.text = string.Empty;
    }
}
