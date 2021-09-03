using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
	public int maxHealth;

	[SyncVar(hook = "OnHealthChanged")]
	public int currentHealth;
	public Text HealthScore;

	// Use this for initialization
	void Start () {
		HealthScore.text = currentHealth.ToString();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDamage(int howMuch){

		if (!isServer)
		{
			return;
		}
			var newHealth = currentHealth - howMuch;
			if (newHealth <= 0)
			{
			currentHealth = maxHealth;
			//Debug.Log("Dead");
			RpcDeath();
			}
			else
			{
				currentHealth = newHealth;
			//HealthScore.text = currentHealth.ToString();
		}
		//}
	}

	void OnHealthChanged(int updatedHealth)
    {
		HealthScore.text = updatedHealth.ToString();
	}

	[ClientRpc]
	void RpcDeath()
    {
		if(isLocalPlayer)
        {
			//transform.position = Vector3.zero;
			var spawnPoints = FindObjectsOfType<NetworkStartPosition>();
			var chosenPoint = Random.Range(0, spawnPoints.Length);
			transform.position = spawnPoints[chosenPoint].transform.position;

        }
    }
}
