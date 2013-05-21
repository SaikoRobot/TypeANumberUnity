
package com.saikorobot.gms;

import android.app.Activity;
import android.util.Log;

import com.google.example.games.basegameutils.GameHelper;
import com.google.example.games.basegameutils.GameHelper.GameHelperListener;
import com.unity3d.player.UnityPlayer;

public class Plugin implements GameHelperListener {

    static final String TAG = Plugin.class.getSimpleName();
    private static final boolean DEBUG = true;

    public static final int RC_UNUSED = 5001;
    private static Plugin sInstance;
    private static String sObjectName;

    private static final int UNINITIALIZED_CB_SIGNIN_NONE = -1;
    private static final int UNINITIALIZED_CB_SIGNIN_FAILED = 1;
    private static final int UNINITIALIZED_CB_SIGNIN_SUCCEEDED = 2;
    private static int sUninitializedCallbackFunction = UNINITIALIZED_CB_SIGNIN_NONE;

    private Plugin() {
    }

    public static synchronized Plugin getInstance() {
        if (DEBUG)
            Log.d(TAG, "getInstance");
        if (sInstance == null) {
            sInstance = new Plugin();
        }
        return sInstance;
    }

    private static GameHelper getHelper(Activity activity) {
        if (!(activity instanceof GameHelperHolder)) {
            throw new RuntimeException("activity not implemented GameHelperHolder");
        }
        return ((GameHelperHolder) activity).getGameHelper();
    }

    public static void start(String objectName) {
        if (DEBUG)
            Log.d(TAG, "start objectName=" + objectName);
        sObjectName = objectName;
        if (sUninitializedCallbackFunction == UNINITIALIZED_CB_SIGNIN_FAILED) {
            sInstance.onSignInFailed();
            sUninitializedCallbackFunction = UNINITIALIZED_CB_SIGNIN_NONE;
        }
        if (sUninitializedCallbackFunction == UNINITIALIZED_CB_SIGNIN_SUCCEEDED) {
            sInstance.onSignInSucceeded();
            sUninitializedCallbackFunction = UNINITIALIZED_CB_SIGNIN_NONE;
        }
    }

    public static void signin(final Activity activity) {
        if (DEBUG)
            Log.d(TAG, "signin");
        activity.runOnUiThread(new Runnable() {

            public void run() {
                getHelper(activity).beginUserInitiatedSignIn();
            }
        });
    }

    public static void signout(final Activity activity) {
        if (DEBUG)
            Log.d(TAG, "signout");
        activity.runOnUiThread(new Runnable() {

            public void run() {
                getHelper(activity).signOut();
            }
        });
    }

    public static boolean isSignedIn(Activity activity) {
        if (DEBUG)
            Log.d(TAG, "isSignedin");
        return getHelper(activity).isSignedIn();
    }

    public static void showAchievements(final Activity activity) {
        if (DEBUG)
            Log.d(TAG, "showAchievements");
        activity.runOnUiThread(new Runnable() {

            public void run() {
                activity.startActivityForResult(getHelper(activity).getGamesClient()
                        .getAchievementsIntent(), RC_UNUSED);
            }
        });
    }

    public static void showLeaderboards(final Activity activity) {
        if (DEBUG)
            Log.d(TAG, "showLeaderboards");
        activity.runOnUiThread(new Runnable() {

            public void run() {
                activity.startActivityForResult(getHelper(activity).getGamesClient()
                        .getAllLeaderboardsIntent(), RC_UNUSED);
            }
        });
    }

    public static void unlockAchievement(final Activity activity, final String id) {
        if (DEBUG)
            Log.d(TAG, "unlockAchievement id=" + id);
        activity.runOnUiThread(new Runnable() {

            public void run() {
                getHelper(activity).getGamesClient().unlockAchievement(id);
            }
        });
    }

    public static void incrementAchievement(final Activity activity, final String id, final int num) {
        if (DEBUG)
            Log.d(TAG, "incrementAchievement id=" + id + " num=" + num);
        activity.runOnUiThread(new Runnable() {

            public void run() {
                getHelper(activity).getGamesClient().incrementAchievement(id, num);
            }
        });
    }

    public static void submitScore(final Activity activity, final String id, final int score) {
        if (DEBUG)
            Log.d(TAG, "submitScore id=" + id + " score=" + score);
        activity.runOnUiThread(new Runnable() {

            public void run() {
                getHelper(activity).getGamesClient().submitScore(id, score);
            }
        });
    }

    public static String getDisplayName(Activity activity) {
        if (DEBUG)
            Log.d(TAG, "getDisplayName");
        return getHelper(activity).getGamesClient().getCurrentPlayer().getDisplayName();
    }

    // -- GameHelperListener
    public void onSignInFailed() {
        if (DEBUG)
            Log.d(TAG, "onSignInFailed sObjectName=" + sObjectName);
        if (sObjectName != null) {
            UnityPlayer.UnitySendMessage(sObjectName, "OnSignInFailed", "");
        } else {
            sUninitializedCallbackFunction = UNINITIALIZED_CB_SIGNIN_FAILED;
        }
    }

    public void onSignInSucceeded() {
        if (DEBUG)
            Log.d(TAG, "onSignInSucceeded sObjectName=" + sObjectName);
        if (sObjectName != null) {
            UnityPlayer.UnitySendMessage(sObjectName, "OnSignInSucceeded", "");
        } else {
            sUninitializedCallbackFunction = UNINITIALIZED_CB_SIGNIN_SUCCEEDED;
        }
    }
    // -- GameHelperListener

}
