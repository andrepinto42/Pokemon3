using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Threading;
using System;

public class TextDialogManager : MonoBehaviour
{
    TMP_Text textoDialogo;
    [SerializeField] GameObject completedMessageImage;
    public int delayPerCharacter = 50;
    SemaphoreSlim mySemaphoreSlim = new SemaphoreSlim(1);

    public static TextDialogManager Singleton; 

    void Awake(){
        textoDialogo = GetComponentInChildren<TMP_Text>();
        if (completedMessageImage == null)
            completedMessageImage = GetComponentInChildren<Image>().gameObject;
    }
    void Start()
    {   
        if (Singleton == null)
            Singleton = this;   
    }

    //This method must return a Task so other classes can await for it to continue their execution
    public async Task PushText(Skill skill, MonManager ally, MonManager enemy)
    {
        string text = ally.MonMain.GetNameMon() + " used " + skill.nameSkill + " !";
        await Push(text);
    }

    public async Task PushText(SkillHandlerStatusBuff.DataHandlerStatusBuff data)
    {
        Debug.Log("HERE GOES " + data.message);
        await Push(data.message,500,data.dataColor);
    }

    public  async Task PushText(string text)
    {
        await Push(text);
    }
    public  async Task PushText(string text,int extraDelay)
    {
        await Push(text,extraDelay);
    }

    public  async Task PushTextAwaitKey(string text)
    {
        await Push(text,0);
        var tokenCancellation = new CancellationTokenSource();
        Task flash = FlashCompletedMessage(tokenCancellation);
        Application.quitting += tokenCancellation.Cancel;
        //Enquanto o utilizador não carregar no espaco esta task não vai retornar
        while(! Input.GetKeyDown(KeyCode.Space))
        {
            await Task.Yield();
        }
        Application.quitting -= tokenCancellation.Cancel;
        tokenCancellation.Cancel();
    }
    private async Task Push(string text,int delay=0,SkillHandlerStatusBuff.DataMessageWithColor data1 = null)
    {
        //It its required to lock this, otherwise the text can be easily overwritten
        await mySemaphoreSlim.WaitAsync();
        try
        {
            completedMessageImage.SetActive(false);
            
            if (data1 != null)
            {
               await SendTextWithColor(text,data1);
            }
            else
            {    
                for (int i = 0; i < text.Length+1; i++)
                {
                    textoDialogo.SetText(text.Substring(0,i));
                    await Task.Delay(delayPerCharacter);             
                }
            }
          
            completedMessageImage.SetActive(true);
            
            if (delay != 0)
                await Task.Delay(delay);
        } 
        finally 
        {
            mySemaphoreSlim.Release();
        }
    }

    private async Task SendTextWithColor(string text,SkillHandlerStatusBuff.DataMessageWithColor data1)
    {
        string colorName = data1.color;
        int startIndex = data1.indexStart;
        int endIndex = data1.indexEnd;

        
        for (int i = 0; i < text.Length+1; i++)
        {
            string output ="";
            
            if (i < startIndex)
            {
                output = "<color=\"black\">";
                output += text.Substring(0,i);
            }                    
            else if (i >= startIndex && i<= endIndex)
            {
                output = text.Substring(0,startIndex);
                output += "<color=\""+colorName +"\">" +text.Substring(startIndex,i-startIndex);  
            }
            else
            {
                output = text.Substring(0,startIndex);
                output += "<color=\""+colorName +"\">" +text.Substring(startIndex,endIndex -startIndex);
                output += "<color=\"black\">" +text.Substring(endIndex,i-endIndex);
            }

            textoDialogo.SetText(output);
            await Task.Delay(delayPerCharacter);             
        }
    }

   

    public async Task FlashCompletedMessage(CancellationTokenSource ts)
    {
        float i =0f;
        bool toggle = true;
        while(true)
        {
            if (ts.IsCancellationRequested)
                break;
            
            if (toggle)
            {
                i+=0.1f;
                if (i>= 1)
                    toggle = false;
            }
            else
            {
                i-= 0.1f;
                if (i<=0)
                    toggle = true;
            }
            
            completedMessageImage.transform.localScale = new Vector3(i,i,i);

            await Task.Delay(40);
        }
    }
}
