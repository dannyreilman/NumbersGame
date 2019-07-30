using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipageHandler : MonoBehaviour
{
    public GameObject buttonPrefab;
    int index = 0;
    List<Button> buttons;

    // Start is called before the first frame update
    void Start()
    {
        buttons = new List<Button>();
    }

    float HIDE_DISTANCE = 10000.0f;
    void hidePage(GameObject page)
    {
        page.transform.localPosition = new Vector2(HIDE_DISTANCE,0);
    }
    
    void showPage(GameObject page)
    {
        page.transform.localPosition = new Vector2(0,0);
    }

    void buttonClicked(int buttonIndex)
    {
        Debug.Log("Click");
        index = buttonIndex;
        for(int i = 0; i < transform.childCount - 1; ++i)
        {
            if(i == index)
                showPage(transform.GetChild(i).gameObject);
            else
                hidePage(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int num_pages = transform.childCount - 1;
        while(buttons.Count < num_pages)
        {
            GameObject spawnedTab = Instantiate(buttonPrefab, transform.GetChild(num_pages));
            spawnedTab.GetComponentInChildren<Text>().text = (buttons.Count + 1).ToString();
            if(buttons.Count != index)
                hidePage(transform.GetChild(buttons.Count).gameObject);
            buttons.Add(spawnedTab.GetComponent<Button>());
            {
                int current_index = buttons.Count - 1;
                buttons[buttons.Count - 1].onClick.AddListener(delegate{buttonClicked(current_index);});
            }
        }
        while(buttons.Count > num_pages)
        {
            Button b = buttons[buttons.Count - 1];
            buttons.Remove(b);
            Destroy(b.gameObject);
        }
    }
}
