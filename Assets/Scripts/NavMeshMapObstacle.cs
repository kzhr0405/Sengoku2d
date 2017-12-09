using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)] //コンポーネントの生成順を標準（=0）よりも遅くする
public class NavMeshMapObstacle : MonoBehaviour {
	//テスト用のI/F、単体テストしたいときに空のGameObjectにアタッチして利用する
	public GameObject mapFrontPrefab;
	void Start() {
		if(mapFrontPrefab != null){
			var obj = Instantiate(mapFrontPrefab);
			makeSceneMapFront(obj);
			//障害物の配置が完了したので、ここでNavMeshを更新する
			LocalNavMeshBuilder builder = GameObject.FindObjectOfType<LocalNavMeshBuilder>();
			if(builder != null){
				builder.UpdateNavMesh(true);
			}		}
	}

	//MapFront系インスタンスを渡すと、子オブジェクトから障害物エリアを生成する
	public static void makeSceneMapFront(GameObject mapFront) {

		int count = mapFront.transform.childCount;
		for(int i = 0; i < count; i++){
			GameObject obj = mapFront.transform.GetChild(i).gameObject;
			if(obj == null) continue;

			//アタッチされているColliderをチェック
			//CircleCollider2DとCapsuleCollider2Dを
			//BoxCollider2Dに差し換える

			CircleCollider2D circleColider = obj.GetComponent<CircleCollider2D>();
			CapsuleCollider2D capsuleCollider = obj.GetComponent<CapsuleCollider2D>();
			if(circleColider == null && capsuleCollider == null){
				continue;//どちらもアタッチされていないオブジェクトは無視する
			}

			#if false //CircleCollider（mapFront3の山）もそのままとする
			if(circleColider != null){
				//半径とオフセットを取得
				float radius = circleColider.radius;
				Vector2 pos = circleColider.offset;
				Destroy(circleColider);

				//横倒しの長方形に差し換え
				BoxCollider2D box = obj.AddComponent<BoxCollider2D>();
				box.offset = pos;
				box.size = new Vector2(radius * 2, radius);
			}
			#endif
			#if false //CapsuleCollider2D(山岳地帯の山)はそのままとし、NavMeshの方だけCubeにする
			if(capsuleCollider != null){
				//半径とオフセットを取得
				Vector2 size = capsuleCollider.size;
				Vector2 pos = capsuleCollider.offset;
				Destroy(capsuleCollider);

				//同じ大きさの長方形に差し換え
				BoxCollider2D box = obj.AddComponent<BoxCollider2D>();
				box.offset = pos;
				box.size = size;
			}
			#endif

			//isWallフラグを立てる代わりにスクリプトをアタッチする
			obj.AddComponent<NavMeshSourceTagObstacle>();

			NavMeshSourceTag tag = obj.AddComponent<NavMeshSourceTag>();
			tag.isWall = true;
		}

	}

}
