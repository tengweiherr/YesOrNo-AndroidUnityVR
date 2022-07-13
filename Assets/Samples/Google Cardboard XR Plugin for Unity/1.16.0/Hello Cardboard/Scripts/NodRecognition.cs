using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodRecognition : MonoBehaviour
{
    private Vector3[] angles;
    private int index;
    private Vector3 centerAngle;
    public GameObject responsiveObject;

    // public List<QuestionsAndAnswers> QnA;
    string startgamestatus = "yes";
    bool endgame = false;
    int score = 0;
    public bool[] answers;
    public int currentQuestion = 0;
    public string[] questions;
    [SerializeField] public TextMeshPro TextOnCube;
    [SerializeField] public TextMeshPro Result;
    [SerializeField] public TextMeshPro Instruction;

    // public Text QuestionTxt;
    // public bool isCorrect = false;

    // Start is called before the first frame update
    void Start()
    {
        ResetGesture();
        //ShowInstruction();
    }

    // Update is called once per frame
    void Update()
    {
        angles[index] = Camera.main.transform.eulerAngles;
        index++;
        if(index==80){
            CheckMovement();
            ResetGesture();
        }
    }

    void CheckMovement(){
        bool right = false, left = false, up = false, down = false;

        for (int i = 0; i < 80; i++)
        {
            if(angles[i].x < centerAngle.x - 20.0f && !up) {
                up = true;
            }else if(angles[i].x > centerAngle.x + 20.0f && !down) {
                down = true;
            }
            if(angles[i].y < centerAngle.y - 20.0f && !left) {
                left = true;
            }else if(angles[i].y > centerAngle.y + 20.0f && !right) {
                right = true;
            }
        }

        if(left && right && !(up & down)){
            print("Shake head");
            if (startgamestatus == "no") {
                responsiveObject.GetComponent<Renderer>().material.color = Color.red;
                Invoke("ShowReadyGo", 3);
            }
            if (!endgame && startgamestatus == "end") checkAnswer(false);
        }

        if(up && down && !(left & right)){
            print("Nod Head");
            if (startgamestatus == "yes") {
                responsiveObject.GetComponent<Renderer>().material.color = Color.green;
                Invoke("ShowNoInstruction", 3);
            }
            if (startgamestatus == "ready")
            {
                responsiveObject.GetComponent<Renderer>().material.color = Color.green;
                Invoke("ShowGame", 3);
            }
            if (!endgame && startgamestatus=="end") checkAnswer(true);
        }
    }

    void ResetGesture(){
        angles = new Vector3[80];
        index = 0;
        centerAngle = Camera.main.transform.eulerAngles;
    }

    void checkAnswer(bool response){
        //if answer correct
        if(answers[currentQuestion] == response){
            responsiveObject.GetComponent<Renderer>().material.color = Color.green;
            score++;
        }
        else
        {
            responsiveObject.GetComponent<Renderer>().material.color = Color.red;
        }
        //after 3 seconds
        Invoke("NextQuestion", 3);
    }

    void NextQuestion() {
        currentQuestion++;
        if (currentQuestion == questions.Length)
        {
            EndGame();
        }
        else {
            responsiveObject.GetComponent<Renderer>().material.color = Color.white;
            TextOnCube.text = questions[currentQuestion];
        }
    }

    void ShowNoInstruction() {
        responsiveObject.GetComponent<Renderer>().material.color = Color.white;
        Instruction.text = "To answer no, Please shake your head for 3 times";
        startgamestatus = "no";
    }

    void ShowReadyGo()
    {
        responsiveObject.GetComponent<Renderer>().material.color = Color.white;
        Instruction.text = "Ready? Nod your head to start the game!";
        startgamestatus = "ready";
    }

    void ShowGame()
    {
        responsiveObject.GetComponent<Renderer>().material.color = Color.white;
        Instruction.text = "";
        startgamestatus = "end";
        TextOnCube.text = questions[currentQuestion];
    }

    void EndGame() {
        responsiveObject.GetComponent<Renderer>().material.color = Color.yellow;
        TextOnCube.text = "";
        Result.text = "You have scored " + score + " out of " + questions.Length + "!";
        endgame = true;
    }

}
