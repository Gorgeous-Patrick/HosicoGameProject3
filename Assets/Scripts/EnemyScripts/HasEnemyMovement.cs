using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HasEnemyMovement : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent nav;
    private HasEnemyVision vision;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform saveTar;
    [SerializeField] private Transform[] checkpoints = { };
    [SerializeField] private float waitTimer = 80f;
    private bool playerFound = false;
    private bool isEnemyDisabled = false;

    [SerializeField] private int currIndex;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        vision = GetComponent<HasEnemyVision>();
        if (checkpoints.Length == 1) Debug.LogError("YOU FORGOT TO ADD CHECKPOINTS YOU BIG LUMMOX");
        currIndex = 0;
    }

    private void Start()
    {
        UpdateCheckpoint();
        saveTar = targetTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnemyDisabled) return;
        if (vision.currTimer >= 30)
        {
            nav.SetDestination(player.position);
        }
        else
        {
            if(checkpoints.Length > 1)
            {
                if (Vector3.Distance(targetTransform.position, transform.position) < 2)
                    UpdateCheckpoint();
                nav.SetDestination(targetTransform.position);
            }
        }
        /*else
        {
            // ... disable the nav mesh agent.
            nav.enabled = false;
        } */
    }

    void UpdateCheckpoint()
    {
        if(checkpoints.Length > 1)
        {
            currIndex += 1;
            if (currIndex == checkpoints.Length)
                currIndex = 0;
            targetTransform = checkpoints[currIndex];
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            BlindEnemy();
            Destroy(collision.gameObject);
        }
    }

    public void BlindEnemy()
    {
        isEnemyDisabled = true;
        nav.enabled = false;
        StartCoroutine(WaitToUnblind());
    }

    private IEnumerator WaitToUnblind()
    {
        yield return new WaitForSeconds(5f);
        isEnemyDisabled = false;
        nav.enabled = true;
    }
}




/* public void UpdateTarget()
    {
        saveTar = targetTransform;
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerFound = true;
    }

    public void lostTarget()
    {
        playerFound = false;
        targetTransform = saveTar;
    }*/