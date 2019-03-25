package com.example.user.controler_test;

import android.content.Intent;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.MotionEvent;
import android.view.View;
import android.widget.ImageButton;
import android.widget.Toast;

import java.io.IOException;
import java.io.PrintWriter;
import java.net.Socket;



public class Game extends AppCompatActivity implements SensorEventListener{
    ImageButton btnGo,btnBooster,btnDrift;
    Socket clientSocket;
    PrintWriter out;
    Boolean isCheck=false;
    Boolean isStarted=false;
    String ipaddr;
    private SensorManager sensorManager;
    private Sensor senAccelerometer;;
    private KalmanFilter mKalmanAccX;
    private KalmanFilter mKalmanAccY;
    float filteredX,filteredY,filteredZ;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_game);
        Intent it2 = this.getIntent();
        ipaddr=it2.getStringExtra("it_tag");

        new Connect().start();
        btnGo = (ImageButton)findViewById(R.id.Gobtn);
        btnBooster = (ImageButton) findViewById(R.id.Booster);
        btnDrift = (ImageButton) findViewById(R.id.Drift);

        try { //Go버튼 업다운 이벤트 처리
            btnGo.setOnTouchListener(new View.OnTouchListener() {
                @Override
                public boolean onTouch(View v, MotionEvent event) {
                    sendGo sGo = new sendGo();

                    int action = event.getAction();
                    if (action == MotionEvent.ACTION_DOWN) {
                        sGo.setLoop(true);
                        sGo.start();
                    } else if (action == MotionEvent.ACTION_UP) {
                        sGo.setLoop(false);
                        sGo.start();

                    }
                    return false;
                }
            });
            //Drift버튼 업다운 이벤트처리
            btnDrift.setOnTouchListener(new View.OnTouchListener() {
                @Override
                public boolean onTouch(View v, MotionEvent event) {
                    sendDrift sMs = new sendDrift();

                    int action = event.getAction();
                    if (action == MotionEvent.ACTION_DOWN) {
                        sMs.setLoop(true);
                        sMs.start();
                    } else if (action == MotionEvent.ACTION_UP) {
                        sMs.setLoop(false);
                        sMs.start();

                    }
                    return false;
                }
            });

            mKalmanAccX = new KalmanFilter(0.0f);
            mKalmanAccY = new KalmanFilter(0.0f);

            sensorManager = (SensorManager) this.getSystemService(SENSOR_SERVICE);
            senAccelerometer = sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);
            sensorManager.registerListener(this, senAccelerometer, SensorManager.SENSOR_DELAY_NORMAL);
        }catch (Exception e){
            Toast.makeText(this,"----에러:"+e.getMessage(),Toast.LENGTH_SHORT).show();
        }
    }



    /*------------------------------------------------------------------------------------------------*/
    //연결메소드
    class Connect extends Thread {
        public void run() {
            try {
                clientSocket = new Socket(ipaddr, 9001); //소켓생성
                out = new PrintWriter(clientSocket.getOutputStream(), true);
            } catch (Exception e) {
                System.out.println("에러-------------:"+e.getMessage());
            }

        }
    }

    //Go를 보내주는 메소드(누르는동안은 계속 보내줘야하기때문에 따로)
    class sendGo extends Thread {
        public void run() {
            System.out.println("-----------ff"+clientSocket);
            try {
                if(isCheck) {
                    out.println("2 Go");
                }else{
                    out.println("2 ST");
                }
            }
            catch (Exception e) {}

        }
        public void setLoop(boolean state){
            isCheck=state;
        }
    }

    //Drift 전송 메소드 (이것도 누르는 동안 계속 보내줘야함)
    class sendDrift extends Thread {
        public void run() {
            try {
                if(isCheck) {
                    out.println("3 Drift");
                }else{
                    out.println("3 ND");
                }
            } catch (Exception e) {}
        }
        public void setLoop(boolean state){
            isCheck=state;
        }
    }

    //Boost 전송 메소드
    class sendBoost extends Thread {
        public void run(){
            try{
                out.println("4 Boost");
            }catch (Exception e) {}
        }
    }

    // 부스트 버튼 처리 메소드
    public void clickBtn(View v) {
        sendBoost sB=new sendBoost();
        sB.start();
    }


    class sendSensor extends Thread {
        float dataX=0.0f;
        //public void setStr(float str){ dataX = str; }
        public void run() {
            try {
                while(true) {
                    dataX=getSensor();
                    out.println("1 "+dataX);
                    sleep(100);
                }

            } catch (Exception e) {
            }
        }
    }
    /*-----------------------------------------------------------------------------------------*/

    public float getSensor(){
        return filteredY;
    }
    @Override
    public void onSensorChanged(SensorEvent sensorEvent) {
        Sensor mySensor = sensorEvent.sensor;
        sendSensor ss = new sendSensor();
        if(mySensor.getType() == Sensor.TYPE_ACCELEROMETER){
            float x = sensorEvent.values[0];
            float y = sensorEvent.values[1];
            float z = sensorEvent.values[2];

            //칼만필터를 적용한다
            //filteredX = (float) mKalmanAccX.update(x);
            //filteredY = (float) mKalmanAccY.update(y);
            //filteredZ = (float) mKalmanAccY.update(z);

            filteredY=y;
            if(!isStarted){
                isStarted=true;
                ss.start();
            }
        }
    }

    class KalmanFilter {

        private double Q = 0.00001;
        private double R = 0.001;
        private double X = 0, P = 1, K;

        //첫번째값을 입력받아 초기화 한다. 예전값들을 계산해서 현재값에 적용해야 하므로 반드시 하나이상의 값이 필요하므로~
        KalmanFilter(double initValue) {
            X = initValue;
        }

        //예전값들을 공식으로 계산한다
        private void measurementUpdate(){
            K = (P + Q) / (P + Q + R);
            P = R * (P + Q) / (R + P + Q);
        }

        //현재값을 받아 계산된 공식을 적용하고 반환한다
        public double update(double measurement){
            measurementUpdate();
            X = X + (measurement - X) * K;

            return X;
        }
    }


    @Override
    public void onAccuracyChanged(Sensor sensor, int i) {

    }

    @Override
    protected void onPause() { //Activity위에 다른 Activity가 올라온 경우 (알람,전화)
        try {
            if(clientSocket!=null) {
                clientSocket.close();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        super.onPause();
        sensorManager.unregisterListener(this);
    }

    @Override
    protected void onResume() { //focus를 다시 얻었을때
        super.onResume();
        sensorManager.registerListener(this, senAccelerometer, SensorManager.SENSOR_DELAY_NORMAL);
    }

    @Override
    protected void onStop() { //Activity가 완전히 화면을 벗어날때
        // TODO Auto-generated method stub
        try {
            if(clientSocket!=null) {
                clientSocket.close();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        super.onStop();
    }

    @Override
    public void onBackPressed() {
        try {
            if(clientSocket!=null) {
                clientSocket.close();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        Intent it = new Intent(this,MainActivity.class);
        this.startActivity(it);
        this.finish();
    }
}
