#READ ME

## 概要
GooglePlay Games Service のサンプルプロジェクト"Type-a-Number"をUnity Androidに移植したプロジェクトです。

## 注意
Unity4.1.2とAndroid SDK R22でビルドしました。
ただし、Unity4.1.2はAndroid SDK R22には対応していないのでR21をコピーしておいてからR22にアップデートするなどしたほうがよいです。

##   手順
1. [Google Play Game ServicesのGetting Started for Android](https://developers.google.com/games/services/android/quickstart)の手順をしっかりクリアします。eclipse にはサンプルのTypeANumberプロジェクトと、google-play-services_libとBaseGameUtilsのライブラリプロジェクトが出来上がっていて、GooglePlayデベロッパーコンソールにはアチーブメントやリーダーボードの設定が済んでいるはず。
2. デベロッパーコンソールで新しく リンク済みアプリを追加する。今回はcom.saikorobot.games.tancunityとした。
3. UnityのプロジェクトのMain CameraのTypeANumberコンポーネントの アチーブメントやリーダーボードのキー（Replace Meとなっているところ）を設定する。
4. PlayerSettingsでパッケージ名（Bundle Identifer）を2. でリンク済みにしたもの変更しておく。Eclipse 形式でexportしてEclipseで開く。 
5.  ライブラリプロジェクトのgoogle-play-services_libとBaseGameUtilsを参照するようにする。
6. AndroidManifest.xmlの&lt;application&gt;タグ内部に以下を追記。

        <meta-data
            android:name="com.google.android.gms.games.APP_ID"
            android:value="@string/app_id" />
7.  /res/values/idx.xml を以下の内容で作成する。Replace Meの部分はデベロッパーコンソールのAppIDに置き換える。

        <?xml version="1.0" encoding="utf-8"?> 
        <resources>   
        <string name="app_id">Replace Me</string> 
        </resources> 

8.  AndroidProject/src/com/saikorobot/gms/ の GameHelperHolder.java と Plugin.javaをプロジェクトにインポートする。
9.  XXXXNativeActivity.javaと XXXXActivity.java （XXXXProxyActivity.java以外の2つ）を TypeANumberUnityNativeActivity.javaやTypeANumberUnityActivity.javaのように変更する。以下例
        
        略
        public class TypeANumberUnityActivity extends Activity implements GameHelperHolder {
            public GameHelper mGameHelper;
            protected void onCreate(Bundle savedInstanceState) {
                super.onCreate(savedInstanceState);
                略
                playerView.requestFocus();

                mGameHelper = new GameHelper(this);
                mGameHelper.setup(Plugin.getInstance(), GameHelper.CLIENT_GAMES);
            }
            @Override
            protected void onStart() {
                super.onStart();
                mGameHelper.onStart(this);
            }
            @Override
            protected void onStop() {
                super.onStop();
                mGameHelper.onStop();
            }
            @Override
            protected void onActivityResult(int requestCode, int resultCode, Intent data) {
                super.onActivityResult(requestCode, resultCode, data);
                mGameHelper.onActivityResult(requestCode, resultCode, data);
            }
            public GameHelper getGameHelper() {
                return mGameHelper;
            }
            略
        }
        
9.  デベロッパーコンソールで認証した署名ファイルでAPKを作成して実行する。