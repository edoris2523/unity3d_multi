using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : NetworkBehaviour {

	// Player의 Rigidbody
	public Rigidbody m_rigidbody;

	// Chassis, Turret의 Transform을 inspector에서 불러오기 위해 만듬
	public Transform m_chassis;
	public Transform m_turret;

	// Player의 이동속도와 Turret, Chassis의 회전속도
	public float m_moveSpeed = 100f;
	public float m_chassisRotateSpeed = 1f;
	public float m_turretRotateSpeed = 3f;

	// Use this for initialization
	void Start () {
		m_rigidbody = GetComponent<Rigidbody> ();
	}


	public void MovePlayer(Vector3 dir){
		Vector3 moveDir = dir * m_moveSpeed * Time.deltaTime;
		m_rigidbody.velocity = moveDir;
	}

	public void FaceDir(Transform xform, Vector3 dir, float rotSpeed){
		if (dir != Vector3.zero) {
			Quaternion desiredRot = Quaternion.LookRotation (dir);

			xform.rotation = Quaternion.Slerp (xform.rotation, desiredRot, rotSpeed * Time.deltaTime);
		}
	}

	public void RotateChassis(Vector3 dir){
		FaceDir (m_chassis, dir, m_chassisRotateSpeed);
	}

	public void RotateTurret(Vector3 dir){
		FaceDir (m_turret, dir, m_turretRotateSpeed);
	}
}
