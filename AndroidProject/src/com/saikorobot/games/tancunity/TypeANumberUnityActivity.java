
package com.saikorobot.games.tancunity;

import com.google.example.games.basegameutils.GameHelper;
import com.saikorobot.gms.GameHelperHolder;
import com.saikorobot.gms.Plugin;
import com.unity3d.player.*;
import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;

public class TypeANumberUnityActivity extends Activity implements GameHelperHolder
{
    public GameHelper mGameHelper;
    private UnityPlayer mUnityPlayer;

    // UnityPlayer.init() should be called before attaching the view to a
    // layout.
    // UnityPlayer.quit() should be the last thing called; it will terminate the
    // process and not return.
    protected void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);

        requestWindowFeature(Window.FEATURE_NO_TITLE);

        mUnityPlayer = new UnityPlayer(this);

        if (mUnityPlayer.getSettings().getBoolean("hide_status_bar", true))
            getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,
                    WindowManager.LayoutParams.FLAG_FULLSCREEN);

        int glesMode = mUnityPlayer.getSettings().getInt("gles_mode", 1);
        boolean trueColor8888 = false;
        mUnityPlayer.init(glesMode, trueColor8888);

        View playerView = mUnityPlayer.getView();
        setContentView(playerView);
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

    protected void onDestroy()
    {
        super.onDestroy();
        mUnityPlayer.quit();
    }

    // onPause()/onResume() must be sent to UnityPlayer to enable pause and
    // resource recreation on resume.
    protected void onPause()
    {
        super.onPause();
        mUnityPlayer.pause();
        if (isFinishing())
            mUnityPlayer.quit();
    }

    protected void onResume()
    {
        super.onResume();
        mUnityPlayer.resume();
    }

    public void onConfigurationChanged(Configuration newConfig)
    {
        super.onConfigurationChanged(newConfig);
        mUnityPlayer.configurationChanged(newConfig);
    }

    public void onWindowFocusChanged(boolean hasFocus)
    {
        super.onWindowFocusChanged(hasFocus);
        mUnityPlayer.windowFocusChanged(hasFocus);
    }

    // Pass any keys not handled by (unfocused) views straight to UnityPlayer
    public boolean onKeyMultiple(int keyCode, int count, KeyEvent event)
    {
        return mUnityPlayer.onKeyMultiple(keyCode, count, event);
    }

    public boolean onKeyDown(int keyCode, KeyEvent event)
    {
        return mUnityPlayer.onKeyDown(keyCode, event);
    }

    public boolean onKeyUp(int keyCode, KeyEvent event)
    {
        return mUnityPlayer.onKeyUp(keyCode, event);
    }

    public GameHelper getGameHelper() {
        return mGameHelper;
    }
}
