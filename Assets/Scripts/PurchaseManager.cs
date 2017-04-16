using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class PurchaseManager : MonoBehaviour, IStoreListener {

    // Unity IAP objects 
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Product Id
    private static string Id100Consumable = "busyodama100";
	private static string Id1000Consumable = "busyodama1000";
	private static string Id5000Consumable = "busyodama5000";
    private static string Id12000Consumable = "busyodama12000";
    private static string IdAddJinkei1NonConsumable = "addjinkei1";
	private static string IdAddJinkei2NonConsumable = "addjinkei2";
	private static string IdAddJinkei3NonConsumable = "addjinkei3";
	private static string IdAddJinkei4NonConsumable = "addjinkei4";

    private int m_SelectedItemIndex = -1; // -1 == no product
	private bool m_PurchaseInProgress;

	public AudioSource[] audioSources;
    public GameObject canvasObj = null;

    void Awake() {

        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        canvasObj = GameObject.Find("Canvas").gameObject;

        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null) {
            InitializePurchasing();
        }

        initUI();
    }
    

    public void InitializePurchasing(){

        if (IsInitialized()) {
            // ... we are done here.
            return;
        }

        //var module = StandardPurchasingModule.Instance();
        //module.useMockBillingSystem = true; // Microsoft
        // The FakeStore supports: no-ui (always succeeding), basic ui (purchase pass/fail), and 
        // developer ui (initialization, purchase, failure code setting). These correspond to 
        // the FakeStoreUIMode Enum values passed into StandardPurchasingModule.useFakeStoreUIMode.
        //module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

        //var builder = ConfigurationBuilder.Instance(module);
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        #if UNITY_ANDROID
                builder.Configure<IGooglePlayConfiguration>().SetPublicKey("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAymGVVCcxKpIsmeRbN+HjbxXDayMe3CzfIUils9Lwe2cku3FeFAKkQHFnUUUIbKrSAx5W5incPJchYJk0m/kiCkFOS1owDXIzr2UwR8CjznZpIoBz/YhRe+ODlxSSrYXwAwK5eCxObIgXDTvQ4aH31hJbGEuXPjrx18NS/ucc74PU2UBc796uTtrVEPzyVJCsxEJAe+pfgsXIkikKBsdDGwahonB+6UghCGJh/Y7vh4IAxfNr685kNfZ/nUqJAzQ8Uk4INv0ZnedQBVeEkHXyEVqYUonHorRvzka5X4pDXCXT8dyl5hGuR/WoOMAjMm3pVmGKYCaxrOodT8JOktdOEwIDAQAB");
        #endif


        builder.AddProduct (Id100Consumable, ProductType.Consumable, new IDs{
            {"jp.zeimoter.sengoku2d.busyodama100", AppleAppStore.Name,GooglePlay.Name}
        });
		builder.AddProduct (Id1000Consumable, ProductType.Consumable, new IDs {
            {"jp.zeimoter.sengoku2d.busyodama1000", AppleAppStore.Name,GooglePlay.Name}
        });
		builder.AddProduct (Id5000Consumable, ProductType.Consumable, new IDs{
            {"jp.zeimoter.sengoku2d.busyodama5000", AppleAppStore.Name,GooglePlay.Name}
        });
        builder.AddProduct(Id12000Consumable, ProductType.Consumable, new IDs{
            {"jp.zeimoter.sengoku2d.busyodama12000", AppleAppStore.Name,GooglePlay.Name}
        });
        builder.AddProduct (IdAddJinkei1NonConsumable, ProductType.NonConsumable, new IDs{
            {"jp.zeimoter.sengoku2d.addjinkei1", AppleAppStore.Name,GooglePlay.Name}
        });

		builder.AddProduct (IdAddJinkei2NonConsumable, ProductType.NonConsumable, new IDs{
            {"jp.zeimoter.sengoku2d.addjinkei2", AppleAppStore.Name,GooglePlay.Name}
        });
		builder.AddProduct (IdAddJinkei3NonConsumable, ProductType.NonConsumable, new IDs{
            {"jp.zeimoter.sengoku2d.addjinkei3", AppleAppStore.Name,GooglePlay.Name}
        });

		builder.AddProduct (IdAddJinkei4NonConsumable, ProductType.NonConsumable, new IDs{
            {"jp.zeimoter.sengoku2d.addjinkei4", AppleAppStore.Name,GooglePlay.Name}
        });

		UnityPurchasing.Initialize(this, builder);
	}


    private bool IsInitialized() {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void BuyProductId(string productId){

        //reset        
        if (IsInitialized()) { 
            Product product = m_StoreController.products.WithID(productId);

			if(product !=null && product.availableToPurchase){
				audioSources [0].Play ();
                m_StoreController.InitiatePurchase(product);

			}else{
				Debug.Log("Error");
			}
		}else {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
	}


	public void RestorePurchases(){

        if (!IsInitialized()) {
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
			Application.platform == RuntimePlatform.OSXPlayer){


            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");
			audioSources [3].Play ();

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
				// The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
	}



	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e){
		Debug.Log("Purchase OK: " + e.purchasedProduct.definition.id);
		Debug.Log("Receipt: " + e.purchasedProduct.receipt);
        Message msg = new Message();
        m_PurchaseInProgress = false;

		//Data Change Process
		if (e.purchasedProduct.definition.id == "busyodama100"){
			int nowBusyoDama = PlayerPrefs.GetInt ("busyoDama");
			int newBusyoDama = nowBusyoDama + 100;
			PlayerPrefs.SetInt ("busyoDama", newBusyoDama);
            

        } else if (e.purchasedProduct.definition.id =="busyodama1000") {
			int nowBusyoDama = PlayerPrefs.GetInt ("busyoDama");
			int newBusyoDama = nowBusyoDama + 1000;
			PlayerPrefs.SetInt ("busyoDama", newBusyoDama);

		} else if (e.purchasedProduct.definition.id =="busyodama5000") {
			int nowBusyoDama = PlayerPrefs.GetInt ("busyoDama");
			int newBusyoDama = nowBusyoDama + 5000;
			PlayerPrefs.SetInt ("busyoDama", newBusyoDama);

		} else if (e.purchasedProduct.definition.id == "busyodama12000") {
            int nowBusyoDama = PlayerPrefs.GetInt("busyoDama");
            int newBusyoDama = nowBusyoDama + 12000;
            PlayerPrefs.SetInt("busyoDama", newBusyoDama);

        }else if (e.purchasedProduct.definition.id == "addjinkei1") {
			PlayerPrefs.SetBool ("addJinkei1", true);			
		
		}else if (e.purchasedProduct.definition.id == "addjinkei2") {
			PlayerPrefs.SetBool ("addJinkei2", true);			
		
		}else if (e.purchasedProduct.definition.id == "addjinkei3") {
			PlayerPrefs.SetBool ("addJinkei3", true);			
		
		}else if (e.purchasedProduct.definition.id == "addjinkei4") {
			PlayerPrefs.SetBool ("addJinkei4", true);			
		}
        
        audioSources [3].Play ();

        string successMsg = "";
        if (e.purchasedProduct.definition.id.Contains("busyodama")) {
            successMsg = msg.getMessage(102);
        }else {
            successMsg = msg.getMessage(103);
        }

        canvasObj = GameObject.Find("Canvas").gameObject;
        msg.makeMessageOnGameObject(successMsg, canvasObj);
        initUI();

        // Indicate we have handled this purchase, we will not be informed of it again.x
        return PurchaseProcessingResult.Complete;
        
	}

	void initUI (){

		GameObject content = GameObject.Find ("ScrollView").transform.FindChild ("Content").gameObject;
		string path = "Prefabs/Purchase/Purchased";

		if (PlayerPrefs.HasKey ("addJinkei1")) {
			GameObject icon = Instantiate (Resources.Load (path)) as GameObject;
			GameObject btn = content.transform.FindChild ("addjinkei1").gameObject;
			icon.transform.SetParent(btn.transform);
			icon.transform.localScale = new Vector2 (1, 1);
			icon.transform.localPosition = new Vector3(0, 0, 0);
			btn.GetComponent<Button> ().enabled = false;

		}
		if (PlayerPrefs.HasKey ("addJinkei2")) {
			GameObject icon = Instantiate (Resources.Load (path)) as GameObject;
			GameObject btn = content.transform.FindChild ("addjinkei2").gameObject;
			icon.transform.SetParent(btn.transform);
			icon.transform.localScale = new Vector2 (1, 1);
			icon.transform.localPosition = new Vector3(0, 0, 0);
			btn.GetComponent<Button> ().enabled = false;

		}
		if (PlayerPrefs.HasKey ("addJinkei3")) {
			GameObject icon = Instantiate (Resources.Load (path)) as GameObject;
			GameObject btn = content.transform.FindChild ("addjinkei3").gameObject;
			icon.transform.SetParent(btn.transform);
			icon.transform.SetParent(btn.transform);
			icon.transform.localScale = new Vector2 (1, 1);
			icon.transform.localPosition = new Vector3(0, 0, 0);
			btn.GetComponent<Button> ().enabled = false;

		}
		if (PlayerPrefs.HasKey ("addJinkei4")) {
			GameObject icon = Instantiate (Resources.Load (path)) as GameObject;
			GameObject btn = content.transform.FindChild ("addjinkei4").gameObject;
			icon.transform.SetParent(btn.transform);
			icon.transform.localScale = new Vector2 (1, 1);
			icon.transform.localPosition = new Vector3(0, 0, 0);
			btn.GetComponent<Button> ().enabled = false;

		}

		if (Application.platform != RuntimePlatform.IPhonePlayer &&
		    Application.platform != RuntimePlatform.OSXPlayer &&
            Application.platform != RuntimePlatform.OSXEditor) {
			GameObject btn = content.transform.FindChild ("restore").gameObject;
			btn.SetActive (false);
		}else {
            GameObject btn = content.transform.FindChild("restore").gameObject;
            btn.SetActive(true);
        }

	}


	/*Fail Start*/
	public void OnInitializeFailed(InitializationFailureReason error){

        string errorMsg = "";
        Message msg = new Message();
        switch (error){
		case InitializationFailureReason.AppNotKnown:
            errorMsg = msg.getMessage(104);
            audioSources[4].Play();
            break;
		
		case InitializationFailureReason.PurchasingUnavailable:
            errorMsg = msg.getMessage(105);
            audioSources[4].Play();
            break;

		case InitializationFailureReason.NoProductsAvailable:
            errorMsg = msg.getMessage(106);
            audioSources[4].Play();
            break;

		}
        m_PurchaseInProgress = false;

        //Msg
        
        msg.makeMessageOnGameObject(errorMsg, canvasObj);


        PlayerPrefs.Flush();
        
    }

	public void OnPurchaseFailed(Product item, PurchaseFailureReason r){

        string errorMsg = "";
        Message msg = new Message();

        switch (r) {
		case PurchaseFailureReason.PurchasingUnavailable:
            errorMsg = msg.getMessage(107);
                audioSources[4].Play();
            break;
		case PurchaseFailureReason.ExistingPurchasePending:
            errorMsg = msg.getMessage(108);
                audioSources[4].Play();
            break;
        case PurchaseFailureReason.PaymentDeclined:
            errorMsg = msg.getMessage(109);
                audioSources[4].Play();
            break;
        case PurchaseFailureReason.ProductUnavailable:
            errorMsg = msg.getMessage(110);
                audioSources[4].Play();
            break;
        case PurchaseFailureReason.SignatureInvalid:
            errorMsg = msg.getMessage(111);
                audioSources[4].Play();
            break;
		case PurchaseFailureReason.UserCancelled:
            errorMsg = msg.getMessage(112);
                audioSources[4].Play();
            break;
		case PurchaseFailureReason.Unknown:
            errorMsg = msg.getMessage(113);
                audioSources[4].Play();
            break;		
		}       
        m_PurchaseInProgress = false;

        //reset
        //Msg
        
        msg.makeMessageOnGameObject(errorMsg, canvasObj);

    }
	/*Fail End*/

}
