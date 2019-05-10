using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public enum InputScheme { WASD, arrowKeys };                            // Create an enum to have different inputs
    public InputScheme input = InputScheme.WASD;                            // Get what type of inputs want to use
    public enum Action { Shoot, None };                                     // Create a enum to get what kind of action each key does
    public enum MoveActions { Forward, Backward, Left, Right, None };       // Creat a enum to get the direction want to go

    // Variables that get user input keys
    [HideInInspector] public Action action;           // User Actions
    [HideInInspector] public MoveActions move1;  // User movement
    [HideInInspector] public MoveActions move2;  // Second value for User movement


    void Start() {
        // Initialize our all Actions to None
        action = Action.None;           
        move1 = MoveActions.None;  
        move2 = MoveActions.None;  
    }

    // Update is called once per frame
    void Update() {
        action = Action.None;               // Set to no key action

        switch (input) {
            case InputScheme.WASD:                      // WASD Input Scheme
                // Reset after key is let go of a key
                move1 = MoveActions.None;               // No key currently
                move2 = MoveActions.None;               // No Key currently

                if (Input.GetKey(KeyCode.W)) {          // Set Forward key
                    move1 = MoveActions.Forward;
                    SetSecondInput();
                }
                if (Input.GetKey(KeyCode.S)) {          // Set Backwards key
                    move1 = MoveActions.Backward;
                    SetSecondInput();
                }
                if (Input.GetKey(KeyCode.D)) {          // Set Right key     
                    move1 = MoveActions.Right;
                    SetSecondInput();
                }
                if (Input.GetKey(KeyCode.A)) {          // Set Left Key    
                    move1 = MoveActions.Left;
                    SetSecondInput();           
                }
                break;
            case InputScheme.arrowKeys:                 // Arrow Keys Input Scheme
                // Reset after key is let go of a key
                move1 = MoveActions.None;               // No key currently
                move2 = MoveActions.None;               // No Key currently

                if (Input.GetKey(KeyCode.UpArrow)) {    // Set Forward key
                    move1 = MoveActions.Forward;
                    SetSecondInput();
                }
                if (Input.GetKey(KeyCode.DownArrow)) {  // Set Backwards Key
                    move1 = MoveActions.Backward;
                    SetSecondInput();
                }
                if (Input.GetKey(KeyCode.RightArrow)) { // Set Right Key
                    move1 = MoveActions.Right;
                    SetSecondInput();
                }
                if (Input.GetKey(KeyCode.LeftArrow)) {  // Set Left key
                    move1 = MoveActions.Left;
                    SetSecondInput();
                }
                break;
        }


        if (Input.GetKey(KeyCode.Space)) {
            action = Action.Shoot;          // Set shoot key
        }
    } // End Update

    // Function: SETSECONDINPUT
    // Second movement input to have a two at one time direction movement
    void SetSecondInput() {
        if(move2 == MoveActions.None) {
            move2 = move1;
        }
    }
}
