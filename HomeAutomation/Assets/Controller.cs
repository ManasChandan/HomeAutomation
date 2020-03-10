using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Controller : MonoBehaviour,IVirtualButtonEventHandler
{

    bool select = false;
    long bulb1 = 0;
    long bulb2 = 0;
    public GameObject bulb_1, bulb_2, plane_Bulb1, plane_Down, plane_Bulb2, plane_Up, select_indicator,Indicator_Capsule;
    MeshRenderer bul1, bul2, up, plane_Bul1, down, plane_Bul2, selsct_I;
    long a = 0;
    private DatabaseReference bu1, bu2, po; 
    // Start is called before the first frame update
    void Start()
    {
        
        VirtualButtonBehaviour[] vrb = GetComponentsInChildren<VirtualButtonBehaviour>();
        for(int i=0;i<vrb.Length;i++)
        {
            vrb[i].RegisterEventHandler(this); 
        }

        bul1 = bulb_1.GetComponent<MeshRenderer>();
        bul2 = bulb_2.GetComponent<MeshRenderer>();
        up = plane_Up.GetComponent<MeshRenderer>();
        down = plane_Down.GetComponent<MeshRenderer>();
        plane_Bul1 = plane_Bulb1.GetComponent<MeshRenderer>();
        plane_Bul2 = plane_Bulb2.GetComponent<MeshRenderer>();
        selsct_I = select_indicator.GetComponent<MeshRenderer>();
        
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sensorrunner.firebaseio.com/");
        bu1 = FirebaseDatabase.DefaultInstance.GetReference("Bulb1");
        bu2 = FirebaseDatabase.DefaultInstance.GetReference("Bulb2");
        po = FirebaseDatabase.DefaultInstance.GetReference("Potentiometer");
        call_Values();
        


    }

    // Update is called once per frame
    void Update()
    {
        if(select==false)
        {
            up.enabled = false;
            down.enabled = false;
            plane_Bul1.enabled = true;
            plane_Bul2.enabled = true;
            selsct_I.material.color = Color.white; 
        }
        else if(select==true)
        {
            up.enabled = true;
            down.enabled = true;
            plane_Bul1.enabled = false;
            plane_Bul2.enabled = false;
            selsct_I.material.color = Color.red; 
        }
        if(bulb1==1)
        {
            bul1.material.color = Color.red; 
        }
        else if(bulb1==0)
        {
            bul1.material.color = Color.white;
        }
        if (bulb2 == 1)
        {
            bul2.material.color = Color.red;
        }
        else if (bulb2 == 0)
        {
            bul2.material.color = Color.white;
        }


    }

    void IVirtualButtonEventHandler.OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("Pressed");
        if(vb.VirtualButtonName=="Select" && select==false)
        {
            select = true; 
        }
        else if(vb.VirtualButtonName=="Select" && select==true)
        {
            select = false; 
        }
        else if(vb.VirtualButtonName=="Down")
        {
            Debug.Log("Down_Pressed"); 
            if(select==false && bulb1==0)
            {
                bulb1 = 1;
                bu1.SetValueAsync(bulb1); 
            }
            else if(select==false && bulb1==1)
            {
                bulb1 = 0;
                bu1.SetValueAsync(bulb1);
            }
            else if(select==true)
            {
                a = a - 1;
                if (a < 0)
                {
                    a = 0;
                    return;
                }
                Indicator_Capsule.transform.localScale = new Vector3(0.1f, a * 0.1f, 0.1f);
                po.SetValueAsync(a * 36); 
                
            }
        }
        else if (vb.VirtualButtonName == "Up")
        {
            Debug.Log("Up_PRessed");
            if (select == false && bulb2 == 0)
            {
                bulb2 = 1;
                bu2.SetValueAsync(bulb2);
            }
            else if (select == false && bulb2 == 1)
            {
                bulb2 = 0;
                bu2.SetValueAsync(bulb2); 
            }
            else if(select==true)
            {
                a = a + 1;
                if (a > 6)
                {
                    a = 6;
                    return;
                }
                Indicator_Capsule.transform.localScale = new Vector3(0.1f, a * 0.1f, 0.1f);
                po.SetValueAsync(a * 36); 

            }
        }
    }

    void IVirtualButtonEventHandler.OnButtonReleased(VirtualButtonBehaviour vb)
    {
         
    }
    void call_Values()
    {
        bu1.GetValueAsync().ContinueWith(task =>
        {
            if(task.IsFaulted)
            {
                //wait
            }
            else if(task.IsCompleted)
            {
                DataSnapshot snap = task.Result;
                bulb1 = (long)snap.Value;
            }

        });
        bu2.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //wait
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snap = task.Result;
                bulb2 = (long)snap.Value;
            }

        });
        po.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //wait
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snap = task.Result;
                a = (((long)snap.Value)/36);
                Indicator_Capsule.transform.localScale = new Vector3(0.1f, a * 0.1f, 0.1f);
            }

        });


    }
}
