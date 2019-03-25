package com.example.user.controler_test;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

public class MainActivity extends AppCompatActivity {
    EditText ipaddr;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        ipaddr = (EditText)findViewById(R.id.ipaddr);
    }
    public void connectGame(View v){

        try {
            Intent it = new Intent(this, Game.class);
            it.putExtra("it_tag", ipaddr.getText().toString());
            this.startActivity(it);
            this.finish();
        }catch (Exception e){
            Toast.makeText(this,"err:"+e.getMessage(),Toast.LENGTH_SHORT).show();
        }
    }
}
