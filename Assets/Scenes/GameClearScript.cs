using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) // �}�E�X�̍��{�^���������ꂽ�Ƃ�
        {
            SceneManager.LoadScene("TitleScene"); // �V�[�����^�C�g���V�[���ɕύX���郁�\�b�h���Ăяo��
        }
    }
}
