using UnityEngine;
using System.Collections;

public class SelectPurchaseItem : MonoBehaviour {

	public void OnClick(){
        
		PurchaseManager script = GameObject.Find ("PurchaseManager").GetComponent<PurchaseManager> ();
		script.BuyProductId (name);
	}
}
