using UnityEngine;
using UnityEngine.UI;

public class HpHolder : MonoBehaviour
{
    public Image[] m_imageArr;
    public int m_iHp;

    public void SetHp(int _iHp)
    {
        m_iHp = _iHp;
        for (int i = 0; i < m_imageArr.Length; i++)
        {
            m_imageArr[i].enabled = i < m_iHp;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
