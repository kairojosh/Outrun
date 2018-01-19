
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {
    //References
    public LayerMask bulletMask;
    public LayerMask playerMask;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
    public GameObject target;
	//Handling
	public float Recoil = 1f;
	public float ReloadSpeed = 1f;
	public float bulletVelocity = 0.5f;
	public float bulletDuration = .5f;
	public float velocity = .5f;
    public float rotationSpeed = 1000f;
    public float stopDistance = 1f;

    public Transform player;
    public GridScript pathReference;
    //Local Variables
    private Quaternion targetRotation; 
	private float TimeStamp;
	private float speed = 5f;
    private CharacterController controller;
    public GameObject explosionReference;
	public Rigidbody attachedRigidBody;


	void Explode(){
        Quaternion rotation = Quaternion.Euler(90, 0, 0);
        Vector3 explosionPosition = new Vector3(transform.position.x, 2, transform.position.y);
        var explode = Instantiate(explosionReference, explosionPosition , rotation);
        Destroy(explode, 100);

	}


     void Start()
    {
        
        controller = gameObject.GetComponent<CharacterController>();
        TimeStamp = Time.time;

    }

    void Fire(){
		var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
		bullet.GetComponent<Rigidbody> ().velocity = bulletSpawn.transform.forward * bulletVelocity;

		Destroy (bullet, bulletDuration);
		//When it's time to start firing again
		TimeStamp = Time.time + ReloadSpeed;
	}
	 
    private void FixedUpdate()
    {
        //The bullet went past the enemy too fast, shorter than a frame, so I had to fix a frame
        if (Physics.CheckSphere(gameObject.transform.position, gameObject.transform.lossyScale.x, bulletMask))
        {
            Explode();
            gameObject.SetActive(false);
            Destroy(gameObject);
            Debug.Log("Collision");
        }
        if (Time.time >= TimeStamp)
        {
            if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, 10, playerMask))
            {
                Fire();

            }


        }
    }


	IEnumerator movement (){
        for (int i = 0; i < pathReference.path.Count - 1; i++)
        {
            //try to get the look difference changed
            Vector3 difference = player.position - gameObject.transform.position;
            difference.Normalize();
            targetRotation = Quaternion.LookRotation(difference);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);


            Vector3 targetPosition = new Vector3(pathReference.path[i].position.x, pathReference.path[i].position.y, pathReference.path[i].position.z);
            Debug.Log(gameObject.transform.position);
            Debug.Log(targetPosition);
			gameObject.transform.position = Vector3.Lerp((gameObject.transform.position), (targetPosition),velocity);
            yield return new WaitForSeconds(0.2f);
            
        }
	}


    private void Update () {

     
          
        //Firing sort of works, but breaks occasionally, needs fixing

        if (pathReference.path != null)
        {
				StartCoroutine(movement());

           /* for (int i = 0; i < pathReference.path.Count - 1; i++)
            {
                //try to get the look difference changed
                Vector3 difference = player.position - gameObject.transform.position;
                difference.Normalize();
                targetRotation = Quaternion.LookRotation(difference);
                transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);


				Vector3 targetPosition = new Vector3 (pathReference.path[i].position.x, pathReference.path[i].position.y, pathReference.path[i].position.z);
				Debug.Log (gameObject.transform.position);
				Debug.Log (targetPosition);
				gameObject.transform.position = Vector3.Lerp((gameObject.transform.position),(targetPosition),velocity);

                //Vector3 nodeAIdifference= (pathReference.path[i].position - gameObject.transform.position).normalized;
                //controller.Move(nodeAIdifference * Time.deltaTime * velocity);

            }*/
				
        }
    
     
      /* implement this in college
        //Determines if the player is moving backwards, sideways or forwards
        if (Vector3.Dot(transform.forward, motion) > .4)
		{
			speed = ForwardsVelocity;
		}
		else if(Vector3.Dot(transform.forward, motion) < -.6)
		{
			speed = BackwardsVelocity;
		}
		else
		{
			speed = SidewaysVelocity;
		}
        */

	}
}
