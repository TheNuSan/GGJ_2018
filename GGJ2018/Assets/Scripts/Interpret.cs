using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interpret : MonoBehaviour {

    public static Interpret Instance;
    public static int MinWords = 3;
    
    public enum wordType {
        UNKOWN,
        VERB,
        PERSON,
        THING
    };

    public List<string> Verbs;
    public List<string> People;
    public List<string> Things;

    public List<string> SwapRules = new List<string>();

    List<string> VerbsBase = new List<string>();
    List<string> PeopleBase = new List<string>();
    List<string> ThingsBase = new List<string>();

    List<List<string>> dictionary = new List<List<string>>();

    public InterpretError MyInterpretError;
    public ActionManager MyActionManager;
    public Text RulesText;

    void Awake()
    {
        Instance = this;
        init();
    }

    // Use this for initialization
    void Start()
    {
        
    }

    public void init() {
        VerbsBase.Add("move");
        VerbsBase.Add("pick");
        VerbsBase.Add("use");

        PeopleBase.Add("Dwarf");
        PeopleBase.Add("Elf");
        PeopleBase.Add("Vampire");
        PeopleBase.Add("Werewolf");

        // directions
        /*ThingsBase.Add("up");
        ThingsBase.Add("down");
        ThingsBase.Add("right");
        ThingsBase.Add("left");*/
        ThingsBase.Add("North");
        ThingsBase.Add("South");
        ThingsBase.Add("East");
        ThingsBase.Add("West");
        ThingsBase.Add("Key");
        ThingsBase.Add("Shovel");
        ThingsBase.Add("Hammer");

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

    public bool DoInterpret(string[] words)
    {
        bool res = false;
        int i;
        int nb_verbs = 0;
        int nb_people = 0;
        int nb_things = 0;
        bool syntax_error = false;

        //List<wordType> wordtypes = new List<wordType>();
        List<string> ordered_words = new List<string>();
        List<string> translated_sentence = new List<string>();
        string person = "";
        string verb = "";
        wordType word_type = wordType.UNKOWN;
        int translated_index = -1;

        // get the sentence from the base
        for (i = 0; i < words.Length; i++) {
            word_type = (wordType)findword(words[i].ToLower(), out translated_index);
            
            switch (word_type) {
                case wordType.VERB:
                    nb_verbs++;
                    verb = VerbsBase[translated_index];
                    translated_sentence.Add(verb);
                    break;

                case wordType.PERSON:
                    nb_people++;
                    person = PeopleBase[translated_index];
                    translated_sentence.Add(person);
                    break;

                case wordType.THING:
                    nb_things++;
                    ordered_words.Add(ThingsBase[translated_index]);
                    translated_sentence.Add(ThingsBase[translated_index]);
                    break;

                case wordType.UNKOWN:
                default:
                    if (i < MinWords) {
                        // syntax error    
                        MyInterpretError.SyntaxError(words[i]);
                        syntax_error = true;
                    }
                    break;
            }

            if (syntax_error)
                return res;
        }
        
        // insure the sentence means something
        if (nb_people == 1 && nb_verbs == 1 && nb_things > 0) {
            // interpret the sentence
            //Debug.Log("DoInterpret");
            //Debug.Log(PrintedList(translated_sentence));

            // put the words into order
            ordered_words.Insert(0, verb);
            ordered_words.Insert(0, person);

            if (CheckConsistency(ordered_words))
            {
                // call action
                res = MyActionManager.PerformAction(ordered_words);
            }
            
            if (res)
                MyInterpretError.Success(person);
        }
        else {
            // the sentence has no meaning
            MyInterpretError.MeaningError(translated_sentence);
        }

        return res;
    }

    int findword(string word, out int translated)
    {
        wordType res = wordType.UNKOWN;
        wordType curr_type = wordType.UNKOWN;
        int j = 0;
        translated = -1;

        foreach (List<string> l in dictionary)
        {
            curr_type += 1;
            j = 0;
            foreach (string s in l)
            {
                if (word == s.ToLower()) {
                    translated = j;
                    res = curr_type;
                    break;
                }
                j++;
            }
        }
        return (int)res;
    }

    bool CheckConsistency(List<string> command) {
        bool res = false;
        string verb = command[1];
        string thing = command[2];

        switch (verb) {

            case "pick":
            case "use":
                res = ThingsBase.IndexOf(thing) >= ThingsBase.IndexOf("Key");
                if (!res)
                    MyInterpretError.InconsistentAction(command[0], thing);
                break;

            case "move":
            default:
                res = ThingsBase.IndexOf(thing) <= ThingsBase.IndexOf("West");
                if (!res)
                    MyInterpretError.InconsistentDirection(command[0], thing);
                break;
        }

        return res;
    }


    public void Shuffle(int difficulty) {

        Verbs = new List<string>(VerbsBase);
        People = new List<string>(PeopleBase);
        Things = new List<string>(ThingsBase);

        // init dictionary
        dictionary.Clear();
        dictionary.Add(Verbs);
        dictionary.Add(People);
        dictionary.Add(Things);

        SwapRules.Clear();

        switch (difficulty) {

            case 9:
                // Everything is fucked up like hell
                ShuffleWithin(ThingsBase, Things, 3);
                ShuffleTroughout(PeopleBase, VerbsBase, People, Verbs, 3);
                break;

            case 8:
                // Everything is even more fucked up
                ShuffleWithin(ThingsBase, Things, 3);
                ShuffleBetween(PeopleBase, VerbsBase, People, Verbs, 2);
                break;

            case 7:
                // Everything is fucked up
                ShuffleWithin(VerbsBase, Verbs, 1);
                ShuffleTroughout(PeopleBase, ThingsBase, People, Things, 3);
                break;
            
            case 6:    
                // Peoples are mixed with Things
                ShuffleTroughout(PeopleBase, ThingsBase, People, Things, 3);
                break;

            case 5:
                // People are swapped with things
                ShuffleBetween(PeopleBase, ThingsBase, People, Things, 3);
                break;

            case 4:
                // Two verbs are swapped, and things are shuffled together
                ShuffleWithin(VerbsBase, Verbs, 1);
                ShuffleWithin(ThingsBase, Things, 2);
                break;

            case 3:
                // Things are shuffled together
                ShuffleWithin(ThingsBase, Things, 2);
                break;

            case 2:
                // Two verbs are swapped
                ShuffleWithin(VerbsBase,Verbs, 1);
                break;

            case 1:
                // Two people are swapped
                ShuffleWithin(PeopleBase, People, 1);
                break;

            case 0:
            default:
            // the langage is in understandable
                break;
        }

        /*Debug.Log("Shuffle()");
        Debug.Log("People");
        Debug.Log(PrintedList(People));
        Debug.Log("Verbs");
        Debug.Log(PrintedList(Verbs));
        Debug.Log("Things");
        Debug.Log(PrintedList(Things));
        */
        //Debug.Log("SwapRules");
        //Debug.Log(PrintedList(SwapRules));

        string rules_text = "";
        if (SwapRules.Count > 0)
            rules_text = "Beware of rules:\n";
        foreach(string s in SwapRules) {
            rules_text += s + "\n";
        };

        if (rules_text == "")
        {
            RulesText.text = "Let's send a team member to the exit.\n" +
            "Gosh, a door... Let's grab the key first.";
        }
        else
        {
            RulesText.text = rules_text;
        }
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
            j = GetSwapCandidate(ref inputList, ref swapped);

            // pick second candidate to swap
            k = GetSwapCandidate(ref inputList, ref swapped);

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
            j = GetSwapCandidate(ref inputList1, ref swapped1);

            // pick second candidate to swap
            k = GetSwapCandidate(ref inputList2, ref swapped2);

            // do the swap
            outputList1[j] = inputList2[k];
            outputList2[k] = inputList1[j];

            // record rule
            SwapRules.Add(outputList1[j] + " <--> " + outputList2[k]);
        }
    }

    void ShuffleTroughout(
        List<string> inputList1, List<string> inputList2,
        List<string> outputList1, List<string> outputList2,
        int nb_swap)
    {
        List<bool> swapped1 = new List<bool>();
        List<bool> swapped2 = new List<bool>();
        int i, j = 0, k = 0, t1 = 0, t2 = 0;

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
            t1 = Random.Range(0, 2);
            if (t1 == 0)
            {
                j = GetSwapCandidate(ref inputList1, ref swapped1);
            } else {
                j = GetSwapCandidate(ref inputList2, ref swapped2);
            }

            // pick second candidate to swap
            t2 = Random.Range(0, 2);
            if (t2 == 0)
            {
                k = GetSwapCandidate(ref inputList1, ref swapped1);
            }
            else
            {
                k = GetSwapCandidate(ref inputList2, ref swapped2);
            }

            // do the swap and record rules
            if (t1 == 0) {
                if (t2 == 0) {
                    outputList1[j] = inputList1[k];
                    outputList1[k] = inputList1[j];
                    SwapRules.Add(outputList1[j] + " <--> " + outputList1[k]);
                } else {
                    outputList1[j] = inputList2[k];
                    outputList2[k] = inputList1[j];
                    SwapRules.Add(outputList1[j] + " <--> " + outputList2[k]);
                }
            
            } else {
                if (t2 == 0) {
                    outputList2[j] = inputList1[k];
                    outputList1[k] = inputList2[j];
                    SwapRules.Add(outputList2[j] + " <--> " + outputList1[k]);
                } else {
                    outputList2[j] = inputList2[k];
                    outputList2[k] = inputList2[j];
                    SwapRules.Add(outputList2[j] + " <--> " + outputList2[k]);
                }
            }
        }
    }

    int GetSwapCandidate(ref List<string> InputList, ref List<bool> Swapped) {
        
        int res = Random.Range(0, InputList.Count);
        while (Swapped[res])
        {
            res++;
            if (res == InputList.Count)
                res = 0;
        }
        Swapped[res] = true;

        return res;
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
