using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFiles : MonoBehaviour
{
    [SerializeField] private TextAsset fileName; //파일지정자를 정하지 않으면 로드가 안된다.
    //위처럼 하고 readtextasset을 쓰면 쉽게 객체만 바꿔서 사용가능하다.





    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        List<string> lines = FileManager.ReadTextAsset(fileName,false); //트루 폴스에 따라 빈칸(빈줄)을 스킾할지 표현할지 정해짐

        foreach(string line in lines)
            Debug.Log(line);

        yield return null;
    }
}
