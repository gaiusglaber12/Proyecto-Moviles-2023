package com.example.unityplugin;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.util.Log;

public class PopupController {

    public static final PopupController _instance = new PopupController();

    public static PopupController GetInstance(){
        return _instance;
    }

    public static Activity mainActivity;

    public static final String LOGTAG = "Hello world";

    public interface AlertViewCallBack {
        public void OnButtonTapped(int id);
    }

    public void ShowAlertView(String[] strings, final AlertViewCallBack callBack)
    {
        if (strings.length < 3)
        {
            Log.i(LOGTAG,"Error - expected at least 3 strings, got" + strings.length);
        }
        DialogInterface.OnClickListener MyClickListener = new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int id) {
                dialog.dismiss();
                Log.i(LOGTAG, "Tapped: " + id);
                callBack.OnButtonTapped(id);
            }
        };

        AlertDialog alertDialog = new AlertDialog.Builder(mainActivity)
                .setTitle(strings[0])
                .setMessage(strings[1])
                .setCancelable(false)
                .create();

        alertDialog.setButton(AlertDialog.BUTTON_NEUTRAL, strings[2], MyClickListener);
        if (strings.length > 3)
            alertDialog.setButton(AlertDialog.BUTTON_NEGATIVE, strings[3], MyClickListener);
        if (strings.length > 4)
            alertDialog.setButton(AlertDialog.BUTTON_POSITIVE, strings[4], MyClickListener);
        alertDialog.show();
    }
}