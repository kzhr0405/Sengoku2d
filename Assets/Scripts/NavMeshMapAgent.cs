using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMapAgent : MonoBehaviour {

	//インスタンス化された武将オブジェクトにNavMeshAgent2Dを追加するヘルパー関数
	public static void addNavMeshAgent2D(GameObject prefab) {
		if(!GameScene.isUseNavigation){
			return;
		}

		float speed = 1;
		float radius = 1;

		//移動速度を反映させる
		var unitMover = prefab.GetComponent<UnitMover>();
		var homing = prefab.GetComponent<Homing>();
		var homingLong = prefab.GetComponent<HomingLong>();
		if(unitMover != null){
			speed = unitMover.speed;
		}
		if(homing != null){
			speed = homing.speed;
		}
		if(homingLong != null){
			speed = homingLong.speed;
		}

		//コライダーの大きさを反映させる
		var collider = prefab.GetComponent<CircleCollider2D>();
		if(collider != null){
			radius = collider.radius;
		}

		var agent = prefab.AddComponent<NavMeshAgent2D>();

		agent.speed = speed;
		agent.angularSpeed = 120;//default
		agent.acceleration = 8;//default
		agent.radius = radius;
		
	}


}
