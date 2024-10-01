using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;
    FloatingJoystick joystick;

    public float speed = 100;

    Animator anim;

    [SerializeField] private List<Transform> papers = new List<Transform>();
    [SerializeField] private Transform paperPlace;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        joystick = FindObjectOfType<FloatingJoystick>();

        papers.Add(paperPlace);
    }

    private void Update()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            Vector3 pos = new Vector3(joystick.Horizontal, -9.8f * Time.deltaTime, joystick.Vertical);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(pos.x, 0, pos.z)), 2);


            cc.SimpleMove(pos * speed * Time.deltaTime);

            if (anim.GetBool("isRunning") == false)
            {
                anim.SetBool("isRunning", true);
            }
            if (anim.GetBool("isStanding") == true)
            {
                anim.SetBool("isStanding", false);
            }

        }
        else
        {
            if (anim.GetBool("isRunning") == true)
            {
                anim.SetBool("isRunning", false);
            }



            if (anim.GetBool("isStanding") == false)
            {
                anim.SetBool("isStanding", true);

            }


        }

        if (papers.Count > 1)
        {
            for (int i = 1; i < papers.Count; i++)
            {
                var firstPaper = papers.ElementAt(i - 1);
                var secondPaper = papers.ElementAt(i);

                secondPaper.position = new Vector3(Mathf.Lerp(secondPaper.position.x, firstPaper.position.x, Time.deltaTime * 15f),
                Mathf.Lerp(secondPaper.position.y, firstPaper.position.y + 0.17f, Time.deltaTime * 15f), firstPaper.position.z);
            }
        }


        if (Physics.Raycast(transform.position, transform.forward, out var hit, 1f))
        {
            Debug.DrawRay(transform.position, transform.forward * 1f, Color.green);
            if (hit.collider.CompareTag("table") && papers.Count < 9)
            {
                Debug.Log(hit);
                if (hit.collider.transform.childCount > 2)
                {
                    var paper = hit.collider.transform.GetChild(1);
                   // paper.rotation = Quaternion.Euler(paper.rotation.x, Random.Range(0f, 180f), paper.rotation.z);
                    papers.Add(paper);
                    paper.parent = null;
                }


            }

        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1f, Color.red);

        }
    }
}
