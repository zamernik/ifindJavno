package com.example.ifindandroid;

import android.os.Bundle;
import android.text.Html;
import android.view.View;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import android.content.Intent;
import android.os.Bundle;
import android.text.method.ScrollingMovementMethod;
import android.util.Log;
import android.view.View;
import android.widget.TextView;
import androidx.appcompat.app.AppCompatActivity;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.Volley;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import java.util.ArrayList;

public class MainActivity extends AppCompatActivity {
    private RequestQueue requestQueue;
    private TextView osebe;
    private String url = "https://ifind-is-gcgwejbgfbgparhf.germanywestcentral-01.azurewebsites.net/api/v1/uporabniki" ;
    private String urlDogodki = "https://ifind-is-gcgwejbgfbgparhf.germanywestcentral-01.azurewebsites.net/api/v1/dogodki" ;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);
        requestQueue = Volley.newRequestQueue(getApplicationContext());
        osebe = (TextView) findViewById(R.id.osebe);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
    }
    public  void prikaziOsebe(View view){
        if (view != null){
            JsonArrayRequest request = new JsonArrayRequest(url, jsonArrayListener, errorListener);
            requestQueue.add(request);
        }
    }
    public  void prikaziDogodke(View view){
        if (view != null){
            JsonArrayRequest request = new JsonArrayRequest(urlDogodki, jsonArrayListenerD, errorListener);
            requestQueue.add(request);
        }
    }
    public void reset(View view){
        osebe.setText("");
    }

    private Response.Listener<JSONArray> jsonArrayListener = new Response.Listener<JSONArray>() {
        @Override
        public void onResponse(JSONArray response){
            ArrayList<String> data = new ArrayList<>();
            data.add("SEZNAM UPORABNINKOV Z E-MAILI:");
            //for loop ki gre skozi atribute
            for (int i = 0; i < response.length(); i++){
                try {
                    JSONObject object =response.getJSONObject(i);
                    String name = object.getString("ime");
                    String surname = object.getString("priimek");
                    String email = object.getString("email");

                    data.add(name + " " + surname + " " + ", " +  email);

                } catch (JSONException e){
                    e.printStackTrace();
                    return;

                }
            }

            osebe.setText("");


            for (String row: data){
                String currentText = osebe.getText().toString();
                osebe.setText(currentText + "\n\n" + row);
            }

        }

    };
    private Response.ErrorListener errorListener = new Response.ErrorListener() {
        @Override
        public void onErrorResponse(VolleyError error) {
            Log.d("REST error", error.getMessage());
        }
    };

    private Response.Listener<JSONArray> jsonArrayListenerD = new Response.Listener<JSONArray>() {
        @Override
        public void onResponse(JSONArray response){
            ArrayList<String> data = new ArrayList<>();
            data.add("SEZNAM DOGODKOV Z ČASOM:");
            //for loop ki gre skozi atribute
            for (int i = 0; i < response.length(); i++){
                try {
                    JSONObject object =response.getJSONObject(i);
                    String dogodek = object.getString("naziv");
                    String cas = object.getString("datumCas");
                    String deli[] = cas.split("T");
                    String datum = deli[0];
                    String ura = deli[1];

                    data.add(dogodek + ", " + datum + " ob " + ura);

                } catch (JSONException e){
                    e.printStackTrace();
                    return;

                }
            }

            osebe.setText("");


            for (String row: data){
                String currentText = osebe.getText().toString();
                osebe.setText(currentText + "\n\n" + row);
            }

        }

    };

}