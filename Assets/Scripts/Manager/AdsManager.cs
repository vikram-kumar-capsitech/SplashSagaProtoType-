using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{

    public string appId = "ca-app-pub-3940256099942544~3347511713";

#if UNITY_ANDROID

        string bannerId = "ca-app-pub-3940256099942544/6300978111";
        string interId = "ca-app-pub-3940256099942544/1033173712";
        string rewardId = "ca-app-pub-3940256099942544/5224354917";
        string nativeId = "ca-app-pub-3940256099942544/2247696110";

#elif UNITY_IPHONE

        string bannerId = "ca-app-pub-3940256099942544/2934735716";
        string interId = "ca-app-pub-3940256099942544/4411468910";
        string rewardId = "ca-app-pub-3940256099942544/1712485313";
        string nativeId = "ca-app-pub-3940256099942544/3986624511";

#endif

    BannerView bannerView;
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;
    NativeAd nativeAd;

    private void Start()
    {
        MobileAds.Initialize(initstatus =>
        {
            print("Ads interstitial !!");
        });
    }

    #region Banner

    public void loadBanner()
    {
        createBannerView();
        ListenToBannerEvents();

        if (bannerView == null) 
        {
            createBannerView();
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        print("loading banner Ad !!");
        bannerView.LoadAd(adRequest);
    }

    void createBannerView()
    {
        if (bannerView != null)
        {
            destroyBanner();
        }
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);
    }

    void ListenToBannerEvents()
    {
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner View loaded an ad with reaponse : " 
                + bannerView.GetResponseInfo());
        };
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.Log("Banner View failed to load an ad with error : "
                + error);
        };
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Banner View paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner View recorded an impression.");
        };
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner View was clicked.");

        };
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened");

        };
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed");

        };

    }

    public void destroyBanner()
    {
        if(bannerView != null)
        {
            print("destroying banner ad");
            bannerView.Destroy();
            bannerView = null;
        }
    }
    #endregion

    #region interstitial

    public void loadInterstitialAd() 
    {
        if(interstitialAd != null)
        {
            Debug.Log("Work inprogress");

            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(interId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Interstitial Ad failed to load" + error);
                return;
            }
            print("Interstitial ad loaded !!" + ad.GetResponseInfo());

            interstitialAd = ad;

            interstitialEvent(interstitialAd);
        });
    }
    public void showInterstitialAd() 
    {
        if (interstitialAd != null && interstitialAd.CanShowAd()) 
        {
            interstitialAd.Show();
        }
        else
        {
            print("Interstitial ad not ready !!");
        }
    }
    public void interstitialEvent(InterstitialAd ad) 
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Interstitial ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression");
        };
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked");
        };
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen opened");
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen closed");
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("intertitial ad failed to open full screen content " + 
                "with error : " + error);
        };

    }

    #endregion

    #region Rewarded

    public void loadRewardAd()
    {
        if(rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(rewardId, adRequest,(RewardedAd ad,LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Rewarded Ad failed to load" + error);
                return;
            }
            print("Rewarded ad loaded !!" + ad.GetResponseInfo());

            rewardedAd = ad;

            rewardAdEvent(rewardedAd);
        });

        showRewardAd();
    }
    public void showRewardAd() 
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                print("Give reward to player !!");
                giveReward();
            });
        }
        else
        {
            print("Interstitial ad not ready !!");
        }
    }
    public void rewardAdEvent(RewardedAd ad)
    {
        rewardedAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Rewarded ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        rewardedAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression");
        };
        rewardedAd.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked");

        };
        rewardedAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen opened");
        };
        rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen closed");
        };
        rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                "with error : " + error);
        };

    }

    #endregion

    #region extra

    public void giveReward()
    {
        Debug.Log("Rewarded $100");
    }

    #endregion

}
