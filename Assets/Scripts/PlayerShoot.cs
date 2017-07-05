﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	public Rigidbody m_bulletPrefab;
	public Transform m_bulletSpawn;

	public int m_shotsPerBurst = 3;
	public float m_reloadTime = 1f;
	int m_shotsLeft;
	bool m_isReloading;

	// Use this for initialization
	void Start () {
		m_shotsLeft = m_shotsPerBurst;
		m_isReloading = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Shoot ();
		}
	}

	public void Shoot(){
		if (m_isReloading || m_bulletPrefab == null) {
			return;
		}

		Bullet bullet = null;
		bullet = m_bulletPrefab.GetComponent<Bullet> ();

		Rigidbody rbody = Instantiate (m_bulletPrefab, m_bulletSpawn.position, m_bulletSpawn.rotation) as Rigidbody;
		if (rbody != null) {
			rbody.velocity = bullet.m_speed * m_bulletSpawn.transform.forward;
		}

		m_shotsLeft--;
		if (m_shotsLeft <= 0) {
			StartCoroutine ("Reload");
		}
	}

	IEnumerator Reload(){
		m_shotsLeft = m_shotsPerBurst;
		m_isReloading = true;
		yield return new WaitForSeconds (m_reloadTime);
		m_isReloading = false;
	}
}
