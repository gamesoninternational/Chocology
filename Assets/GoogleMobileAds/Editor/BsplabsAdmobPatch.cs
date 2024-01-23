namespace GoogleMobileAds.Editor
{
    public class BsplabsAdmobPatch
    {
        public static void SetAdmobAppID(string androidAppId, string iosAppID)
        {
#if USE_ADMOB_BSPLABS
            GoogleMobileAdsSettings instance = GoogleMobileAdsSettings.LoadInstance();
            instance.DelayAppMeasurementInit = true;
            instance.GoogleMobileAdsAndroidAppId = androidAppId;
            instance.GoogleMobileAdsIOSAppId = iosAppID;
            UnityEditor.EditorUtility.SetDirty(instance);
#endif
        }
    }
}