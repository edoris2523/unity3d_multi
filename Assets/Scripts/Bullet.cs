using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Bullet : NetworkBehaviour {

	Rigidbody m_rigidbody;
	Collider m_collider;
	List<ParticleSystem> m_allParticles;

	public int m_speed = 100;
	public int m_bounces = 5;
	public float m_damage = 1f;
	public float m_lifetime = 5f;
	public ParticleSystem m_explosionFX;
	public List<string> m_bounceTags;
	public List<string> m_collisionTags;

	// Use this for initialization
	void Start () {
		m_allParticles = GetComponentsInChildren<ParticleSystem> ().ToList ();
		m_rigidbody = GetComponent <Rigidbody> ();
		m_collider = GetComponent<Collider> ();
		StartCoroutine ("SelfDestruct");
	}

	IEnumerator SelfDestruct(){
		yield return new WaitForSeconds (m_lifetime);
		Explode ();
	}

	void Explode(){
		m_collider.enabled = false;
		m_rigidbody.velocity = Vector3.zero;
		m_rigidbody.Sleep ();

		foreach (ParticleSystem ps in m_allParticles) {
			ps.Stop ();
		}

		if (m_explosionFX != null) {
			m_explosionFX.transform.parent = null;
			m_explosionFX.Play ();
		}

		if (isServer) {
			Destroy (gameObject);
			foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>()) {
				m.enabled = false;
			}
		}
	}

	void OnCollisionExit(Collision collision){
		if(m_rigidbody.velocity != Vector3.zero){
			transform.rotation = Quaternion.LookRotation (m_rigidbody.velocity);
		}
	}

	void OnCollisionEnter(Collision collision){
		CheckCollisions (collision);
		if (m_bounceTags.Contains (collision.gameObject.tag)) {
			if (m_bounces <= 0) {
				Explode ();
			}
			m_bounces--;
		}
	}

	void CheckCollisions(Collision collision){
		if (m_collisionTags.Contains (collision.collider.tag)) {
			Debug.Log ("Hit");
			Explode ();
			PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth> ();

			if (playerHealth != null) {
				playerHealth.Damage (m_damage);
			}
		}
	}
}
