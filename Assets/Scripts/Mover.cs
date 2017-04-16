using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
	protected Vector3 Position
	{
		set { this.transform.position = value; }
		get { return this.transform.position; }
	}
	
	protected float PositionX
	{
		set { Position = new Vector3(value, Position.y, Position.z); }
		get { return Position.x; }
	}
	
	protected float PositionY
	{
		set { Position = new Vector3(Position.x, value, Position.z); }
		get { return Position.y; }
	}
	
	protected float PositionZ
	{
		set { Position = new Vector3(Position.x, Position.y, value); }
		get { return Position.z; }
	}
}