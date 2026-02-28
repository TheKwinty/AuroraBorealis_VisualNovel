using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Testing
{

    public class Testing_Architect : MonoBehaviour
    {
        DialogueSystem ds;
        TextArchitect architech;

        string[] lines = new string[5]
        {
            "Hello welcome home",
            "We happy to see you here again",
            "We hope evetything will be supper now",
            "Let`s create project which bring us a cool paod job",
            "And we will see and swimm in sea and ocean",
        };

        void Start()
        {
            ds = DialogueSystem.instance;
            architech = new TextArchitect(ds.dialogueContainer.dialogueText);
            architech.buildMethod = TextArchitect.BuildMethod.typewriter;
            architech.speed = 0.5f;// також ми можемо сповільнювати або пришвидшувати виведення тексту

        }

            //Перевірка роботи корутини Build i Append, а також системи hurry up
        void Update()
        {
            //Перевірка роботи функції hurry-up
            //string longLine = "French-style meat: Pork (or chicken) cutlets, onions (sliced into rings), mushrooms (optional, sliced), mayonnaise (or sour cream/Greek yogurt), grated hard cheese (like mozzarella or gouda), salt, black pepper, garlic (optional), a pinch of paprika, fresh dill/parsley (optional), a little oil or butter for the pan, and tomatoes (optional slices).";
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (architech.isBuilding)
                {
                    if (!architech.hurryUp)
                        architech.hurryUp = true;
                    else
                        architech.ForceComplite();
                }
                else
                    //architech.Build(longLine);
                    architech.Build(lines[Random.Range(0, lines.Length)]);
            }
            else if (Keyboard.current != null && Keyboard.current.aKey.wasPressedThisFrame)
            {
                //architech.Append(longLine);
                architech.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}
