using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Threading;

public class TextDialogManager : MonoBehaviour
{
    private readonly object textLock = new object();
    TMP_Text textoDialogo;
    [SerializeField] GameObject completedMessageImage;
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
    private async Task Push(string text,int delay)
    {
        //It its required to lock this, otherwise the text can be easily overwritten
        await mySemaphoreSlim.WaitAsync();
        try
        {
            completedMessageImage.SetActive(false);
            for (int i = 0; i < text.Length+1; i++)
            {
                textoDialogo.SetText(text.Substring(0,i));
                await Task.Delay(50);             
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
        
    //This method must return a Task so other classes can await for it to continue their execution
    public async Task PushText(Skill skill, MonManager ally, MonManager enemy)
    {
        string text = ally.MonMain.GetNameMon() + " used " + skill.nameSkill + " !";
        await Push(text,0);
    }

    public  async Task PushText(string text)
    {
        await Push(text,0);
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
