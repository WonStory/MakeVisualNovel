using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_DatabaseExtension_Example : CMD_DatabaseExtension // 작동하는 지 확인하기 위한 스크립트
{
    new public static void Extend(CommandDatabase database)
    {
        //매개변수 없이 커맨드를 넣겠다
        database.AddCommand("print", new Action(PrintDefaultMessage));
    }

    private static void PrintDefaultMessage()
    {
        Debug.Log("Printing a defalut message to console");
    }




}
