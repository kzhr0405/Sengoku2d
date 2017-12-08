using UnityEngine;
using System.Collections;

public class SelectPurchaseItem : MonoBehaviour {

	public void OnClick(){
        
        if(Application.loadedLevelName == "clearOrGameOver") {
            PurchaseManagerScenario script = GameObject.Find("PurchaseManager").GetComponent<PurchaseManagerScenario>();
            script.BuyProductId(name);
        }else {
            PurchaseManager script = GameObject.Find ("PurchaseManager").GetComponent<PurchaseManager> ();
		    script.BuyProductId (name);
        }
    }
}
