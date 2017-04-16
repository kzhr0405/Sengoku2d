using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollAuto : MonoBehaviour
{
	public ScrollRect myScrollRect;
	
	public void Start () {

		//Change the current vertical scroll position.
		myScrollRect.verticalNormalizedPosition = 0.5f;
	}
}
