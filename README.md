# EgguWare
ultra gamer unturned cheat

## what you need to use
1. visual studio community https://visualstudio.microsoft.com/downloads/
2. a few brain cells

## how 2 compile and use
1. download da files
2. open EgguWare.csproj
3. at the top click build > rebuild
4. go find the built dll in the bin > (release/debug) folder

## making a loader/bypass
1. download dnspy https://github.com/0xd4d/dnSpy/releases
2. open it and drag in a file referenced by Unturned. heres a list of known goods:
```UnityEngine.IMGUIModule.dll
UnityEngine.TextRenderingModule.dll
UnityEngine.PhysicsModule.dll
UnityEngine.UI.dll
```
3. drag EgguWare.dll into dnspy
4. open the unity dll dropdown and navegate to <modules> ![aaa](https://cdn.discordapp.com/attachments/435943029740666880/677927779622191137/unknown.png)
5. right click <module> and click "Create Method"
6. tick SpecialName, RTSpecialName and Static. Untick HideBySig and type in ".cctor" for the Name. it should look like this ![aasdsd](https://cdn.discordapp.com/attachments/435943029740666880/677928480469418008/unknown.png)
7. press ok, then right click ".cctor() void" under <modules>, "and press edit method body"
8. change "body type" to IL
9. right click anywhere in the open box, and press "append new instruction", under OpCode where it says "nop", click that and change it to "call"
10. in the "operand" part where it says "null", click that, and press method
11. click the EgguWare dropdown (if its missing see step 3) and then EgguWare.dll > - > Load > Start() ![aaa](https://cdn.discordapp.com/attachments/435943029740666880/677929636797349888/unknown.png)
12. press ok, and right click in the box again and append new instruction, for the opcode, select "ret", everything should look like this ![aasdsd](https://cdn.discordapp.com/attachments/435943029740666880/677930027358224414/unknown.png)
13. in the bottom right, click "ok"
14. click file > save module > ok. this will override the old unity file that was there
15. place the EgguWare.dll in your Unturned/Unturned_Data/Managed folder
16. place your assets (EgguWareV1.assets on the repo) in your Unturned/Unturned_Data folder
17. start your game, and you should have your cheat loaded (F1)
