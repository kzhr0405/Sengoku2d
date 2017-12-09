using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

//NavMeshのソースとなるオブジェクトにアタッチして利用するスクリプト
//staticのリストでこのスクリプトがアタッチされたオブジェクトのBoxColliderを管理し
//LocalNavMeshBuildrクラスからCollect関数がコールされたら
//全BoxColliderからNavMeshBuildSourceのリストを生成して返す
// Tagging component for use with the LocalNavMeshBuilder
// Supports mesh-filter and terrain - can be extended to physics and/or primitives
[DefaultExecutionOrder(-200)]
public class NavMeshSourceTag : MonoBehaviour
{
    // Global containers for all active mesh/terrain tags
	public static List<BoxCollider> m_Box = new List<BoxCollider>();
	public static List<BoxCollider> m_BoxWall = new List<BoxCollider>();

	public bool isWall = false;
	private GameObject m_ObjectBy2D;//2Dオブジェクトが代わりに提供する3Dオブジェクト

	public static void clearStaticList(){
		//Debug.Log("clearStaticList  box:" + m_Box.Count + "  boxwall:" + m_BoxWall.Count);
		m_Box.Clear();
		m_BoxWall.Clear();
	}

	void onStart() 
	{
		if(GetComponent<NavMeshSourceTagObstacle>() != null){
			isWall = true;
		}
	}

    void OnEnable()
    {
		Collider2D collider = GetComponent<BoxCollider2D>();
		if(collider == null){
			collider = GetComponent<CapsuleCollider2D>();
			if(collider == null){
				collider = GetComponent<CircleCollider2D>();
			}
		}

		if(m_ObjectBy2D == null && collider != null){
#if true
			//スプライトの形状をそのまま床面CubeにしてくれるNavigation2D準拠のオブジェクト作成
			var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.isStatic = true;
			// position via offset and transformpoint
			var localPos = new Vector3(collider.offset.x, collider.offset.y, 0);
			var worldPos = collider.transform.TransformPoint(localPos);
			go.transform.position = new Vector3(worldPos.x, 0, worldPos.y);
			// scale depending on scale * collider size (circle=radius/box=size/...)
			if(collider.GetType() == typeof(BoxCollider2D) ){
				go.transform.localScale = NavMeshUtils2D.ScaleFromBoxCollider2D((BoxCollider2D)collider);
			}else if(collider.GetType() == typeof(CapsuleCollider2D) ){
				go.transform.localScale = NavMeshUtils2D.ScaleFromCapsuleCollider2D((CapsuleCollider2D)collider);
				//元のコライダーの大きさを小さくする
				CapsuleCollider2D capsuleCollider = (CapsuleCollider2D)collider;
				Vector2 size = capsuleCollider.size;
				size.x *=0.9f;
				size.y *=0.75f;
				capsuleCollider.size = size;
			}else if(collider.GetType() == typeof(CircleCollider2D) ){
				go.transform.localScale = NavMeshUtils2D.ScaleFromCircleCollider2D((CircleCollider2D)collider);
				//元のコライダーの大きさを小さくする
				CircleCollider2D circleCollider = (CircleCollider2D)collider;
				circleCollider.radius *= 0.75f;
			}
			// rotation
			go.transform.rotation = Quaternion.Euler(NavMeshUtils2D.RotationTo3D(collider.transform.eulerAngles));

			//BoxColliderだけ残して、保存
			Destroy(go.GetComponent<MeshFilter>());
			Destroy(go.GetComponent<MeshRenderer>());
			m_ObjectBy2D = go;
#endif
		}

		if(m_ObjectBy2D != null){
			NavMeshSourceTagObstacle obstacle = GetComponent<NavMeshSourceTagObstacle>();
			BoxCollider box = m_ObjectBy2D.GetComponent<BoxCollider>();
			if(isWall || obstacle != null){
				m_BoxWall.Add(box);
			}else{
				m_Box.Add(box);
			}
		}

    }

    void OnDisable()
    {
		if(m_ObjectBy2D != null){
			var box = m_ObjectBy2D.GetComponent<BoxCollider>();
			if(box != null){
				m_Box.Remove(box);
				m_BoxWall.Remove(box);
			}
		}
	}

	void OnDestroy()
	{
		if(m_ObjectBy2D){
			Destroy(m_ObjectBy2D);
			m_ObjectBy2D = null;
		}
	}

    // Collect all the navmesh build sources for enabled objects tagged by this component
    public static void Collect(ref List<NavMeshBuildSource> sources)
    {
        sources.Clear();
		collectBoxCollider(ref sources, ref m_Box, 0);//歩行エリアとして登録
		collectBoxCollider(ref sources, ref m_BoxWall, 1);//障害物として登録
    }

	private static void collectBoxCollider(
								ref List<NavMeshBuildSource> sources, 
								ref List<BoxCollider> box, 
								int area
								)
	{
		for (int i = 0; i < box.Count; i++) {
			BoxCollider collider = box[i];
			if(collider == null) continue;

			var s = new NavMeshBuildSource ();
			s.shape = NavMeshBuildSourceShape.Box;
			s.component = collider;
			var center = collider.transform.TransformPoint (collider.center);
			var scale = collider.transform.lossyScale;
			var size = new Vector3 (collider.size.x * Mathf.Abs (scale.x), collider.size.y * Mathf.Abs (scale.y), collider.size.z * Mathf.Abs (scale.z));
			s.transform = Matrix4x4.TRS (center, collider.transform.rotation, Vector3.one);
			s.size = size;
			s.area = area;
			sources.Add (s);
		}
	}
}
