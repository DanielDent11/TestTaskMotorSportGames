using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    private static TrafficLight trafficLight;

    void Start()
    {
        trafficLight = new TrafficLight();
    }

    public void SetToRed()
    {
        trafficLight.topLamp.SetON();
        trafficLight.middleLamp.SetOFF();
        trafficLight.bottomLamp.SetOFF();
        trafficLight.currentState = TrafficLight.States.Red;
    }

    public void SetToYellow()
    {
        trafficLight.topLamp.SetOFF();
        trafficLight.middleLamp.SetON();
        trafficLight.bottomLamp.SetOFF();
        trafficLight.currentState = TrafficLight.States.Yellow;
    }

    public void SetToGreen()
    {
        trafficLight.topLamp.SetOFF();
        trafficLight.middleLamp.SetOFF();
        trafficLight.bottomLamp.SetON();
        trafficLight.currentState = TrafficLight.States.Green;
    }

    public void SetToOFF()
    {
        trafficLight.topLamp.SetOFF();
        trafficLight.middleLamp.SetOFF();
        trafficLight.bottomLamp.SetOFF();
        trafficLight.currentState = TrafficLight.States.OFF;
    }

    public class TrafficLight
    {
        public enum States { OFF, Red, Yellow, Green, FlashingRed, FlashingGreen };

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
            lampGameObject.GetComponent<MeshRenderer>().sharedMaterial.color = onColor;
            currentState = States.ON;
        }

        public void SetOFF()
        {
            lampGameObject.GetComponent<MeshRenderer>().sharedMaterial.color = offColor;
            currentState = States.OFF;
        }
    }
}
