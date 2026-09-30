using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallMovement : MonoBehaviour
{
    [SerializeField]private float moveSpeed = 1f;
    Vector3 moveDirection = new(1f,0f,0f);
    private bool canMove = false;

    void Start()
    {
        StartCoroutine(StartDelayRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove == true)
        {
         transform.Translate(moveDirection * moveSpeed * Time.deltaTime);   
        }
    }

    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
        canMove = true;
    }

}
