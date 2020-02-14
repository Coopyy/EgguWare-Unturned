using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class test : MonoBehaviour
{
    int Tab;
    public Vector2 scrollPosition = Vector2.zero;
    float yes2 = 322;
    bool yes;
    public static string yeet = "text field";
    public Rect windowRect = new Rect(20, 220, 550, 450);
    public Rect windowRect1 = new Rect(800, 220, 550, 450);
    public static Color GUIColor;
    public GUISkin skin;
    public static List<GUIContent> buttons = new List<GUIContent>();
    // Use this for initialization
    void Start()
    {
        GUIColor = GUI.color;

    }

    // Update is called once per frame


    void DrawOutline(Rect r, string t, int strength)
    {
        GUI.color = new Color(0, 0, 0, 1);
        int i;
        for (i = -strength; i <= strength; i++)
        {
            GUI.Label(new Rect(r.x - strength, r.y + i, r.width, r.height), t);
            GUI.Label(new Rect(r.x + strength, r.y + i, r.width, r.height), t);
        }
        for (i = -strength + 1; i <= strength - 1; i++)
        {
            GUI.Label(new Rect(r.x + i, r.y - strength, r.width, r.height), t);
            GUI.Label(new Rect(r.x + i, r.y + strength, r.width, r.height), t);
        }
        GUI.color = new Color(1, 1, 1, 1);
    }
    int i = -40;
    bool sex = false;
    void OnGUI()
    {
        if (GUI.Button(new Rect(200, 200, 20, 30), "openmenu"))
        {
            if (sex == false)
            {
                sex = true;
            }
            else
            {
                sex = false;
                i = -40;
            }
        }
            
        if (sex == false)
            return;
        GUIStyle guiStyle = new GUIStyle("label");
        guiStyle.margin = new RectOffset(10, 10, 0, 5);
        guiStyle.fontSize = 22;
        if (i < 0)
            i++;
        GUI.skin = skin;
        windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "Aimbot");
        windowRect1 = GUILayout.Window(20, windowRect1, DoMyWindow1, "Aimbot");
        GUILayout.BeginArea(new Rect(0, i, Screen.width, 40), style: "NavBox");
        GUILayout.BeginHorizontal();
        GUI.color = new Color32(34, 177, 76, 255);
        GUILayout.Label("<b>EgguWare</b> <size=15>v1.0.3</size>", guiStyle);
        GUI.color = GUIColor;
        Tab = GUILayout.Toolbar(Tab, new GUIContent[] { new GUIContent("Visuals"), new GUIContent("Aimbot"), new GUIContent("Players"), new GUIContent("Weapons"), new GUIContent("Misc"), new GUIContent("Settings") }, style: "TabBtn");
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void DoMyWindow(int windowID)
    {
        GUILayout.Space(0);
        GUILayout.BeginArea(new Rect(10, 35, 260, 400), style: "box", text: "Silent Aimbot");
        yes = GUILayout.Toggle(yes, "asdasd");
        yes = GUILayout.Toggle(yes, "asdasd");
        yes = GUILayout.Toggle(yes, "asdasd");
        yes = GUILayout.Toggle(yes, "asdasd");

        yes2 = GUILayout.HorizontalSlider(yes2, 0, 400);

        yeet = GUILayout.TextField(yeet);

        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(280, 35, 260, 400), style: "box", text: "Aimlock");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        GUI.DragWindow();
    }
     
    void DoMyWindow1(int windowID)
    {
        GUILayout.Space(-5);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.Button("asdasd");
       
        GUILayout.Button("asdasd", style: "SelectedButton");
        GUILayout.BeginVertical(style: "SelectedButtonDropdown");
        yes = GUILayout.Toggle(yes, "asdasd");
        yes = GUILayout.Toggle(yes, "asdasd");
        yes = GUILayout.Toggle(yes, "asdasd");
        yes = GUILayout.Toggle(yes, "asdasd");
        GUILayout.EndVertical();
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.Button("asdasd");
        GUILayout.EndScrollView();
        GUI.DragWindow();
    }
}
