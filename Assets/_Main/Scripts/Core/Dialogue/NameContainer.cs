using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 이건 스크린의 박스를 조절할 수 있도록 직접 엑세스하게 해주는 것
/// </summary>

namespace DIALOGUE
{
    [System.Serializable]
    public class NameContainer
    {
        [SerializeField] private GameObject root;
        [SerializeField] private TextMeshProUGUI nameText;

        public void Show(string nameToShow = "")
        {
            root.SetActive(true);

            if (nameToShow != string.Empty)
            {
                nameText.text = nameToShow;
            }
        }
        
        public void Hide()
        {
            root.SetActive(false);
        }













    }
}