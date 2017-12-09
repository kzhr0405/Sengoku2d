using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)] //コンポーネントの生成順を標準（=0）よりも遅くする
public class NavMeshMapWall : MonoBehaviour {
	
	//テスト用のI/F、単体テストしたいときに空のGameObjectにアタッチして利用する
	public GameObject wallPrefab;
	void Start() {
		if(wallPrefab != null){
			var obj = Instantiate(wallPrefab);
			makeSceneFloor(obj);
		}
	}

	//Wallインスタンスを渡すと、シーン用の床を作成する
	public static void makeSceneFloor(GameObject wall) {
		var min = new Vector2(Mathf.Infinity, Mathf.Infinity);
		var max = -min;

		//外周を取り巻く４つのオブジェクトを壁オブジェクトに設定する
		int count = wall.transform.childCount;
		for(int i = 0; i < count; i++){
			GameObject obj = wall.transform.GetChild(i).gameObject;
			if(obj == null) continue;
			obj.AddComponent<NavMeshSourceTagObstacle>();//壁として認識させる
			NavMeshSourceTag tag = obj.AddComponent<NavMeshSourceTag>();
//			tag.isWall = true;//このタイミングでフラグをセットしても遅いので効かない

			//コライダーの位置から最大と最小を更新
			Collider2D collider = obj.GetComponent<BoxCollider2D>();
			if(collider != null){
				var minmax = NavMeshUtils2D.AdjustMinMax(collider, min, max);
				min = minmax[0];
				max = minmax[1];
			}
		}

		//4つの外周オブジェクトを内包する大きなBoxCollider2Dを持つオブジェクトを作成し
		//それを移動可能範囲を示すオブジェクトとして設定する
		float w = max.x - min.x;
		float h = max.y - min.y;
		float offsetX = w/2 + min.x;
		float offsetY = h/2 + min.y;
		var floor = wall.AddComponent<BoxCollider2D>();
		floor.size = new Vector2(w, h);
		floor.offset = new Vector2(offsetX, offsetY);
		wall.AddComponent<NavMeshSourceTag>();

		//bakeエリアを移動可能範囲オブジェクトの大きさに合わせる
		var size = LocalNavMeshBuilder.m_Size;
		size.x = w + (offsetX * 2);
		size.z = h + (offsetY * 2);
		LocalNavMeshBuilder.m_Size = size;

	}
}
