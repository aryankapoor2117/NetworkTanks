using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TankController : NetworkBehaviour {
	public float MoveSpeed = 150.0f;
	public float RotateSpeed = 3.0f;
	public Color LocalPlayerColor;
	public GameObject ShotPrefab;
	public Transform ShotSpawnTransform;
	public float ShotSpeed;
	public float ReloadRate = 0.5f;
	private float _nextShotTime;

	// Use this for initialization
	void Start () {

	}

	public override void OnStartLocalPlayer(){
		var tankParts = GetComponentsInChildren<MeshRenderer> ();
		foreach (var part in tankParts) {
			part.material.color = LocalPlayerColor;
		}
	}
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * RotateSpeed;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);

		if (Input.GetKeyDown (KeyCode.Space) && Time.time > _nextShotTime) {
			CmdFire ();
		}
	}

	[Command]
	void CmdFire(){
		_nextShotTime = Time.time + ReloadRate;
		var bullet = Instantiate (ShotPrefab, ShotSpawnTransform.position, Quaternion.identity) as GameObject;
		bullet.GetComponent<Rigidbody> ().velocity = transform.forward * ShotSpeed;

		NetworkServer.Spawn (bullet);
	}

	void OnCollisionEnter(Collision collision){
		var other = collision.gameObject;
		try{
			var causeDamageScript = other.GetComponent<CauseDamage>();
			var totalDamage = causeDamageScript.GetDamage();
			var healthScript = GetComponent<Health>();
			healthScript.TakeDamage(totalDamage);
			Debug.Log("Shot");
		} catch {
			Debug.Log ("Something hit us but didn't do any damage.");
		}
	}
}
