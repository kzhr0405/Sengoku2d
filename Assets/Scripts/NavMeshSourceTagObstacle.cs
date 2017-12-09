using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshSourceTagObstacle : MonoBehaviour {
	//このスクリプトは何もしない
	//存在の有無でフラグとして扱われる
	//NaviMeshSourceTagのisWallフラグは動的に生成（AddComponent）後にセットしても遅いようなので
	//事前に親オブジェクトにこのスクリプトをアタッチしておくことで
	//onEnable時にこのスクリプトの存在有無をチェックして障害物と見なすかどうかを決定する
}
