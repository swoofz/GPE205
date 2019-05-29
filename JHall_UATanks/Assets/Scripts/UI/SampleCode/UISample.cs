using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UISample : MonoBehaviour {

    public Text myUIText;
    public Image myUIImage;
    public RectTransform myUITextTransform;

    public Sprite alertSprite;
    public Sprite okSprite;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKey(KeyCode.Space)) {

            myUIText.text = "Intruder Alert! Intruder Alert! Alarm! Alarm! Schies dem fester!";

            myUIImage.sprite = alertSprite;

            myUITextTransform.anchoredPosition3D = new Vector3(0, 100, 0);

        } else {

            myUIText.text = "Oll Klear";

            myUIImage.sprite = okSprite;

            myUITextTransform.anchoredPosition3D = new Vector3(0, 0, 0);

        }
    }
}
