using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDatabase
{
    private Dictionary<string, Delegate> database = new Dictionary<string, Delegate>();

    public bool HasCommand(string commandName) => database.ContainsKey(commandName);

    public void AddCommand(string commandName, Delegate command)
    {
        if (!database.ContainsKey(commandName))
        {
            database.Add(commandName, command);
        }

        else
        {
            Debug.LogError($"Command already exists in the database '{commandName}'");
        }
    }

    public Delegate GetCommand(string commandName) //데이터베이스에 엑세스해야되지만 숨겨져있어서 명령을 가져오는 함수를 만들어서 공개한다. 대리자를 통해 명령관리자를 찾는다
    {
        if (!database.ContainsKey(commandName))
        {
            Debug.LogError($"Command '{commandName}' does not exist in the database!");
            return null;
        }

        return database[commandName];
    }


}
