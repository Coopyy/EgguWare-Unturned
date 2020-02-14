using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    int Tab;
    public static MenuTab SelectedTab;
    public Vector2 scrollPosition = Vector2.zero;
    float yes2 = 322;
    bool yes;
    public static string yeet = "gays lol";
    public Rect windowRect = new Rect(20, 20, 0, 200);
    public static Color GUIColor;
    public GUISkin skin;
    public static List<GUIContent> buttons = new List<GUIContent>();
    // Use this for initialization
    void Start()
    {
        GUIColor = GUI.color;


        foreach (MenuTab val in Enum.GetValues(typeof(MenuTab)))
            buttons.Add(new GUIContent(Enum.GetName(typeof(MenuTab), val)));
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

    void OnGUI()
    {
        GUI.skin = skin;
        windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "<size=15>EgguWare</size>");
    }

    void DoMyWindow(int windowID)
    {
        Tab = GUILayout.Toolbar(Tab, new GUIContent[] { new GUIContent("Cheats"), new GUIContent("Retardation"), new GUIContent("Player"), new GUIContent("Other") });
        GUILayout.BeginHorizontal();
        GUILayout.Button("hello");
        GUILayout.Button("hello");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Button("hello");
        GUILayout.Button("hello");
        GUILayout.EndHorizontal();
        GUILayout.Box("asdasds");
        yes = GUILayout.Toggle(yes, "yeet");
        GUILayout.Label("niggaz: " + yes2);
        yes2 = GUILayout.HorizontalSlider(yes2, 0, 400);
        //GUI.skin = null;
        GUILayout.BeginHorizontal();
        yeet = GUILayout.TextField(yeet);
        GUILayout.Button("benis");
        GUILayout.EndHorizontal();

        SelectedTab = (MenuTab)GUILayout.Toolbar((int)SelectedTab, buttons.ToArray());

        GUI.DragWindow();
    }
    public enum MenuTab
    {
        Visuals = 0,
        Aimbot = 1,
        Misc = 2,
        Weapons = 3,
        Config = 4
    }
}
