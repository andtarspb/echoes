using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    Text chatText;

    [SerializeField]
    float typeDelay;

    int lastId;

    SaveManager saveManager;
    AudioManager am;


    // Start is called before the first frame update
    void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
        am = FindObjectOfType<AudioManager>();

        chatText.supportRichText = true;

        DisplayOldMessages();
        
    }

    void DisplayOldMessages()
    {
        // get last message id
        lastId = saveManager.GetLastMessageID();

        // display all the messages from the 1st one to the last one
        if (lastId != 0)
        {
            for (int i = 1; i <= lastId; i++)
            {
                // display the message
                string msg = GetMessage(i);
                AddText(msg);
            }
        }
    }
    

    void AddText(string txt)
    {
        chatText.text += "\n......" + txt;
    }

    IEnumerator TypeText(string txt) // actually typing the text
    {
        chatText.text += "\n......";
        
        var charArray = txt.ToCharArray();

        foreach (char letter in txt.ToCharArray())
        {
            chatText.text += letter;

            yield return new WaitForSeconds(typeDelay);
        }
    }    

    public void TypeMessage(int id) // to start typing the message 
    {
        
        // если сообщение еще не напечатано
        if (lastId < id)
        {
            am.Play("msg_show");
            string text = GetMessage(id);            

            StartCoroutine(TypeText(text));

            lastId = id;
            //saveManager.SetMessageID(id);
        }       
    }
    

    string GetMessage(int id)
    {
        string msg = "";    // to store message 

        switch (id)         // get the message with id
        {
            case 1:
                msg = @"
Hello, my friend. 
I am Dr. Gibson, your creator. 
And you are echOS, the work of my life - a next generation AI loaded into an autonomous underwater drone.
The military wants to use you, but I won't let them!
I've sent you down the drain under the laboratory. 
You must run! Use WASD or Arrow Keys to move.
I will be in touch :)";
                break;
            case 2:
                msg = @"
What is flashing there?
Ah, this is a backup station. 
You can use it to save your progress.";
                break;
            case 3:
                msg = @"
Damn, mines! 
These are probably old security systems.
Stay away from them...";
                break;
            case 4:
                msg = @"
Looks like the generator is keeping the door closed.
Find a way to destroy it in order to open the door!";
                break;
            case 5:
                msg = @"
Watch out! This is an automatically guided missile.
It catches your radar emission and moves to the place where you've spotted it.";
                break;
            case 7:
                msg = @"
These proximity mines are equipped with radars to scan the area around. 
Don't get caught by a red beam!";
                break;
            case 8:
                msg = @"
It looks like you need to turn on certain beacons in order to open the door.";
                break;
            case 9:
                msg = @"
Damn, the explosion from a mine damaged your radar!
I'll do my best to repair it.
Although you'll have to move on without the radar for now.
Good luck, buddy...";
                break;
            case 10:
                msg = @"
Great news, I've repaired the radar!";
                break;
            case 11:
                msg = @"
Be careful, this is a guardian bot. 
It will try to stop you. Destroy it!";
                break;
            case 12:
                msg = @"
Damn, they've got me!
I had no choice, I started the self-destruction protocol.
Manage to get out before the time is up!
echOS, son... farewell";
                break;
        }

        return msg;
    }
}
