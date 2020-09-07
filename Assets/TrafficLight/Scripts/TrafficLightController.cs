using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrafficLightController : MonoBehaviour
{
    public CycleItem[] cycle;
    public Text currentStateText;

    private static TrafficLight trafficLight;
    private IEnumerator cycleCoroutine;
    private IEnumerator flashingCoroutine;

    GameObject buttons;

    void Start()
    {
        trafficLight = new TrafficLight();
        buttons = GameObject.Find("/Canvas/Buttons");
        SetToOFF();        
    }

    void OnDisable()
    {
        SetToOFF();
    }

    public void FadeOut()
    {
        if (trafficLight.currentState == TrafficLight.States.OFF)
        {
            return;
        }
        else if (trafficLight.currentState == TrafficLight.States.Red)
        {
            StartCoroutine(FadeOutCoroutine(trafficLight.topLamp, 0.01f));
        }
        else if (trafficLight.currentState == TrafficLight.States.Yellow)
        {
            StartCoroutine(FadeOutCoroutine(trafficLight.middleLamp, 0.01f));

        }
        else if (trafficLight.currentState == TrafficLight.States.Green)
        {
            StartCoroutine(FadeOutCoroutine(trafficLight.bottomLamp, 0.01f));
        }
    }

    public void SetToState(TrafficLight.States state)
    {
        if (state == TrafficLight.States.OFF)
        {
            SetToOFF();
        }
        else if (state == TrafficLight.States.Red)
        {
            SetToRed();
        }
        else if (state == TrafficLight.States.Yellow)
        {
            SetToYellow();
        }
        else if (state == TrafficLight.States.Green)
        {
            SetToGreen();
        }
        else if (state == TrafficLight.States.FlashingGreen)
        {
            SetToFlashingGreen();
        }
        else if (state == TrafficLight.States.FlashingRed)
        {
            SetToFlashingRed();
        }
        else if (state == TrafficLight.States.FlashingYellow)
        {
            SetToFlashingYellow();
        }
    }

    public void RunCycle()
    {
        SetToOFF();
        cycleCoroutine = CycleCoroutine();
        StartCoroutine(cycleCoroutine);
        foreach (Transform button in buttons.transform)
        {
            if (!button.gameObject.CompareTag("StopCycle"))
            {
                button.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void StopCycle()
    {
        if (cycleCoroutine != null)
        {
            StopCoroutine(cycleCoroutine);
            SetToOFF();
        }
        foreach (Transform button in buttons.transform)
        {
            button.GetComponent<Button>().interactable = true;
        }
    }

    public void SetToRed()
    {
        SetToOFF();
        trafficLight.currentState = TrafficLight.States.Red;
        currentStateText.text = "Red";
        trafficLight.topLamp.SetON();
        trafficLight.middleLamp.SetOFF();
        trafficLight.bottomLamp.SetOFF();
    }

    public void SetToYellow()
    {
        SetToOFF();
        trafficLight.currentState = TrafficLight.States.Yellow;
        currentStateText.text = "Yellow";
        trafficLight.topLamp.SetOFF();
        trafficLight.middleLamp.SetON();
        trafficLight.bottomLamp.SetOFF();
    }

    public void SetToGreen()
    {
        SetToOFF();
        trafficLight.currentState = TrafficLight.States.Green;
        currentStateText.text = "Green";
        trafficLight.topLamp.SetOFF();
        trafficLight.middleLamp.SetOFF();
        trafficLight.bottomLamp.SetON();
    }

    public void SetToOFF()
    {
        if (flashingCoroutine != null)
            StopCoroutine(flashingCoroutine);
        trafficLight.currentState = TrafficLight.States.OFF;
        if (currentStateText != null)
        {
            currentStateText.text = "OFF";
        }
        if (trafficLight.topLamp != null) 
            trafficLight.topLamp.SetOFF();
        if (trafficLight.middleLamp != null)
            trafficLight.middleLamp.SetOFF();
        if (trafficLight.bottomLamp != null)
            trafficLight.bottomLamp.SetOFF();
    }

    public void SetToFlashingRed(bool clearControllerStateBeforeSetting = true)
    {
        SetToOFF();
        currentStateText.text = "FlashingRed";
        trafficLight.currentState = TrafficLight.States.FlashingRed;
        flashingCoroutine = FlashingCoroutine(trafficLight.topLamp, 0.2f);
        StartCoroutine(flashingCoroutine);
    }

    public void SetToFlashingYellow(bool clearControllerStateBeforeSetting = true)
    {
        SetToOFF();
        currentStateText.text = "FlashingYellow";
        trafficLight.currentState = TrafficLight.States.FlashingYellow;
        flashingCoroutine = FlashingCoroutine(trafficLight.middleLamp, 0.2f);
        StartCoroutine(flashingCoroutine);
    }

    public void SetToFlashingGreen(bool clearControllerStateBeforeSetting = true)
    {
        SetToOFF();
        currentStateText.text = "FlashingGreen";
        trafficLight.currentState = TrafficLight.States.FlashingGreen;
        flashingCoroutine = FlashingCoroutine(trafficLight.bottomLamp, 0.2f);
        StartCoroutine(flashingCoroutine);
    }

    public class TrafficLight
    {
        public enum States { OFF, Red, Yellow, Green, FlashingRed, FlashingGreen, FlashingYellow };

        public States currentState;

        public TrafficLightLamp topLamp;
        public TrafficLightLamp middleLamp;
        public TrafficLightLamp bottomLamp;

        public TrafficLight()
        {
            GameObject topLamp = GameObject.Find("TrafficLightBase/TrafficLightHead/TopLamp");
            GameObject middleLamp = GameObject.Find("TrafficLightBase/TrafficLightHead/MiddleLamp");
            GameObject bottomLamp = GameObject.Find("TrafficLightBase/TrafficLightHead/BottomLamp");

            this.topLamp = new TrafficLightLamp(new Color(0.1f, 0f, 0f, 0f), new Color(1f, 0f, 0f, 0f), topLamp);
            this.middleLamp = new TrafficLightLamp(new Color(0.1f, 0.1f, 0f, 0f), new Color(1f, 1f, 0f, 0f), middleLamp);
            this.bottomLamp = new TrafficLightLamp(new Color(0f, 0.1f, 0f, 0f), new Color(0f, 1f, 0f, 0f), bottomLamp);

            currentState = States.OFF;
            this.topLamp.SetOFF();
            this.middleLamp.SetOFF();
            this.bottomLamp.SetOFF();
        }

    }

    public class TrafficLightLamp
    {
        public enum States { ON, OFF };

        public States currentState;

        private Color offColor;
        private Color onColor;

        private GameObject lampGameObject;

        public TrafficLightLamp(Color offColor, Color onColor, GameObject lampGameObject)
        {
            this.offColor = offColor;
            this.onColor = onColor;
            this.lampGameObject = lampGameObject;

            SetOFF();
        }

        public void SetON()
        {
            lampGameObject.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color", onColor);
            lampGameObject.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_EmissionColor", onColor);
            lampGameObject.GetComponent<MeshRenderer>().sharedMaterial.EnableKeyword("_EMISSION");
            currentState = States.ON;
        }

        public void SetOFF()
        {
            lampGameObject.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color", offColor);
            lampGameObject.GetComponent<MeshRenderer>().sharedMaterial.DisableKeyword("_EMISSION");
            currentState = States.OFF;
        }

        public void SetEmissionIntensity(float intensity)
        {
            var renderer = lampGameObject.GetComponent<MeshRenderer>();
            Color oldColor = renderer.sharedMaterial.color;
           // float factor = Mathf.Pow(2, intensity);
            Color newColor = new Color((oldColor.r != 0f) ? 1f * intensity : 0f, (oldColor.g != 0f) ? 1f * intensity : 0f, (oldColor.b != 0f) ? 1f * intensity : 0f);
            renderer.sharedMaterial.SetColor("_EmissionColor", newColor);
            renderer.sharedMaterial.SetColor("_Color", newColor);
            //DynamicGI.SetEmissive(renderer, newColor);
        }
    }

    [System.Serializable]
    public struct CycleItem
    {
        public TrafficLight.States state;
        // Duration in seconds
        public float duration;
    }

    private IEnumerator CycleCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < cycle.Length; ++i)
            {
                SetToState(cycle[i].state);
                yield return new WaitForSeconds(cycle[i].duration);
            }
        }
    }

    private IEnumerator FadeOutCoroutine(TrafficLightLamp lamp, float duration)
    {
        const float Step = 0.05f;
        float intensity = 1f;
        int numberOfFrames = Mathf.RoundToInt((intensity - 0.1f) / Step);
        float delay = duration / numberOfFrames;
        for (int i = 0; i < numberOfFrames; ++i)
        {
            lamp.SetEmissionIntensity(intensity);
            intensity -= Step;
            yield return new WaitForSeconds(delay);
        }
        lamp.SetOFF();
    }

    private IEnumerator FlashingCoroutine(TrafficLightLamp lamp, float sec)
    {
        while (true)
        {
            lamp.SetON();
            StartCoroutine(FadeOutCoroutine(lamp, sec));
            yield return new WaitForSeconds(sec + 0.4f);
        }
    }
}

