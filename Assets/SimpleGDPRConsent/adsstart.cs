using Newtonsoft.Json;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace BsplabsMobileAds
{
    public class Data
    {
        public string BSPLABS_ADMOB_INTERSTITIAL { get; set; }
        public string BSPLABS_ADMOB_REWARDED { get; set; }
        public string BSPLABS_ADMOB_BANNER { get; set; }
        public string BSPLABS_APPLOVIN_INTERSTITIAL { get; set; }
        public string BSPLABS_APPLOVIN_REWARDED { get; set; }
        public string BSPLABS_APPLOVIN_BANNER { get; set; }
        public string BSPLABS_UNITYADS_GAMEID { get; set; }
        public string BSPLABS_UNITYADS_INTERSTITIAL { get; set; }
        public string BSPLABS_UNITYADS_REWARDED { get; set; }
        public string BSPLABS_UNITYADS_BANNER { get; set; }
        public string BSPLABS_IRONSOURCE_APPKEY { get; set; }
        public string BSPLABS_IRONSOURCE_INTERSTITIAL { get; set; }
        public string BSPLABS_IRONSOURCE_REWARDED { get; set; }
        public string BSPLABS_IRONSOURCE_BANNER { get; set; }


    }

    public class adsstart : MonoBehaviour
    {
        // Start is called before the first frame update
        public string Url_AdServer_Json;

        private const string ADS_PERSONALIZATION_CONSENT = "Ads";
        public bool Show_TOS_Type;
        public bool Show_for_All;
        public bool Show_TOS_Link;
        public string TOS_Link;
        public bool Show_Privacy_Link;
        public string Privacy_Link;





        void Start()
        {
            //if url_adserver_json is set and not empty, then load the json from the url
            if (!string.IsNullOrEmpty(Url_AdServer_Json))
            {
                StartCoroutine(LoadJsonFromUrl(Url_AdServer_Json));
            }

            ///PlayerPrefs.DeleteAll();
            Debug.Log("Terms of Service has been accepted: " + SimpleGDPR.IsTermsOfServiceAccepted);
            Debug.Log("Ads personalization consent state: " + SimpleGDPR.GetConsentState(ADS_PERSONALIZATION_CONSENT));
            Debug.Log("Is user possibly located in the EEA: " + SimpleGDPR.IsGDPRApplicable);

            if (BspLabsAdsMobile.Instance.UserConsentWasSet() == false)

            {
                if (Show_TOS_Type == true)
                {
                    if (SimpleGDPR.IsTermsOfServiceAccepted == false)
                    {
                        if (Show_for_All == true)
                        {
                            ShowTOS();
                        }
                        else
                        {
                            if (SimpleGDPR.IsGDPRApplicable == true)
                            {
                                ShowTOS();
                            }
                        }
                    }
                    else
                    {
                        if (SimpleGDPR.GetConsentState(ADS_PERSONALIZATION_CONSENT) == SimpleGDPR.ConsentState.No)
                        {
                            BspLabsAdsMobile.Instance.SetUserConsent(false);
                            BspLabsAdsMobile.Instance.SetCCPAConsent(false);
                        }
                        else
                        {
                            BspLabsAdsMobile.Instance.SetUserConsent(true);
                            BspLabsAdsMobile.Instance.SetCCPAConsent(true);
                        }
                    }

                }
                else
                {
                    if (SimpleGDPR.GetConsentState(ADS_PERSONALIZATION_CONSENT) == SimpleGDPR.ConsentState.Unknown)
                    {
                        if (Show_for_All == true)
                        {
                            StartCoroutine(ShowGDPRConsentDialogAndWait());
                        }
                        else
                        {
                            if (SimpleGDPR.IsGDPRApplicable == true)
                            {
                                StartCoroutine(ShowGDPRConsentDialogAndWait());
                            }
                        }
                    }
                    else
                    {
                        if (SimpleGDPR.GetConsentState(ADS_PERSONALIZATION_CONSENT) == SimpleGDPR.ConsentState.Yes)
                        {
                            BspLabsAdsMobile.Instance.SetUserConsent(true);
                            BspLabsAdsMobile.Instance.SetCCPAConsent(true);
                        }
                        else
                        {
                            BspLabsAdsMobile.Instance.SetUserConsent(false);
                            BspLabsAdsMobile.Instance.SetCCPAConsent(false);
                        }
                    }
                }
            }
            BspLabsAdsMobile.Instance.Initialize();

        }


        private void Rejson(string jsondata)
        {
            Data data = JsonConvert.DeserializeObject<Data>(jsondata);
            PlayerPrefs.SetString("BSPLABS_ADMOB_INTERSTITIAL", data.BSPLABS_ADMOB_INTERSTITIAL);
            PlayerPrefs.SetString("BSPLABS_ADMOB_REWARDED", data.BSPLABS_ADMOB_REWARDED);
            PlayerPrefs.SetString("BSPLABS_ADMOB_BANNER", data.BSPLABS_ADMOB_BANNER);
            PlayerPrefs.SetString("BSPLABS_APPLOVIN_INTERSTITIAL", data.BSPLABS_APPLOVIN_INTERSTITIAL);
            PlayerPrefs.SetString("BSPLABS_APPLOVIN_REWARDED", data.BSPLABS_APPLOVIN_REWARDED);
            PlayerPrefs.SetString("BSPLABS_APPLOVIN_BANNER", data.BSPLABS_APPLOVIN_BANNER);
            PlayerPrefs.SetString("BSPLABS_UNITYADS_GAMEID", data.BSPLABS_UNITYADS_GAMEID);
            PlayerPrefs.SetString("BSPLABS_UNITYADS_INTERSTITIAL", data.BSPLABS_UNITYADS_INTERSTITIAL);
            PlayerPrefs.SetString("BSPLABS_UNITYADS_REWARDED", data.BSPLABS_UNITYADS_REWARDED);
            PlayerPrefs.SetString("BSPLABS_UNITYADS_BANNER", data.BSPLABS_UNITYADS_BANNER);
            PlayerPrefs.SetString("BSPLABS_IRONSOURCE_APPKEY", data.BSPLABS_IRONSOURCE_APPKEY);
            PlayerPrefs.SetString("BSPLABS_IRONSOURCE_INTERSTITIAL", data.BSPLABS_IRONSOURCE_INTERSTITIAL);
            PlayerPrefs.SetString("BSPLABS_IRONSOURCE_REWARDED", data.BSPLABS_IRONSOURCE_REWARDED);
            PlayerPrefs.SetString("BSPLABS_IRONSOURCE_BANNER", data.BSPLABS_IRONSOURCE_BANNER);

        }

        //load json from url using unitywebrequest
        private IEnumerator LoadJsonFromUrl(string url)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                request.timeout = 30;
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error while loading json from url: {request.error}");
                    yield break;
                }
                else
                {
                    //read json and deserialize it using newtonsoft json
                    string json = request.downloadHandler.text;
                    Rejson(json);
                    //debug log json
                    Debug.Log("json ads" + json);

                }
            }
        }


        private void TermsOfServiceDialogClosed()
        {
            // We can assume that user has accepted the terms because
            // TermsOfServiceDialog dialog can only be closed via the 'Accept' button
            Debug.Log("Accepted Terms of Service & set consent to true");
            BspLabsAdsMobile.Instance.SetUserConsent(true);
            BspLabsAdsMobile.Instance.SetCCPAConsent(true);
        }

        private IEnumerator ShowGDPRConsentDialogAndWait()
        {
            // Show a consent dialog with two sections (and wait for the dialog to be closed):
            // - Ads Personalization: its value can be changed directly from the UI,
            //   result is stored in the ADS_PERSONALIZATION_CONSENT identifier
            // - Unity Analytics: its value can't be changed from the UI since Unity presents its own UI
            //   to toggle Analytics consent. Instead, a button is shown and when the button is clicked,
            //   UnityAnalyticsButtonClicked function is called to present Unity's own UI
            yield return SimpleGDPR.WaitForDialog(new GDPRConsentDialog().
                AddSectionWithToggle(ADS_PERSONALIZATION_CONSENT, "Ads Personalization and Data Analitic", "When enabled, you'll see ads that are more relevant to you. Otherwise, you will still receive ads, but they will no longer be tailored toward you.").
                AddPrivacyPolicies(
                Privacy_Link,
                TOS_Link,
                "https://policies.google.com/privacy",
                "https://unity3d.com/legal/privacy-policy"
                ));

            if (SimpleGDPR.GetConsentState(ADS_PERSONALIZATION_CONSENT) == SimpleGDPR.ConsentState.Yes)
            {
                BspLabsAdsMobile.Instance.SetUserConsent(true);
                BspLabsAdsMobile.Instance.SetCCPAConsent(true);
            }
            else
            {
                BspLabsAdsMobile.Instance.SetUserConsent(false);
                BspLabsAdsMobile.Instance.SetCCPAConsent(false);
            }
            // Check if user has granted the Ads Personalization permission

        }

        private void ShowTOS()
        {
            if (Show_TOS_Link == true && Show_Privacy_Link == false)
            {
                SimpleGDPR.ShowDialog(new TermsOfServiceDialog().
                SetTermsOfServiceLink(TOS_Link), TermsOfServiceDialogClosed);
            }

            if (Show_TOS_Link == false && Show_Privacy_Link == true)
            {
                SimpleGDPR.ShowDialog(new TermsOfServiceDialog().SetPrivacyPolicyLink(Privacy_Link),
                TermsOfServiceDialogClosed);
            }
            if (Show_TOS_Link == true && Show_Privacy_Link == true)
            {
                SimpleGDPR.ShowDialog(new TermsOfServiceDialog().
               SetTermsOfServiceLink(TOS_Link).
               SetPrivacyPolicyLink(Privacy_Link),
               TermsOfServiceDialogClosed);
            }
        }
        public void OpenAdsConsent()
        {
            StartCoroutine(ShowGDPRConsentDialogAndWait());
        }

    }
}
