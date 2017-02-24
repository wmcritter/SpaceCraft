using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	public GameObject Light;
	public GameObject Beam;
	public float range = 20.0f;
	
	LineRenderer lr;
	
	void Start () {
		lr = Beam.GetComponent<LineRenderer>();
	}
	
	bool isFiring;
	// Update is called once per frame
	void Update () 
	{
		//Debug.DrawLine(gameObject.transform.position, gameObject.transform.position +(gameObject.transform.up * 3.0f), Color.cyan);
		if (Input.GetMouseButton(1))
		{
			if (!isFiring)
			{
				isFiring = true;
				Beam.SetActive(true);
				Light.SetActive(true);
				gameObject.GetComponent<AudioSource>().Play();
			}
			float drawRange = range;
			Vector3 laserPos = gameObject.transform.position + (gameObject.transform.up * 0.1f);
			//ray cast to see if we hit anything. if so, only draw to the thing we hit
			Ray ray = new Ray(laserPos, gameObject.transform.up);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, range))
			{
				drawRange = hit.distance;
				//do damage to enemy
				if (hit.collider.tag.Equals ("Enemy"))
				{
					//hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(GameObject.FindGameObjectWithTag("Player"), 0.5f * Time.deltaTime);
				}
			}
			
			lr.SetPosition(0, laserPos);
			lr.SetPosition(1, laserPos + (gameObject.transform.up * drawRange));
			       
			       
		}
		else if (isFiring)
		{
			isFiring = false;
			Beam.SetActive(false);
			Light.SetActive(false);
			gameObject.GetComponent<AudioSource>().Stop();
		}
		
	}
}
