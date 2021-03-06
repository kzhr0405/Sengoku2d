//
//  MovieInterstitial6000.m
//  SampleViewRecipe
//
//  Created by Junhua Li on 2016/11/03.
//
//

#import "MovieInterstitial6000.h"
#import "ALInterstitialAd.h"
#import "MovieReward6000.h"

@interface MovieInterstitial6000()<ALAdLoadDelegate, ALAdDisplayDelegate, ALAdVideoPlaybackDelegate>
@property (nonatomic, strong)NSString* submittedPackageName;
@end

@implementation MovieInterstitial6000

/**
 *  データの設定
 */
-(void)setData:(NSDictionary *)data
{
    NSString * sdkKeyStr = [data objectForKey:@"sdk_key"];
    NSDictionary* infoDict = [[NSBundle mainBundle] infoDictionary];
    if ( ![infoDict objectForKey:@"AppLovinSdkKey"] ) {
        [infoDict setValue:sdkKeyStr forKey:@"AppLovinSdkKey"];
    }
    //申請されたパッケージ名を受け取り
    self.submittedPackageName = [data objectForKey:@"package_name"];
}

/**
 *  広告の読み込みを開始する
 */
-(void)startAd
{
    [MovieConfigure6000 configure];
    [ALInterstitialAd shared].adDisplayDelegate = self;
    [ALInterstitialAd shared].adLoadDelegate = self;
    [ALInterstitialAd shared].adVideoPlaybackDelegate = self;
}

-(BOOL)isPrepared{
    //申請済のバンドルIDと異なる場合のメッセージ
    //(バンドルIDが申請済のものと異なると、正常に広告が返却されない可能性があります)
    if(self.submittedPackageName != nil
       && ![
            [self.submittedPackageName lowercaseString]
            isEqualToString:[[[NSBundle mainBundle] bundleIdentifier] lowercaseString]
            ])
    {
        //表示を消したい場合は、こちらをコメントアウトして下さい。
        NSLog(@"[ADF][Applovin]アプリのバンドルIDが、申請されたものと異なります。");
    }
    return [ALInterstitialAd isReadyForDisplay];
}

-(void)showAd
{
    if([ALInterstitialAd isReadyForDisplay]){
        [ALInterstitialAd show];
    }
    else{
        // No interstitial ad is currently available.  Perform failover logic...
    }
}

/**
 * 対象のクラスがあるかどうか？
 */
-(BOOL)isClassReference
{
    Class clazz = NSClassFromString(@"ALSdk");
    if (clazz) {
    } else {
        NSLog(@"Not found Class: ALSdk");
        return NO;
    }
    return YES;
}

/**
 *  広告の読み込みを中止
 */
-(void)cancel
{
    [[ALInterstitialAd shared] dismiss];
}

// ------------------------------ -----------------
// ここからはApplovinのDelegateを受け取る箇所

/**
 *  広告の読み込み準備が終わった
 */
-(void)adService:(ALAdService *)adService didLoadAd:(ALAd *)ad {
    NSLog(@"didLoadAd");
    if ( self.delegate ) {
        if ([self.delegate respondsToSelector:@selector(AdsFetchCompleted:)]) {
            [self.delegate AdsFetchCompleted:self];
        }
    }
}

/**
 *  広告の読み込みに失敗
 */
-(void)adService:(ALAdService *)adService didFailToLoadAdWithError:(int)code {
    NSLog(@"didFailToLoadAdWithError code:%d", code);
}

/**
 *  広告の表示が開始された場合
 */
-(void) ad:(ALAd *) ad wasDisplayedIn: (UIView *)view {
    NSLog(@"wasDisplayedIn");
    
    if ( self.delegate ) {
        if ([self.delegate respondsToSelector:@selector(AdsDidShow:)]) {
            [self.delegate AdsDidShow:self];
        }
    }
}

/**
 *  アプリが落とされたりした場合などのバックグラウンドに回った場合の動作
 */
-(void) ad:(ALAd *) ad wasHiddenIn: (UIView *)view {
    NSLog(@"wasHiddenIn");
    
    //[ALIncentivizedInterstitialAd preloadAndNotify:nil];
    if ( self.delegate ) {
        if ([self.delegate respondsToSelector:@selector(AdsDidHide:)]) {
            [self.delegate AdsDidHide:self];
        }
    }
}

/**
 *  広告をクリックされた場合の動作
 */
-(void) ad:(ALAd *) ad wasClickedIn: (UIView *)view {
    NSLog(@"wasClickedIn");
}

/**
 *  広告（ビデオ)の表示を開始されたか
 */
-(void) videoPlaybackBeganInAd: (ALAd*) ad {
    NSLog(@"videoPlaybackBeganInAd");
    // 広告の読み
}

/**
 *  広告の終了・停止時に呼ばれる
 * パーセント、読み込み終わりの設定を表示
 */
-(void) videoPlaybackEndedInAd: (ALAd*) ad atPlaybackPercent:(NSNumber*) percentPlayed fullyWatched: (BOOL) wasFullyWatched {
    NSLog(@"videoPlaybackBeganInAd");
    
    if ( wasFullyWatched && self.delegate ) {
        if ([self.delegate respondsToSelector:@selector(AdsDidCompleteShow:)]) {
            [self.delegate AdsDidCompleteShow:self];
        }
    }
}

@end
