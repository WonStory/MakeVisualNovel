using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager
{
    public static List<string> ReadTextFile(string filePath, bool includeBlankLines = true) //빈칸을 포함할지말지 불키, 파일패스 지정
    {
        if (!filePath.StartsWith('/'))
        {
            filePath = FilePaths.root + filePath;
        }
        List<string> lines = new List<string>(); //해당 파일에 있는 모든 줄을 읽어야댐
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (includeBlankLines || !string.IsNullOrWhiteSpace(line)) //빈칸이나 널 혹은 스페이스인지 확인하고 데이터를 가져온다.
                    {
                        lines.Add(line);
                    }
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            Debug.LogError($"File not found: '{ex.FileName}'");//맞춤법이 틀렸는지 어떤지 알 수 있음
        }

        return lines;

    }

    public static List<string> ReadTextAsset(string filePath, bool includeBlankLines = true) //빈칸을 포함할지말지 불키, 파일패스 지정
    {
        TextAsset asset = Resources.Load<TextAsset>(filePath);
        if (asset == null)
        {
            Debug.LogError($"Asset not found: '{filePath}'");
            return null;
        }

        return ReadTextAsset(asset, includeBlankLines);
    }

    public static List<string> ReadTextAsset(TextAsset asset, bool includeBlankLines = true)
    {
        List<string> lines = new List<string>();
        using (StringReader sr = new StringReader(asset.text))
        {
            while (sr.Peek() > -1) //끝에 다다랐다는 것을 뜻하므로
            {
                string line = sr.ReadLine();
                if (includeBlankLines || !string.IsNullOrWhiteSpace(line)) //빈칸이나 널 혹은 스페이스인지 확인하고 데이터를 가져온다.
                {
                    lines.Add(line);
                }
            }
        }
        return lines;
    }





}
