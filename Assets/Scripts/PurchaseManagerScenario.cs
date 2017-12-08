using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class PurchaseManagerScenario : MonoBehaviour, IStoreListener {

    // Unity IAP objects 
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Product Id
    private static string IdScenario1NonConsumable = "scenario1";
    private static string IdScenario2NonConsumable = "scenario2";
    private static string IdScenario3NonConsumable = "scenario3";

    private int m_SelectedItemIndex = -1; // -1 == no product
    private bool m_PurchaseInProgress;

    public AudioSource[] audioSources;
    public GameObject panel = null;

    void Awake() {

        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        panel = GameObject.Find("Panel").gameObject;

        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null) {
            InitializePurchasing();
        }

        initUI();

    }

    public void BuyProductId(string productId) {
        //reset        
        if (IsInitialized()) {
            Product product = m_StoreController.products.WithID(productId);
            Debug.Log(name);

            if (product != null && product.availableToPurchase) {
                audioSources[0].Play();
                m_StoreController.InitiatePurchase(product);
            }else {
                Debug.Log("Error");
            }
        }
        else {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    void initUI() {
        
        if(PlayerPrefs.GetBool("scenario1")) {
            if(GameObject.Find("scenario1")) {
                Destroy(GameObject.Find("scenario1"));
            }
        }
        if(PlayerPrefs.GetBool("scenario2")) {
            if (GameObject.Find("scenario2")) {
                Destroy(GameObject.Find("scenario2"));
            }
        }
        if (PlayerPrefs.GetBool("scenario3")) {
            if (GameObject.Find("scenario3")) {
                Destroy(GameObject.Find("scenario3"));
            }
        }
    }

    public void InitializePurchasing() {

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
        
        builder.AddProduct(IdScenario1NonConsumable, ProductType.NonConsumable, new IDs{
            {"jp.zeimoter.sengoku2d.scenario1", AppleAppStore.Name,GooglePlay.Name}
        });

        builder.AddProduct(IdScenario2NonConsumable, ProductType.NonConsumable, new IDs{
            {"jp.zeimoter.sengoku2d.scenario2", AppleAppStore.Name,GooglePlay.Name}
        });
        builder.AddProduct(IdScenario3NonConsumable, ProductType.NonConsumable, new IDs{
            {"jp.zeimoter.sengoku2d.scenario3", AppleAppStore.Name,GooglePlay.Name}
        });

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized() {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void RestorePurchases() {

        if (!IsInitialized()) {
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer) {


            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");
            audioSources[3].Play();

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
    }



    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e) {
        Debug.Log("Purchase OK: " + e.purchasedProduct.definition.id);
        Debug.Log("Receipt: " + e.purchasedProduct.receipt);
        Message msg = new Message();
        m_PurchaseInProgress = false;

        //Data Change Process
        if (e.purchasedProduct.definition.id == "scenario1") {
            PlayerPrefs.SetBool("scenario1", true);

        }else if (e.purchasedProduct.definition.id == "scenario2") {
            PlayerPrefs.SetBool("scenario2", true);

        }else if (e.purchasedProduct.definition.id == "scenario3") {
            PlayerPrefs.SetBool("scenario3", true);
        }
        PlayerPrefs.Flush();
        audioSources[3].Play();

        msg.makeMessageOnGameObject(msg.getMessage(164), panel);
        initUI();

        // Indicate we have handled this purchase, we will not be informed of it again.x
        return PurchaseProcessingResult.Complete;

    }



    /*Fail Start*/
    public void OnInitializeFailed(InitializationFailureReason error) {

        string errorMsg = "";
        Message msg = new Message();
        switch (error) {
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
        msg.makeMessageOnGameObject(errorMsg, panel);


        PlayerPrefs.Flush();

    }

    public void OnPurchaseFailed(Product item, PurchaseFailureReason r) {

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

        msg.makeMessageOnGameObject(errorMsg, panel);

    }
}
