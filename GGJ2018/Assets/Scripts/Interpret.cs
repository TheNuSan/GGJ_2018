using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpret : MonoBehaviour {

    public static int MinWords = 3;

    public List<string> Verbs;
    public List<string> People;
    public List<string> Things;

    public List<string> SwapRules = new List<string>();

    List<string> VerbsBase = new List<string>();
    List<string> PeopleBase = new List<string>();
    List<string> ThingsBase = new List<string>();

    public InterpretError MyInterpretError;

    // Use this for initialization
    void Start()
    {

    }

    public void init() {
        VerbsBase.Add("move");

        PeopleBase.Add("dwarf");
        PeopleBase.Add("elf");
        PeopleBase.Add("vampire");
        PeopleBase.Add("warewolf");

        // directions
        /*ThingsBase.Add("up");
        ThingsBase.Add("down");
        ThingsBase.Add("right");
        ThingsBase.Add("left");*/
        ThingsBase.Add("north");
        ThingsBase.Add("south");
        ThingsBase.Add("east");
        ThingsBase.Add("west");

        Verbs = new List<string>(VerbsBase);
        People = new List<string>(PeopleBase);
        Things = new List<string>(ThingsBase);

        /*Debug.Log("init()");
        Debug.Log("People");
        Debug.Log(PrintedList(People));
        Debug.Log("Verbs");
        Debug.Log(PrintedList(Verbs));
        Debug.Log("Things");
        Debug.Log(PrintedList(Things));*/
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void DoInterpret()
    {
        // get the sentence from the base



        // interpret the sentence

        // syntax error


    }


    public void Shuffle(int difficulty) {

        Verbs = new List<string>(VerbsBase);
        People = new List<string>(PeopleBase);
        Things = new List<string>(ThingsBase);
        
        SwapRules.Clear();

        switch (difficulty) {

            case 5:
                // Everything is fucked up
                //break;

            case 4:
                // Peoples are mixed with Things
                ShuffleBetween(PeopleBase, ThingsBase, People, Things, 3);
                break;

            case 3:
            // Verbs are shuffled together
                // ShuffleWithin(VerbsBase,Verbs);
                //break;

            case 2:
                // Things are shuffled together
                ShuffleWithin(ThingsBase, Things, 2);
                break;

            case 1:
                // People are shuffled together
                //ShuffleWithin(PeopleBase, People, 1);
                //ShuffleWithin(PeopleBase, People, 2);
                ShuffleWithin(PeopleBase, People, 3);
                break;

            case 0:
            default:
            // the langage is in understandable
                break;
        }

        Debug.Log("Shuffle()");
        Debug.Log("People");
        Debug.Log(PrintedList(People));
        Debug.Log("Verbs");
        Debug.Log(PrintedList(Verbs));
        Debug.Log("Things");
        Debug.Log(PrintedList(Things));

        Debug.Log("SwapRules");
        Debug.Log(PrintedList(SwapRules));
    }

    void ShuffleWithin(List<string> inputList, List<string> outputList, int nb_swap) {
        List<bool> swapped = new List<bool>();
        int i, j = 0, k = 0;

        // init swapped list
        for (i = 0; i < inputList.Count; i++)
        {
            swapped.Add(false);
        }

        // maw swaps possible
        if (nb_swap * 2 > inputList.Count)
            nb_swap = inputList.Count / 2;

        for (i = 0; i < nb_swap; i++) {
            // pick first candidate to swap
            j = Random.Range(0, inputList.Count);
            while (swapped[j]) {
                j++;
                if (j == inputList.Count)
                    j = 0;
            }
            swapped[j] = true;

            // pick second candidate to swap
            k = Random.Range(0, inputList.Count);
            while (swapped[k])
            {
                k++;
                if (k == inputList.Count)
                    k = 0;
            }
            swapped[k] = true;

            // do the swap
            outputList[j] = inputList[k];
            outputList[k] = inputList[j];

            // record rule
            SwapRules.Add(outputList[j] + " <--> " + outputList[k]);
        }
    }


    void ShuffleBetween(
        List<string> inputList1, List<string> inputList2,
        List<string> outputList1, List<string> outputList2,
        int nb_swap)
    {
        List<bool> swapped1 = new List<bool>();
        List<bool> swapped2 = new List<bool>();
        int i, j = 0, k = 0;

        // init swapped list
        for (i = 0; i < inputList1.Count; i++)
        {
            swapped1.Add(false);
        }
        for (i = 0; i < inputList2.Count; i++)
        {
            swapped2.Add(false);
        }

        // max swaps possible
        if (nb_swap > inputList1.Count)
            nb_swap = inputList1.Count;
        if (nb_swap > inputList2.Count)
            nb_swap = inputList2.Count;

        for (i = 0; i < nb_swap; i++)
        {
            // pick first candidate to swap
            j = Random.Range(0, inputList1.Count);
            while (swapped1[j])
            {
                j++;
                if (j == inputList1.Count)
                    j = 0;
            }
            swapped1[j] = true;

            // pick second candidate to swap
            k = Random.Range(0, inputList2.Count);
            while (swapped2[k])
            {
                k++;
                if (k == inputList2.Count)
                    k = 0;
            }
            swapped2[k] = true;

            // do the swap
            outputList1[j] = inputList2[k];
            outputList2[k] = inputList1[j];

            // record rule
            SwapRules.Add(outputList1[j] + " <--> " + outputList2[k]);
        }
    }


    string PrintedList(List<string> inputList)
    {
        string res = "[ ";

        foreach (string o in inputList) {
            res += o + " ";
        }
        res += "]";
        return res;
    }
}
