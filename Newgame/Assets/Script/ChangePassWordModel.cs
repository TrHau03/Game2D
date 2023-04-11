using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePassWordModel 
{
    public ChangePassWordModel(string username, string oldpassword, string newpassword)
    {
        this.username = username;
        this.oldpassword = oldpassword;
        this.newpassword = newpassword;
    }
    
    public string username { get; set; }
    public string oldpassword { get; set; }
    public string newpassword { get; set; }
}
